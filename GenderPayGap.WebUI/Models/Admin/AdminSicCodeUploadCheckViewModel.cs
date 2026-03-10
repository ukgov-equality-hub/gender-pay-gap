using GenderPayGap.Database;
using GenderPayGap.WebUI.Models.AdminReferenceData;
using GovUkDesignSystemDotNet;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenderPayGap.WebUI.Models.Admin;

public class AdminSicCodeUploadCheckViewModel 
{

    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public AddsEditsDeletesSet<SicCode> AddsEditsDeletesSet { get; set; }

    public string SerializedNewRecords { get; set; }

    [GovUkValidateRequired(ErrorMessageIfMissing = "Please enter a reason for this change.")]
    [GovUkValidateCharacterCount(Limit = 250, Units = CharacterCountMaxLengthUnit.Characters, NameAtStartOfSentence = "Reason", NameWithinSentence = "Reason")]
    public string Reason { get; set; }

}