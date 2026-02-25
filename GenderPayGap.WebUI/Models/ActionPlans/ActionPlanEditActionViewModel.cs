using GenderPayGap.Core;
using GenderPayGap.Core.Helpers;
using GenderPayGap.Database;
using GovUkDesignSystemDotNet;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenderPayGap.WebUI.Models.ActionPlans;

public class ActionPlanEditActionViewModel
{
    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public Organisation Organisation { get; set; }

    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public int ReportingYear { get; set; }
    
    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public Actions Action { get; set; }

    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public ActionTag ActionTag { get; set; }
    
    [GovUkValidateRequiredIf(IsRequiredPropertyName = nameof(StatusRequired), ErrorMessageIfMissing = "To save your supporting text, you must select a status")]
    public ActionStatus? Status { get; set; }

    public bool StatusRequired => !string.IsNullOrWhiteSpace(SupportingText);
    
    public string SupportingText { get; set; }
}
