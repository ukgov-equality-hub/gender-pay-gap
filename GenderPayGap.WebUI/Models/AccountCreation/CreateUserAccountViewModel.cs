using GenderPayGap.WebUI.Models.ValidationAttributes;
using GovUkDesignSystemDotNet;

namespace GenderPayGap.WebUI.Models.AccountCreation;

public class CreateUserAccountViewModel 
{

    [GovUkValidateRequired(ErrorMessageIfMissing = "Enter an email address.")]
    [GovUkValidateCharacterCount(Limit = 255, Units = CharacterCountMaxLengthUnit.Characters, NameAtStartOfSentence = "Email address", NameWithinSentence = "email address")]
    public string EmailAddress { get; set; }

    [GovUkValidateRequired(ErrorMessageIfMissing = "Confirm your email address.")]
    public string ConfirmEmailAddress { get; set; }

    [GovUkValidateCharacterCount(Limit = 50, Units = CharacterCountMaxLengthUnit.Characters, NameAtStartOfSentence = "First name", NameWithinSentence = "first name")]
    [GovUkValidateRequired(ErrorMessageIfMissing = "Enter your first name.")]
    public string FirstName { get; set; }

    [GovUkValidateCharacterCount(Limit = 50, Units = CharacterCountMaxLengthUnit.Characters, NameAtStartOfSentence = "Last name", NameWithinSentence = "last name")]
    [GovUkValidateRequired(ErrorMessageIfMissing = "Enter your last name.")]
    public string LastName { get; set; }

    [GovUkValidateCharacterCount(Limit = 50, Units = CharacterCountMaxLengthUnit.Characters, NameAtStartOfSentence = "Job title", NameWithinSentence = "job title")]
    [GovUkValidateRequired(ErrorMessageIfMissing = "Enter your job title.")]
    public string JobTitle { get; set; }

    [GpgPasswordValidation]
    [GovUkValidateRequired(ErrorMessageIfMissing = "Enter a password.")]
    public string Password { get; set; }

    [GovUkValidateRequired(ErrorMessageIfMissing = "Confirm your password.")]
    public string ConfirmPassword { get; set; }

    public bool SendUpdates { get; set; }

    public bool AllowContact { get; set; }

    public bool IsPartOfGovUkReportingJourney { get; set; }

}