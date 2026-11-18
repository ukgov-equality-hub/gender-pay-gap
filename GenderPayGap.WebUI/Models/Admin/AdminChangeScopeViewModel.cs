using GenderPayGap.Core;
using GovUkDesignSystemDotNet;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenderPayGap.WebUI.Models.Admin;

public class AdminChangeScopeViewModel 
{

    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public long OrganisationId { get; set; }
    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public string OrganisationName { get; set; }
    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public int ReportingYear { get; set; }
    
    [GovUkValidateRequired(ErrorMessageIfMissing = "Please select a scope")]
    public ScopeStatuses? ScopeStatus { get; set; }
    public bool? ReadGuidance { get; set; }
    public string StatusDetails { get; set; }

    [GovUkValidateRequired(ErrorMessageIfMissing = "Please enter a reason for this change.")]
    [GovUkValidateCharacterCount(Limit = 250, Units = CharacterCountMaxLengthUnit.Characters, NameAtStartOfSentence = "Reason", NameWithinSentence = "Reason")]
    public string Reason { get; set; }

}

public enum NewScopeStatus
{

    InScope = 0,
    OutOfScope = 1

}
