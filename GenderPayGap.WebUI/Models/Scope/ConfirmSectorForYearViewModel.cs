using GenderPayGap.Core;
using GenderPayGap.Database;
using GovUkDesignSystemDotNet;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenderPayGap.WebUI.Models.Scope;

public class ConfirmSectorForYearViewModel
{
        
    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public Organisation Organisation { get; set; }

    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public int ReportingYear { get; set; }

    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public Sectors? Sector { get; set; }

    [GovUkValidateRequired(ErrorMessageIfMissing = "Select whether or not you are happy with the proposed sector")]
    public ConfirmSectorForYearYesNoEnum? ConfirmYesNo { get; set; }

}

public enum ConfirmSectorForYearYesNoEnum
{
    Yes,
    No
}
