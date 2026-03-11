using GenderPayGap.Database;
using GenderPayGap.WebUI.ExternalServices.CompaniesHouse;
using GovUkDesignSystemDotNet;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenderPayGap.WebUI.Models.Admin;

public class AdminChangeCompaniesHouseOptInOutViewModel 
{

    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public Organisation Organisation { get; set; }

    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public CompaniesHouseCompany CompaniesHouseCompany { get; set; }

    [GovUkValidateRequired(ErrorMessageIfMissing = "Please enter a reason for this change.")]
    [GovUkValidateCharacterCount(Limit = 250, Units = CharacterCountMaxLengthUnit.Characters, NameAtStartOfSentence = "Reason", NameWithinSentence = "Reason")]
    public string Reason { get; set; }

}
