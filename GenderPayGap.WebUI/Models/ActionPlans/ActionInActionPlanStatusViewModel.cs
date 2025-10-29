using GenderPayGap.Database;
using GenderPayGap.Core;
using GovUkDesignSystemDotNet;

using Microsoft.AspNetCore.Mvc.ModelBinding;


public class ActionInActionPlanStatusViewModel
{

    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public Organisation Organisation { get; set; }
    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public int ReportingYear { get; set; }

    [GovUkValidateRequired(ErrorMessageIfMissing = "Please select a status for this action")]
    public ActionStatus? ActionStatus { get; set; }

}



public enum ActionStatusesLabels3
{
    [GovUkRadioCheckboxLabelText(Text = "We have already embedded this in our working culture")]
    Embedded,

    [GovUkRadioCheckboxLabelText(Text = "We are working on embedding this but it is not yet routine")]
    InProgress,

    [GovUkRadioCheckboxLabelText(Text = "We would like to consider this practice - provisionally add it to our action plan")]
    AddToPlan,

    [GovUkRadioCheckboxLabelText(Text = "We are not currently pursuing this practice")]
    NotPursuing
}


