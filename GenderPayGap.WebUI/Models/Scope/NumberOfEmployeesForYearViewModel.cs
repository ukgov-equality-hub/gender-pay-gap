using GenderPayGap.Core;
using GenderPayGap.Database;
using GovUkDesignSystemDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenderPayGap.WebUI.Models.Scope;

public class NumberOfEmployeesForYearViewModel
{
        
    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public Organisation Organisation { get; set; }

    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public int ReportingYear { get; set; }

    [ModelBinder<GovUkOptionalIntBinder>]
    [GovUkOptionalIntErrorText(nameAtStartOfSentence: "Number of employees")]
    [GovUkValidateRequiredIf(IsRequiredPropertyName = nameof(NumberOfEmployeesRequired), ErrorMessageIfMissing = "Enter the number of employees on the snapshot date")]
    public int? NumberOfEmployees { get; set; }
    public bool NumberOfEmployeesRequired => !SkippedNumberOfEmployees;

    public bool SkippedNumberOfEmployees { get; set; }

}
