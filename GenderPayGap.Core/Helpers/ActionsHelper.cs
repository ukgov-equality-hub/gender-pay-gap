using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace GenderPayGap.Core.Helpers;

public enum ActionCategories
{
    [Display(Name = "Recruiting staff")]
    RecruitingStaff = 1,
    [Display(Name = "Developing and promoting staff")]
    DevelopingAndPromotingStaff = 2,
    [Display(Name = "Building diversity into your organisation")]
    BuildingDiversityIntoYourOrganisation = 3,
    [Display(Name = "Increasing transparency")]
    IncreasingTransparency = 4,
    [Display(Name = "Supporting employees experiencing menopause")]
    SupportingEmployeesExperiencingMenopause = 5,
}

public static class ActionCategoriesExtensions
{
    public static string GetDisplayName(this ActionCategories actionCategory)
    {
        DisplayAttribute displayAttribute = actionCategory.GetType()
            .GetMember(actionCategory.ToString())
            .FirstOrDefault()
            ?.GetCustomAttribute<DisplayAttribute>();
                
        return displayAttribute?.Name;
    }
}

public enum ActionTag
{
    [Display(Name = "Select actions to address your gender pay gap")]
    GenderPayGap,
    [Display(Name = "Select actions to support employees experiencing menopause")]
    Menopause
}

public static class ActionTagExtensions
{
    public static string GetDisplayName(this ActionTag actionCategory)
    {
        DisplayAttribute displayAttribute = actionCategory.GetType()
            .GetMember(actionCategory.ToString())
            .FirstOrDefault()
            ?.GetCustomAttribute<DisplayAttribute>();
                
        return displayAttribute?.Name;
    }
}

public class ActionDetails
{

    public Actions Action { get; set; }
    public ActionCategories Category { get; set; }
    public List<ActionTag> Tags { get; set; }
    public string Name { get; set; }
    public string Summary { get; set; }
    public string GuidanceUrl { get; set; }
}

