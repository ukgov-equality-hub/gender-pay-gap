using GenderPayGap.Core;
using GenderPayGap.Database;
using GovUkDesignSystemDotNet;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenderPayGap.WebUI.Models.Scope;

public class DeclareScopeForYearViewModel
{
        
    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public Organisation Organisation { get; set; }

    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public int ReportingYear { get; set; }

    [GovUkValidateRequired(ErrorMessageIfMissing = "Select whether or not this employer is required to report.")]
    public ScopeStatuses? Scope { get; set; }

    [GovUkValidateRequiredIf(
        IsRequiredPropertyName = nameof(WhyOutOfScopeRequired),
        ErrorMessageIfMissing = "Select a reason why this employer is not required to report."
    )]
    public DeclareScopeForYearWhyOutOfScope? WhyOutOfScope { get; set; }
    public bool WhyOutOfScopeRequired => Scope == ScopeStatuses.OutOfScope;

    [GovUkValidateRequiredIf(
        IsRequiredPropertyName = nameof(WhyOutOfScopeDetailsRequired),
        ErrorMessageIfMissing = "Provide a reason that this employer is not required to report."
    )]
    [GovUkValidateCharacterCount(Limit = 250, Units = CharacterCountMaxLengthUnit.Characters, NameAtStartOfSentence = "Reason", NameWithinSentence = "Reason")]
    public string WhyOutOfScopeDetails { get; set; }
    public bool WhyOutOfScopeDetailsRequired => WhyOutOfScope == DeclareScopeForYearWhyOutOfScope.Other;
    
    [GovUkValidateRequiredIf(
        IsRequiredPropertyName = nameof(ReadGuidanceRequired),
        ErrorMessageIfMissing = "Select whether or not you have read the guidance."
    )]
    public ReadGuidanceYesNo? ReadGuidance { get; set; }
    public bool ReadGuidanceRequired => Scope == ScopeStatuses.OutOfScope;

}

public enum DeclareScopeForYearWhyOutOfScope
{
    /*
     * There would normally be an annotation here, but we want the label to read as
     * "Why is [OrganisationName] not required to report for reporting year [ReportingYears]?"
     * So this needs to change depending on the organisation and year.
     * The label is set in DeclareScopeForYear.cshtml
     */
    Under250 = 0,

    [GovUkRadioCheckboxLabelText(Text = "Other reason")]
    Other = 1
}

public enum ReadGuidanceYesNo
{
    Yes,
    No
}
