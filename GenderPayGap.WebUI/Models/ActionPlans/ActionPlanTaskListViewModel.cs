using GenderPayGap.Core;
using GenderPayGap.Core.Helpers;
using GenderPayGap.Database;
using GovUkDesignSystemDotNet;

namespace GenderPayGap.WebUI.Models.ActionPlans;

public class ActionPlanTaskListViewModel
{
    public Organisation Organisation { get; set; }
    public int ReportingYear { get; set; }
    public ActionPlan ActionPlan { get; set; }

    public TagViewModel GetGenderPayGapActionsStatusTag()
    {
        List<Actions> gpgActions = ActionsHelper.ListOfGenderPayGapActions.Select(a => a.Action).ToList();
        
        if (ActionPlan != null &&
            ActionPlan.GetNewOrInProgressActions().Any(aiap => gpgActions.Contains(aiap.Action)))
        {
            return new TagViewModel
            {
                HtmlOrText = new("Complete"),
                Classes = ["govuk-tag--green"]
            };
        }

        return new TagViewModel
        {
            HtmlOrText = new("Not started"),
            Classes = ["govuk-tag--blue"]
        };
    }

    public TagViewModel GetMenopauseActionsStatusTag()
    {
        List<Actions> menopauseActions = ActionsHelper.ListOfMenopauseActions.Select(a => a.Action).ToList();
        
        if (ActionPlan != null &&
            ActionPlan.GetNewOrInProgressActions().Any(aiap => menopauseActions.Contains(aiap.Action)))
        {
            return new TagViewModel
            {
                HtmlOrText = new("Complete"),
                Classes = ["govuk-tag--green"]
            };
        }

        return new TagViewModel
        {
            HtmlOrText = new("Not started"),
            Classes = ["govuk-tag--blue"]
        };
    }

    public TagViewModel GetSupportingNarrativeStatusTag()
    {
        bool hasSupportingNarrative = !string.IsNullOrWhiteSpace(ActionPlan?.SupportingNarrative);
        bool hasLinkToReport = !string.IsNullOrWhiteSpace(ActionPlan?.LinkToReport);

        if (hasSupportingNarrative && hasLinkToReport)
        {
            return new TagViewModel
            {
                HtmlOrText = new("Complete"),
                Classes = ["govuk-tag--green"],
            };
        }
        else if (!hasSupportingNarrative && !hasLinkToReport)
        {
            return new TagViewModel
            {
                HtmlOrText = new("Not started"),
                Classes = ["govuk-tag--blue"],
            };
        }
        else
        {
            return new TagViewModel
            {
                HtmlOrText = new("In progress"),
                Classes = ["govuk-tag--blue"],
            };
        }
    }

}
