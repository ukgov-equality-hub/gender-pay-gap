using System.Diagnostics;
using GenderPayGap.Core;
using GenderPayGap.Core.Helpers;
using GenderPayGap.Extensions;

namespace GenderPayGap.Database;

[Serializable]
[DebuggerDisplay("{OrganisationName},{Status}")]
public partial class ActionPlan
{

    public bool HasFulfilledRequirementsToPublish()
    {
        if (ActionsInActionPlans.Count(a => ActionsHelper.DictionaryOfAllActions[a.Action].Category == ActionCategories.SupportingStaffDuringMenopause) == 0)
        {
            return false;
        }
        if (ActionsInActionPlans.Count(a => ActionsHelper.DictionaryOfAllActions[a.Action].Category != ActionCategories.SupportingStaffDuringMenopause) == 0)
        {
            return false;
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