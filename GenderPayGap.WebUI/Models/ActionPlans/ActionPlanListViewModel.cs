using GenderPayGap.Core.Helpers;
using GenderPayGap.Database;

namespace GenderPayGap.WebUI.Models.ActionPlans;

public class ActionPlanListViewModel
{
    public Organisation Organisation { get; set; }
    public int ReportingYear { get; set; }
    public ActionPlan ActionPlan { get; set; }
    public ActionTag ActionTag { get; set; }

}
