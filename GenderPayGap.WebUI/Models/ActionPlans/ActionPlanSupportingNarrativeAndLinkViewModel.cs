using GenderPayGap.Database;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenderPayGap.WebUI.Models.ActionPlans;

public class ActionPlanSupportingNarrativeAndLinkViewModel
{
    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public Organisation Organisation { get; set; }

    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public int ReportingYear { get; set; }

    public string SupportingNarrative { get; set; }
    
    public string LinkToReport { get; set; }
}
