using GovUkDesignSystemDotNet;

namespace GenderPayGap.WebUI.Models.SendFeedback;

public class FeedbackViewModel 
{
    [GovUkValidateRequired(ErrorMessageIfMissing = "Select how easy it was to submit your gender pay gap data")]
    public HowEasyWasItToSubmitYourGenderPayGapData? HowEasyWasItToSubmitYourGenderPayGapData { get; set; }

    [GovUkValidateRequired(ErrorMessageIfMissing = "Select how easy it was to create your action plan")]
    public HowEasyWasItToCreateYourActionPlan? HowEasyWasItToCreateYourActionPlan { get; set; }

    public List<HowDidYouHearAboutGpg> HowDidYouHearAboutGpg { get; set; } = [];

    [GovUkValidateCharacterCount(Limit = 2000, Units = CharacterCountMaxLengthUnit.Characters, NameAtStartOfSentence = "Other source", NameWithinSentence = "other source")]
    public string OtherSourceText { get; set; }

    public List<WhyVisitGpgSite> WhyVisitGpgSite { get; set; } = [];

    [GovUkValidateCharacterCount(Limit = 2000, Units = CharacterCountMaxLengthUnit.Characters, NameAtStartOfSentence = "Other reason", NameWithinSentence = "other reason")]
    public string OtherReasonText { get; set; }

    public List<WhoAreYou> WhoAreYou { get; set; } = [];

    [GovUkValidateCharacterCount(Limit = 2000, Units = CharacterCountMaxLengthUnit.Characters, NameAtStartOfSentence = "Other person", NameWithinSentence = "other person")]
    public string OtherPersonText { get; set; }

    [GovUkValidateCharacterCount(Limit = 2000, Units = CharacterCountMaxLengthUnit.Characters, NameAtStartOfSentence = "Details", NameWithinSentence = "details")]
    public string Details { get; set; }

    public string YourName { get; set; }

    public string EmailAddress { get; set; }

}

    
public enum HowEasyWasItToSubmitYourGenderPayGapData
{

    [GovUkRadioCheckboxLabelText(Text = "Very easy")]
    VeryEasy = 0,

    [GovUkRadioCheckboxLabelText(Text = "Easy")]
    Easy = 1,

    [GovUkRadioCheckboxLabelText(Text = "Neither easy nor difficult")]
    Neutral = 2,

    [GovUkRadioCheckboxLabelText(Text = "Difficult")]
    Difficult = 3,

    [GovUkRadioCheckboxLabelText(Text = "Very difficult")]
    VeryDifficult = 4,

    [GovUkRadioCheckboxLabelText(Text = "Not applicable")]
    NotApplicable = 5,

}

public enum HowEasyWasItToCreateYourActionPlan
{

    [GovUkRadioCheckboxLabelText(Text = "Very easy")]
    VeryEasy = 0,

    [GovUkRadioCheckboxLabelText(Text = "Easy")]
    Easy = 1,

    [GovUkRadioCheckboxLabelText(Text = "Neither easy nor difficult")]
    Neutral = 2,

    [GovUkRadioCheckboxLabelText(Text = "Difficult")]
    Difficult = 3,

    [GovUkRadioCheckboxLabelText(Text = "Very difficult")]
    VeryDifficult = 4,

    [GovUkRadioCheckboxLabelText(Text = "Not applicable")]
    NotApplicable = 5,

}

public enum HowDidYouHearAboutGpg
{

    [GovUkRadioCheckboxLabelText(Text = "News article")]
    NewsArticle,

    [GovUkRadioCheckboxLabelText(Text = "Social media")]
    SocialMedia,

    [GovUkRadioCheckboxLabelText(Text = "Company intranet")]
    CompanyIntranet,

    [GovUkRadioCheckboxLabelText(Text = "Employer union")]
    EmployerUnion,

    [GovUkRadioCheckboxLabelText(Text = "Internet search for a company")]
    InternetSearch,

    [GovUkRadioCheckboxLabelText(Text = "Charity")]
    Charity,

    [GovUkRadioCheckboxLabelText(Text = "Lobby group")]
    LobbyGroup,

    [GovUkRadioCheckboxLabelText(Text = "By having to report gender pay gap data")]
    Report,

    [GovUkRadioCheckboxLabelText(Text = "Other")]
    Other

}

public enum WhyVisitGpgSite
{

    [GovUkRadioCheckboxLabelText(Text = "Find out more about the gender pay gap")]
    FindOutAboutGpg,

    [GovUkRadioCheckboxLabelText(Text = "Submit gender pay gap data for my organisation")]
    ReportOrganisationGpgData,

    [GovUkRadioCheckboxLabelText(Text = "Find out how my organisation can close its gap")]
    CloseOrganisationGpg,

    [GovUkRadioCheckboxLabelText(Text = "Look at gender pay gap data for specific organisations or sectors")]
    ViewSpecificOrganisationGpg,

    [GovUkRadioCheckboxLabelText(Text = "Find out more about creating an action plan")]
    FindOutMoreAboutCreatingAnActionPlan,

    [GovUkRadioCheckboxLabelText(Text = "Create an action plan for my organisation")]
    CreateAnActionPlanForMyOrganisation,

    [GovUkRadioCheckboxLabelText(Text = "Look at action plans for specific organisations or sectors")]
    LookAtActionPlansForOrganisationsOrSectors,

    [GovUkRadioCheckboxLabelText(Text = "Other")]
    Other

}

public enum WhoAreYou
{

    [GovUkRadioCheckboxLabelText(Text = "Employee interested in my organisation’s gender pay gap data")]
    EmployeeInterestedInOrganisationData,

    [GovUkRadioCheckboxLabelText(Text = "Employee interested in my organisation’s action plan")]
    EmployeeInterestedInOrganisationActionPlan,

    [GovUkRadioCheckboxLabelText(Text = "Employee responsible for submitting my organisation’s gender pay gap data")]
    ResponsibleForReportingGpg,

    [GovUkRadioCheckboxLabelText(Text = "Employee responsible for submitting my organisation’s action plan")]
    EmployeeResponsibleForSubmittingActionPlan,

    [GovUkRadioCheckboxLabelText(Text = "Manager involved in gender pay gap reporting or diversity and inclusion")]
    ManagerInvolvedInGpgReport,

    [GovUkRadioCheckboxLabelText(Text = "Person interested in the gender pay gap generally")]
    PersonInterestedInGeneralGpg,

    [GovUkRadioCheckboxLabelText(Text = "Person interested in a specific organisation’s gender pay gap")]
    PersonInterestedInSpecificOrganisationGpg,

    [GovUkRadioCheckboxLabelText(Text = "Other")]
    Other

}
