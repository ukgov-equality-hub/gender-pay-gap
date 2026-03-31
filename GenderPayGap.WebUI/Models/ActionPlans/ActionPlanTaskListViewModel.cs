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
        if (ActionPlan != null && ActionPlan.HasAtLeastOneNewOrInProgressMenopauseAction())
        {
            return new TagViewModel
            {
                HtmlOrText = new("Complete"),
                Classes = ["govuk-tag--green"]
            };
        }

        if (ActionPlan != null && ActionPlan.HasAnyGenderPayGapActions())
        {
            return new TagViewModel
            {
                HtmlOrText = new("In progress"),
                Classes = ["govuk-tag--yellow"]
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
        if (ActionPlan != null && ActionPlan.HasAtLeastOneNewOrInProgressMenopauseAction())
        {
            return new TagViewModel
            {
                HtmlOrText = new("Complete"),
                Classes = ["govuk-tag--green"]
            };
        }

        if (ActionPlan != null && ActionPlan.HasAnyMenopauseActions())
        {
            return new TagViewModel
            {
                HtmlOrText = new("In progress"),
                Classes = ["govuk-tag--yellow"]
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
                Classes = ["govuk-tag--yellow"],
            };
        }
    }

    public TagViewModel GetResponsiblePersonStatusTag()
    {
        bool hasSpecifiedFirstName = !string.IsNullOrWhiteSpace(ActionPlan?.ResponsiblePersonFirstName);
        bool hasSpecifiedLastName = !string.IsNullOrWhiteSpace(ActionPlan?.ResponsiblePersonLastName);
        bool hasSpecifiedJobTitle = !string.IsNullOrWhiteSpace(ActionPlan?.ResponsiblePersonJobTitle);

        if (hasSpecifiedFirstName && hasSpecifiedLastName && hasSpecifiedJobTitle)
        {
            return new TagViewModel
            {
                HtmlOrText = new("Complete"),
                Classes = ["govuk-tag--green"],
            };
        }
        else if (!hasSpecifiedFirstName && !hasSpecifiedLastName && !hasSpecifiedJobTitle)
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
                Classes = ["govuk-tag--yellow"],
            };
        }
    }

}