public static class ActionsHelper
{
    public static readonly Dictionary<Actions, ActionDetails> DictionaryOfAllActions = new()
    {
        {
            Actions.MakeJobDescriptionsInclusive, new()
            {
                Action = Actions.MakeJobDescriptionsInclusive,
                Category = ActionCategories.RecruitingStaff,
                Tags = [ActionTag.GenderPayGap],
                Name = "Make job descriptions inclusive",
                Summary = "Inclusive job descriptions can attract diverse talent by using neutral language, listing only essential requirements, and highlighting equal opportunities.",
                GuidanceUrl = "https://www.gov.uk/government/publications/make-job-descriptions-inclusive/make-job-descriptions-inclusive",
            }
        },
        {
            Actions.EncourageApplicationsFromARangeOfCandidates, new()
            {
                Action = Actions.EncourageApplicationsFromARangeOfCandidates,
                Category = ActionCategories.RecruitingStaff,
                Tags = [ActionTag.GenderPayGap],
                Name = "Encourage applications from a range of candidates",
                Summary = "Actively seeking applications from under-represented groups can ensure a broader range of applicants for all roles.",
                GuidanceUrl = "https://www.gov.uk/government/publications/encourage-applications-from-a-range-of-candidates/encourage-applications-from-a-range-of-candidates",
            }
        },
        {
            Actions.ReduceUnconsciousBiasInCvScreening, new()
            {
                Action = Actions.ReduceUnconsciousBiasInCvScreening,
                Category = ActionCategories.RecruitingStaff,
                Tags = [ActionTag.GenderPayGap],
                Name = "Reduce unconscious bias in CV screening",
                Summary = "Using structured, skill-based screening can minimise bias and boost diversity in hiring.",
                GuidanceUrl = "https://www.gov.uk/government/publications/reduce-unconscious-bias-in-cv-screening/reduce-unconscious-bias-in-cv-screening",
            }
        },
        {
            Actions.UseFairAndStructuredInterviewTechniques, new()
            {
                Action = Actions.UseFairAndStructuredInterviewTechniques,
                Category = ActionCategories.RecruitingStaff,
                Tags = [ActionTag.GenderPayGap],
                Name = "Use fair and structured interview techniques",
                Summary = "Structured interviews support fair, objective hiring. Standardised questions and scoring helps reduce bias and promotes equal opportunity.",
                GuidanceUrl = "https://www.gov.uk/government/publications/use-fair-and-structured-interview-techniques/use-fair-and-structured-interview-techniques",
            }
        },
        {
            Actions.AdvertiseLeavePoliciesInJobAdverts, new()
            {
                Action = Actions.AdvertiseLeavePoliciesInJobAdverts,
                Category = ActionCategories.RecruitingStaff,
                Tags = [ActionTag.GenderPayGap],
                Name = "Advertise leave policies in job adverts",
                Summary = "Advertise parental, carer, and compassionate leave policies widely to ensure all prospective employees know the entitlements they would be eligible for.",
                GuidanceUrl = "https://www.gov.uk/government/publications/advertise-leave-policies-in-job-adverts/advertise-leave-policies-in-job-adverts",
            }
        },
        {
            Actions.AdvertiseFlexibleWorkingArrangementsInJobAdverts, new()
            {
                Action = Actions.AdvertiseFlexibleWorkingArrangementsInJobAdverts,
                Category = ActionCategories.RecruitingStaff,
                Tags = [ActionTag.GenderPayGap, ActionTag.Menopause],
                Name = "Advertise flexible working arrangements in job adverts",
                Summary = "Flexible working policies can aid work-life balance, especially for people with caring roles. Advertising flexibility can attract a wider, more diverse group of applicants."
            }
        },
        {
            Actions.AutomaticallyConsiderEligibleEmployeesForPromotion, new()
            {
                Action = Actions.AutomaticallyConsiderEligibleEmployeesForPromotion,
                Category = ActionCategories.DevelopingAndPromotingStaff,
                Tags = [ActionTag.GenderPayGap],
                Name = "Automatically consider eligible employees for promotion",
                Summary = "Automatically considering all eligible employees for promotion gives them the choice to opt-out rather than opt-in.",
                GuidanceUrl = "https://www.gov.uk/government/publications/automatically-consider-eligible-employees-for-promotion/automatically-consider-eligible-employees-for-promotion",
            }
        },
        {
            Actions.EncourageEmployeeDevelopmentThroughActionableSteps, new()
            {
                Action = Actions.EncourageEmployeeDevelopmentThroughActionableSteps,
                Category = ActionCategories.DevelopingAndPromotingStaff,
                Tags = [ActionTag.GenderPayGap],
                Name = "Encourage employee development through actionable steps",
                Summary = "Giving all employees clear and actionable advice on how to develop may benefit organisations with low rates of progression and retention for women.",
                GuidanceUrl = "https://www.gov.uk/government/publications/encourage-employee-development-through-actionable-steps/encourage-employee-development-through-actionable-steps",
            }
        },
        {
            Actions.OfferMentoringSponsorshipAndOtherDevelopmentProgrammes, new()
            {
                Action = Actions.OfferMentoringSponsorshipAndOtherDevelopmentProgrammes,
                Category = ActionCategories.DevelopingAndPromotingStaff,
                Tags = [ActionTag.GenderPayGap],
                Name = "Offer mentoring, sponsorship and other development programmes",
                Summary = "Providing development programmes, such as mentoring, gives employees a formal channel for advice and support.",
                GuidanceUrl = "https://www.gov.uk/government/publications/offer-mentoring-sponsorship-and-other-development-programmes/offer-mentoring-sponsorship-and-other-development-programmes",
            }
        },
        {
            Actions.SetTargetsToImproveGenderRepresentation, new()
            {
                Action = Actions.SetTargetsToImproveGenderRepresentation,
                Category = ActionCategories.BuildingDiversityIntoYourOrganisation,
                Tags = [ActionTag.GenderPayGap],
                Name = "Set targets to improve gender representation",
                Summary = "Setting specific internal targets that you can monitor using data gives your organisation clear steps to improve gender representation and equality.",
                GuidanceUrl = "https://www.gov.uk/government/publications/set-targets-to-improve-gender-representation/set-targets-to-improve-gender-representation",
            }
        },
        {
            Actions.IncreaseTransparencyForPayPromotionAndRewards, new()
            {
                Action = Actions.IncreaseTransparencyForPayPromotionAndRewards,
                Category = ActionCategories.IncreasingTransparency,
                Tags = [ActionTag.GenderPayGap],
                Name = "Increase transparency for pay, promotion and rewards",
                Summary = "Transparency in pay, promotion, and bonus policies helps ensure everyone understands how decisions are made.",
                GuidanceUrl = "https://www.gov.uk/government/publications/increase-transparency-for-pay-promotion-and-rewards/increase-transparency-for-pay-promotion-and-rewards",
            }
        },
        {
            Actions.EnhanceAndPromoteFlexibleWorkingAndLeavePolicies, new()
            {
                Action = Actions.EnhanceAndPromoteFlexibleWorkingAndLeavePolicies,
                Category = ActionCategories.IncreasingTransparency,
                Tags = [ActionTag.GenderPayGap],
                Name = "Enhance and promote flexible working and leave policies",
                Summary = "Enhance and promote leave policies and flexible working so employees know their entitlements and how to use them.",
                GuidanceUrl = "https://www.gov.uk/government/publications/enhance-and-promote-flexible-working-and-leave-policies/enhance-and-promote-flexible-working-and-leave-policies",
            }
        },
        {
            Actions.TrainManagersToSupportEmployeesExperiencingMenopause, new()
            {
                Action = Actions.TrainManagersToSupportEmployeesExperiencingMenopause,
                Category = ActionCategories.SupportingEmployeesExperiencingMenopause,
                Tags = [ActionTag.Menopause],
                Name = "Train managers to support employees experiencing menopause",
                Summary = "Manager training can help organisations support employees experiencing menopause.",
                GuidanceUrl = "https://www.gov.uk/government/publications/train-managers-to-support-employees-experiencing-menopause/train-managers-to-support-employees-experiencing-menopause",
            }
        },
        {
            Actions.OfferOccupationalHealthAdviceForMenopause, new()
            {
                Action = Actions.OfferOccupationalHealthAdviceForMenopause,
                Category = ActionCategories.SupportingEmployeesExperiencingMenopause,
                Tags = [ActionTag.Menopause],
                Name = "Offer occupational health advice to employees experiencing menopause",
                Summary = "Giving employees specialised occupational health advice can help them manage menopause symptoms, get support and work more comfortably.",
                GuidanceUrl = "https://www.gov.uk/government/publications/offer-occupational-health-advice-for-employees-experiencing-menopause/offer-occupational-health-advice-for-employees-experiencing-menopause",
            }
        },
        {
            Actions.SetUpMenopauseSupportGroupsAndNetworks, new()
            {
                Action = Actions.SetUpMenopauseSupportGroupsAndNetworks,
                Category = ActionCategories.SupportingEmployeesExperiencingMenopause,
                Tags = [ActionTag.Menopause],
                Name = "Set up menopause support groups and networks",
                Summary = "Menopause support groups in your organisation can help provide peer support, information and guidance.",
                GuidanceUrl = "https://www.gov.uk/government/publications/set-up-menopause-support-groups-and-networks/set-up-menopause-support-groups-and-networks",
            }
        },
        {
            Actions.OfferWorkplaceAdjustmentsForMenopause, new()
            {
                Action = Actions.OfferWorkplaceAdjustmentsForMenopause,
                Category = ActionCategories.SupportingEmployeesExperiencingMenopause,
                Tags = [ActionTag.Menopause],
                Name = "Offer workplace adjustments to employees experiencing menopause",
                Summary = "Personalised workplace adjustments for employees experiencing menopause can support their wellbeing and ability to work.",
                GuidanceUrl = "https://www.gov.uk/government/publications/offer-workplace-adjustments-for-employees-experiencing-menopause/offer-workplace-adjustments-for-employees-experiencing-menopause",
            }
        },
        {
            Actions.ConductMenopauseRiskAssessmentForWorkplace, new()
            {
                Action = Actions.ConductMenopauseRiskAssessmentForWorkplace,
                Category = ActionCategories.SupportingEmployeesExperiencingMenopause,
                Tags = [ActionTag.Menopause],
                Name = "Conduct a menopause risk assessment for your workplace",
                Summary = "Menopause risk assessments can identify workplace adjustments to help support your employees' wellbeing.",
                GuidanceUrl = "https://www.gov.uk/government/publications/conduct-a-menopause-risk-assessment-for-your-workplace/conduct-a-menopause-risk-assessment-for-your-workplace",
            }
        },
        {
            Actions.ReviewPoliciesProceduresMenopause, new()
            {
                Action = Actions.ReviewPoliciesProceduresMenopause,
                Category = ActionCategories.SupportingEmployeesExperiencingMenopause,
                Tags = [ActionTag.Menopause],
                Name = "Review policies and procedures to meet the needs of employees experiencing menopause",
                Summary = "Ensure your organisation’s policies align with the needs of employees experiencing menopause by reviewing your policies and procedures.",
                GuidanceUrl = "https://www.gov.uk/government/publications/review-policies-and-procedures-to-meet-the-needs-of-employees-experiencing-menopause/review-policies-and-procedures-to-meet-the-needs-of-employees-experiencing-menopause",
            }
        }
    };
    
    public static readonly List<ActionDetails> ListOfAllActions = DictionaryOfAllActions.Values.ToList();
    
    public static readonly List<ActionDetails> ListOfGenderPayGapActions = DictionaryOfAllActions.Values.Where(a => a.Tags.Contains(ActionTag.GenderPayGap)).ToList();
    public static readonly List<ActionDetails> ListOfMenopauseActions = DictionaryOfAllActions.Values.Where(a => a.Tags.Contains(ActionTag.Menopause)).ToList();
}
