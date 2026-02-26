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
    [Display(Name = "Analysing your data")]
    AnalysingYourData = 4,
    [Display(Name = "Supporting staff during menopause")]
    SupportingStaffDuringMenopause = 5,
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
    [Display(Name = "Select actions to reduce your gender pay gap")]
    GenderPayGap,
    [Display(Name = "Select actions to support women experiencing menopause")]
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
}

public static class ActionsHelper
{
    public static readonly Dictionary<Actions, ActionDetails> DictionaryOfAllActions = new()
    {
        {
            Actions.MakeJobAdvertsInclusive, new()
            {
                Action = Actions.MakeJobAdvertsInclusive,
                Category = ActionCategories.RecruitingStaff,
                Tags = [ActionTag.GenderPayGap],
                Name = "Make job adverts inclusive",
                Summary = "Inclusive job adverts attract diverse talent by using neutral language, listing only essential requirements, and highlighting equal opportunities."
            }
        },
        {
            Actions.EncourageApplicationsFromDiverseCandidates, new()
            {
                Action = Actions.EncourageApplicationsFromDiverseCandidates,
                Category = ActionCategories.RecruitingStaff,
                Tags = [ActionTag.GenderPayGap],
                Name = "Encourage applications from diverse candidates",
                Summary = "Actively seeking applications from underrepresented groups ensures a broader candidate pool for all roles."
            }
        },
        {
            Actions.ReduceUnconsciousBiasInCVScreening, new()
            {
                Action = Actions.ReduceUnconsciousBiasInCVScreening,
                Category = ActionCategories.RecruitingStaff,
                Tags = [ActionTag.GenderPayGap],
                Name = "Reduce unconscious bias in CV screening",
                Summary = "Using structured, skill-based screening minimises bias and boosts diversity in hiring."
            }
        },
        {
            Actions.RunStructuredInterviews, new()
            {
                Action = Actions.RunStructuredInterviews,
                Category = ActionCategories.RecruitingStaff,
                Tags = [ActionTag.GenderPayGap],
                Name = "Run structured interviews",
                Summary = "Running structured interviews ensures fair, objective hiring. Standardised questions and scoring helps reduce bias and promotes equal opportunity."
            }
        },
        {
            Actions.EnsureInterviewOutcomesAreFair, new()
            {
                Action = Actions.EnsureInterviewOutcomesAreFair,
                Category = ActionCategories.RecruitingStaff,
                Tags = [ActionTag.GenderPayGap],
                Name = "Ensure interview outcomes are fair",
                Summary = "Implementing structured interviews with trained assessors leads to objective, consistent, and evidence-based decisions"
            }
        },
        {
            Actions.PromoteFlexibleWorkingArrangementsInJobAdverts, new()
            {
                Action = Actions.PromoteFlexibleWorkingArrangementsInJobAdverts,
                Category = ActionCategories.RecruitingStaff,
                Tags = [ActionTag.GenderPayGap, ActionTag.Menopause],
                Name = "Promote flexible working arrangements in job adverts",
                Summary = "Flexible work policies aid work-life balance, especially for those with caring roles. Advertising flexibility attracts a wider, more diverse talent pool."
            }
        },
        {
            Actions.IncreaseTransparencyForPayPromotionAndRewards, new()
            {
                Action = Actions.IncreaseTransparencyForPayPromotionAndRewards,
                Category = ActionCategories.DevelopingAndPromotingStaff,
                Tags = [ActionTag.GenderPayGap],
                Name = "Increase transparency for pay, promotion and rewards",
                Summary = "Transparency in pay, promotion, and bonus policies ensures everyone understands how decisions are made."
            }
        },
        {
            Actions.AutomaticallyPutForwardEmployeesForPromotion, new()
            {
                Action = Actions.AutomaticallyPutForwardEmployeesForPromotion,
                Category = ActionCategories.DevelopingAndPromotingStaff,
                Tags = [ActionTag.GenderPayGap],
                Name = "Automatically put forward employees for promotion",
                Summary = "Automatically considering all eligible employees for promotion gives them the choice to opt-out rather than opt-in."
            }
        },
        {
            Actions.EncourageEmployeeDevelopmentThroughActionableSteps, new()
            {
                Action = Actions.EncourageEmployeeDevelopmentThroughActionableSteps,
                Category = ActionCategories.DevelopingAndPromotingStaff,
                Tags = [ActionTag.GenderPayGap],
                Name = "Encourage employee development through actionable steps",
                Summary = "Asking employees for advice instead of feedback can aid skill development, and benefit organisations with low female progression and retention."
            }
        },
        {
            Actions.OfferInternshipsMentoringAndOtherDevelopmentProgrammes, new()
            {
                Action = Actions.OfferInternshipsMentoringAndOtherDevelopmentProgrammes,
                Category = ActionCategories.DevelopingAndPromotingStaff,
                Tags = [ActionTag.GenderPayGap],
                Name = "Offer internships, mentoring and other development programmes",
                Summary = "Providing development programmes, such as internships, gives employees a formal channel for advice, support, and advocacy."
            }
        },
        {
            Actions.SetTargetsToImproveGenderRepresentation, new()
            {
                Action = Actions.SetTargetsToImproveGenderRepresentation,
                Category = ActionCategories.BuildingDiversityIntoYourOrganisation,
                Tags = [ActionTag.GenderPayGap],
                Name = "Set targets to improve gender representation",
                Summary = "Setting specific, monitorable internal targets gives your organisation clear steps to boost gender representation and equality."
            }
        },
        {
            Actions.AppointDiversityLeadsOrTaskforces, new()
            {
                Action = Actions.AppointDiversityLeadsOrTaskforces,
                Category = ActionCategories.BuildingDiversityIntoYourOrganisation,
                Tags = [ActionTag.GenderPayGap],
                Name = "Appoint diversity leads or taskforces",
                Summary = "Appointing equality leads or committees to scrutinise hiring and progression policies ensures organisational accountability."
            }
        },
        {
            Actions.SupportStaffToTakeParentalLeave, new()
            {
                Action = Actions.SupportStaffToTakeParentalLeave,
                Category = ActionCategories.BuildingDiversityIntoYourOrganisation,
                Tags = [ActionTag.GenderPayGap],
                Name = "Support staff to take parental leave",
                Summary = "Promoting clear and accessible parental leave policies boosts gender equality and helps employees balance family life with work."
            }
        },
        {
            Actions.AnalyseDataToUnderstandPayGapAndStaffNeeds, new()
            {
                Action = Actions.AnalyseDataToUnderstandPayGapAndStaffNeeds,
                Category = ActionCategories.AnalysingYourData,
                Tags = [ActionTag.GenderPayGap],
                Name = "Analyse your data to understand your pay gap and staff needs",
                Summary = "Analysing pay data can help you understand your gender pay gap's scale and drivers, and choose effective actions to close the gap."
            }
        },
        {
            Actions.MeasureOutcomesOfEqualityInitiatives, new()
            {
                Action = Actions.MeasureOutcomesOfEqualityInitiatives,
                Category = ActionCategories.AnalysingYourData,
                Tags = [ActionTag.GenderPayGap],
                Name = "Measure outcomes of equality initiatives",
                Summary = "Tracking equality initiative outcomes can measure changes and see if you're making progress towards your goals."
            }
        },
        {
            Actions.TrainLineManagersToSupportEmployeesExperiencingMenopause, new()
            {
                Action = Actions.TrainLineManagersToSupportEmployeesExperiencingMenopause,
                Category = ActionCategories.SupportingStaffDuringMenopause,
                Tags = [ActionTag.Menopause],
                Name = "Train line managers to support employees experiencing menopause",
                Summary = "Line manager training on the menopause and its effects can help organisations to better support employees experiencing the menopause."
            }
        },
        {
            Actions.OfferOccupationalHealthAdviceForMenopause, new()
            {
                Action = Actions.OfferOccupationalHealthAdviceForMenopause,
                Category = ActionCategories.SupportingStaffDuringMenopause,
                Tags = [ActionTag.Menopause],
                Name = "Offer occupational health advice to employees experiencing menopause",
                Summary = "Supporting employees with specialised occupational health advice can help them manage symptoms, access support, and work more comfortably."
            }
        },
        {
            Actions.SetUpMenopauseNetworksAndSupportGroups, new()
            {
                Action = Actions.SetUpMenopauseNetworksAndSupportGroups,
                Category = ActionCategories.SupportingStaffDuringMenopause,
                Tags = [ActionTag.Menopause],
                Name = "Set up menopause networks and support groups",
                Summary = "Accessible menopause support groups within your organisation can help provide peer support, information, and guidance."
            }
        },
        {
            Actions.OfferWorkplaceAdjustmentsForMenopause, new()
            {
                Action = Actions.OfferWorkplaceAdjustmentsForMenopause,
                Category = ActionCategories.SupportingStaffDuringMenopause,
                Tags = [ActionTag.Menopause],
                Name = "Offer workplace adjustments to employees experiencing menopause",
                Summary = "Personalised workplace adjustments help support employees experiencing menopause and wider women's health conditions."
            }
        },
        {
            Actions.ConductMenopauseRiskAssessmentForWorkplace, new()
            {
                Action = Actions.ConductMenopauseRiskAssessmentForWorkplace,
                Category = ActionCategories.SupportingStaffDuringMenopause,
                Tags = [ActionTag.Menopause],
                Name = "Conduct a menopause risk assessment for your workplace",
                Summary = "Workplace risk assessments on menopause and women's health can identify necessary adaptations to support employee wellbeing."
            }
        }
    };
    
    public static readonly List<ActionDetails> ListOfAllActions = DictionaryOfAllActions.Values.ToList();
    
    public static readonly List<ActionDetails> ListOfGenderPayGapActions = DictionaryOfAllActions.Values.Where(a => a.Tags.Contains(ActionTag.GenderPayGap)).ToList();
    public static readonly List<ActionDetails> ListOfMenopauseActions = DictionaryOfAllActions.Values.Where(a => a.Tags.Contains(ActionTag.Menopause)).ToList();
}
