using GenderPayGap.Database;

namespace GenderPayGap.WebUI.Models.ActionPlans;

public class ActionPlanPreviewViewModel
{
    public Organisation Organisation { get; set; }
    public int ReportingYear { get; set; }
    public ActionPlan ActionPlan { get; set; }
}
