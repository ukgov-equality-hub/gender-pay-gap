using GenderPayGap.Database;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenderPayGap.WebUI.Models.ActionPlans;

public class ActionPlanResponsiblePersonViewModel
{
    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public Organisation Organisation { get; set; }

    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public int ReportingYear { get; set; }

    public string ResponsiblePersonFirstName { get; set; }
    public string ResponsiblePersonLastName { get; set; }
    public string ResponsiblePersonJobTitle { get; set; }
}
