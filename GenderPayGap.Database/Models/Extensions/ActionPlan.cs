using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using GenderPayGap.Core;
using GenderPayGap.Core.Helpers;
using GenderPayGap.Extensions;

namespace GenderPayGap.Database;

[Serializable]
[DebuggerDisplay("{OrganisationName},{Status}")]
public partial class ActionPlan
{

    [NotMapped]
    public string ResponsiblePerson
    {
        get
        {
            if (string.IsNullOrWhiteSpace(ResponsiblePersonFirstName) ||
                string.IsNullOrWhiteSpace(ResponsiblePersonLastName) ||
                string.IsNullOrWhiteSpace(ResponsiblePersonJobTitle))
            {
                return null;
            }

            return $"{ResponsiblePersonFirstName} {ResponsiblePersonLastName} ({ResponsiblePersonJobTitle})";
        }
    }

    public bool HasAtLeastOneNewOrInProgressGenderPayGapAction() => ActionsInActionPlans
        .Where(a => a.NewStatus == ActionStatus.NewOrInProgress)
        .Any(aiap => aiap.ActionDetails.Tags.Contains(ActionTag.GenderPayGap));
    
    public bool HasAnyCompletedGenderPayGapActions() => ActionsInActionPlans
        .Where(a => a.NewStatus == ActionStatus.Completed)
        .Any(aiap => aiap.ActionDetails.Tags.Contains(ActionTag.GenderPayGap));
    
    public bool HasAtLeastOneNewOrInProgressMenopauseAction() => ActionsInActionPlans
        .Where(a => a.NewStatus == ActionStatus.NewOrInProgress)
        .Any(aiap => aiap.ActionDetails.Tags.Contains(ActionTag.Menopause));

    public bool HasAnyCompletedMenopauseActions() => ActionsInActionPlans
        .Where(a => a.NewStatus == ActionStatus.Completed)
        .Any(aiap => aiap.ActionDetails.Tags.Contains(ActionTag.Menopause));
    
    public bool HasAtLeastTwoNewOrInProgressActions() => ActionsInActionPlans
        .Count(a => a.NewStatus == ActionStatus.NewOrInProgress) >= 2;

    public bool SingleSelectedActionHasBothGenderPayGapAndMenopauseTags()
    {
        return GetNewOrInProgressActions().Count == 1
               && GetNewOrInProgressActions()[0].ActionDetails.Tags.Contains(ActionTag.GenderPayGap)
               && GetNewOrInProgressActions()[0].ActionDetails.Tags.Contains(ActionTag.Menopause);
    }

    public bool HasCompletedSupportingNarrative() => !string.IsNullOrWhiteSpace(SupportingNarrative);
    
    public bool HasCompletedResponsiblePersonDetailsIfNeeded()
    {
        if (Organisation.SectorType == SectorTypes.Private)
        {
            if (string.IsNullOrWhiteSpace(ResponsiblePersonFirstName) ||
                string.IsNullOrWhiteSpace(ResponsiblePersonLastName) ||
                string.IsNullOrWhiteSpace(ResponsiblePersonJobTitle))
            {
                return false;
            }
        }

        return true;
    }

    public void SubmitActionPlan()
    {
        // Change this ActionPlan's status to Submitted
        Status = ActionPlanStatus.Submitted;
        SubmittedDate = VirtualDateTime.Now;

        // Retire any other Submitted ActionPlans for this Organisation and Reporting Year
        var otherSubmittedActionPlansForThisYear = Organisation.ActionPlans
            .Where(ap => ap != this)
            .Where(ap => ap.ReportingYear == ReportingYear)
            .Where(ap => ap.Status == ActionPlanStatus.Submitted);
        
        foreach (ActionPlan submittedActionPlan in otherSubmittedActionPlansForThisYear)
        {
            submittedActionPlan.Status = ActionPlanStatus.Retired;
        }
    }

    public void DeleteActionPlan()
    {
        switch (Status)
        {
            case ActionPlanStatus.Draft:
                Status = ActionPlanStatus.DeletedDraft;
                DeletedDate = VirtualDateTime.Now;
                break;
            
            case ActionPlanStatus.Submitted:
            case ActionPlanStatus.Retired:
                Status = ActionPlanStatus.Deleted;
                DeletedDate = VirtualDateTime.Now;
                break;
            
            case ActionPlanStatus.Deleted:
            case ActionPlanStatus.DeletedDraft:
            default:
                break;
        }
    }

    public List<ActionInActionPlan> GetNewOrInProgressActions() => ActionsInActionPlans.Where(a => a.NewStatus == ActionStatus.NewOrInProgress).ToList();

    public List<ActionInActionPlan> GetCompletedActions() => ActionsInActionPlans.Where(a => a.NewStatus == ActionStatus.Completed).ToList();

}