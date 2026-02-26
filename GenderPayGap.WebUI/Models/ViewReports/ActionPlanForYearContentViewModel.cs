using GenderPayGap.Database;

namespace GenderPayGap.WebUI.Models.ViewReports;

public class ActionPlanForYearContentViewModel
{

    public Organisation Organisation { get; set; }
    public int ReportingYear { get; set; }
    public ActionPlan ActionPlan { get; set; }

}
