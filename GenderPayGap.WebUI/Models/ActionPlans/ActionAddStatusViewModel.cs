using GenderPayGap.Database;
using GenderPayGap.Core;
using GovUkDesignSystemDotNet;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenderPayGap.WebUI.Models.ActionPlans
{
    public class ActionAddStatusViewModel 
    {

        [BindNever /* Output Only - only used for sending data from the Controller to the View */]
        public Organisation Organisation { get; set; }
        [BindNever /* Output Only - only used for sending data from the Controller to the View */]
        public int ReportingYear { get; set; }

        [GovUkValidateRequired(ErrorMessageIfMissing = "Please select a status for this action")]
        public ActionStatus? ActionStatus { get; set; }

    }
}
