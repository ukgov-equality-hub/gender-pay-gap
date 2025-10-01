using GenderPayGap.Database;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenderPayGap.WebUI.Models.ActionPlans
{
    public class ActionPlansProgressMadeViewModel 
    {

        [BindNever /* Output Only - only used for sending data from the Controller to the View */]
        public Organisation Organisation { get; set; }
        [BindNever /* Output Only - only used for sending data from the Controller to the View */]
        public int ReportingYear { get; set; }

        [GovUkValidateRequired(ErrorMessageIfMissing = "Tell us what progress you have made")]
        [GovUkValidateCharacterCount(MaxCharacters = 1000,
            NameAtStartOfSentence = "Your update",
            NameWithinSentence = "your update")]
        public string ProgressMade { get; set; }

    }
}
