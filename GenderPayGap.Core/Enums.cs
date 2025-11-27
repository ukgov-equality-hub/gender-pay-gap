using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace GenderPayGap.Core
{
    public enum UserStatuses : byte
    {

        Unknown = 0,
        New = 1,
        Active = 3,
        Retired = 4

    }

    public enum UserRole
    {
        Employer = 0,
        Admin = 1,
        AdminReadOnly = 2
    }
    
    public enum OrganisationStatuses : byte
    {

        Unknown = 0,
        New = 1,
        Active = 3,
        Retired = 4,
        Pending = 5,
        Deleted = 6

    }

    public enum AddressStatuses : byte
    {

        Unknown = 0,
        New = 1,
        Suspended = 2,
        Active = 3,
        Pending = 5,
        Retired = 6

    }

    public enum RegistrationMethods
    {

        Unknown = 0,
        PinInPost = 1,
        EmailDomain = 2,
        Manual = 3,
        Fasttrack = 4

    }

    public enum ReturnStatuses : byte
    {

        Unknown = 0,
        Draft = 1,
        Suspended = 2,
        Submitted = 3,
        Retired = 4,
        Deleted = 5

    }

    public enum ActionPlanStatus : byte
    {
        Draft = 1,
        DeletedDraft = 2,
        Submitted = 3,
        Retired = 4,
        Deleted = 5
    }

    public enum ActionPlanType : byte
    {
        Original = 1,
        OneYearReview = 2,
        ThreeYearReview = 3
    }

    public enum Actions
    {
        // 1. Make job adverts inclusive
        MakeJobAdvertsInclusive = 1,
        // 2. Encourage applications from diverse candidates
        EncourageApplicationsFromDiverseCandidates = 2,
        // 3. Reduce unconscious bias in CV screening
        ReduceUnconsciousBiasInCVScreening = 3,
        // 4. Run structured interviews
        RunStructuredInterviews = 4,
        // 5. Ensure interview outcomes are fair
        EnsureInterviewOutcomesAreFair = 5,
        // 6. Promote flexible working arrangements in job adverts
        PromoteFlexibleWorkingArrangementsInJobAdverts = 6,
        // 7. Increase transparency for pay, promotion and rewards
        IncreaseTransparencyForPayPromotionAndRewards = 7,
        // 8. Automatically put forward employees for promotion
        AutomaticallyPutForwardEmployeesForPromotion = 8,
        // 9. Encourage employee development through actionable steps
        EncourageEmployeeDevelopmentThroughActionableSteps = 9,
        // 10. Offer internships, mentoring and other development programmes
        OfferInternshipsMentoringAndOtherDevelopmentProgrammes = 10,
        // 11. Set targets to improve gender representation
        SetTargetsToImproveGenderRepresentation = 11,
        // 12. Appoint diversity leads or taskforces
        AppointDiversityLeadsOrTaskforces = 12,
        // 13. Support staff to take parental leave
        SupportStaffToTakeParentalLeave = 13,
        // 14. Analyse your data to understand your pay gap and staff needs
        AnalyseDataToUnderstandPayGapAndStaffNeeds = 14,
        // 15. Measure outcomes of equality initiatives
        MeasureOutcomesOfEqualityInitiatives = 15,
        // 16. Train line managers to support employees experiencing menopause
        TrainLineManagersToSupportEmployeesExperiencingMenopause = 16,
        // 17. Offer occupational health advice to employees experiencing menopause
        OfferOccupationalHealthAdviceForMenopause = 17,
        // 18. Set up menopause networks and support groups
        SetUpMenopauseNetworksAndSupportGroups = 18,
        // 19. Offer workplace adjustments to employees experiencing menopause
        OfferWorkplaceAdjustmentsForMenopause = 19,
        // 20. Conduct a menopause risk assessment for your workplace
        ConductMenopauseRiskAssessmentForWorkplace = 20
    }

    public enum ActionStatus : byte
    {
        [Display(Name = "Do not add to plan")]
        DoNotAddToPlan = 0,
        [Display(Name = "In progress")]
        NewOrInProgress = 1,
        [Display(Name = "Completed")]
        Completed = 2,
    }
    public static class ActionStatusExtensions
    {
        public static string GetDisplayName(this ActionStatus actionStatus)
        {
            DisplayAttribute displayAttribute = actionStatus.GetType()
                .GetMember(actionStatus.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DisplayAttribute>();
                
            return displayAttribute?.Name;
        }
        public static string GetTagColour(this ActionStatus actionStatus)
        {
            return actionStatus switch
            {
                ActionStatus.NewOrInProgress => "govuk-tag--green",
                ActionStatus.Completed => "govuk-tag--yellow",
                ActionStatus.DoNotAddToPlan => "govuk-tag--grey",
                _ => "govuk-tag--grey"
            };
        }
    }


    public enum ScopeRowStatuses : byte
    {

        Unknown = 0,
        Active = 3,
        Retired = 4

    }

    public enum SectorTypes
    {

        Unknown = 0,
        Private = 1,
        Public = 2

    }

    public enum ScopeStatuses
    {

        [Display(Name = "In scope")]
        Unknown = 0,

        [Display(Name = "In scope")]
        InScope = 1,

        [Display(Name = "Out of scope")]
        OutOfScope = 2,

        [Display(Name = "In scope")]
        PresumedInScope = 3,

        [Display(Name = "Out of scope")]
        PresumedOutOfScope = 4

    }

    public static class ScopeStatusesExtensions
    {
        public static bool IsInScopeVariant(this ScopeStatuses scopeStatus)
        {
            return scopeStatus == ScopeStatuses.InScope || scopeStatus == ScopeStatuses.PresumedInScope;
        }
        public static bool IsOutOfScopeVariant(this ScopeStatuses scopeStatus)
        {
            return scopeStatus == ScopeStatuses.OutOfScope || scopeStatus == ScopeStatuses.PresumedOutOfScope;
        }
        
        public static bool IsScopePresumed(this ScopeStatuses scopeStatus)
        {
            return scopeStatus == ScopeStatuses.PresumedInScope || scopeStatus == ScopeStatuses.PresumedOutOfScope;
        }
    }
    
    public enum OrganisationSizes
    {

        [Display(Name = "Not Provided")]
        [Range(0, 0)]
        NotProvided = 0,

        [Display(Name = "Less than 250")]
        [Range(0, 249)]
        Employees0To249 = 1,

        [Display(Name = "250 to 499")]
        [Range(250, 499)]
        Employees250To499 = 2,

        [Display(Name = "500 to 999")]
        [Range(500, 999)]
        Employees500To999 = 3,

        [Display(Name = "1000 to 4999")]
        [Range(1000, 4999)]
        Employees1000To4999 = 4,

        [Display(Name = "5000 to 19,999")]
        [Range(5000, 19999)]
        Employees5000To19999 = 5,

        [Display(Name = "20,000 or more")]
        [Range(20000, int.MaxValue)]
        Employees20000OrMore = 6

    }

    public static class OrganisationSizesExtensions
    {
        public static string GetDisplayName(this OrganisationSizes organisationSize)
        {
            DisplayAttribute displayAttribute = organisationSize.GetType()
                .GetMember(organisationSize.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DisplayAttribute>();
                
            return displayAttribute?.Name;
        }
        
        public static RangeAttribute GetRange(this OrganisationSizes organisationSize)
        {
            return organisationSize.GetType()
                .GetMember(organisationSize.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<RangeAttribute>();
        }
    }

    public enum AuditedAction
    {
        [Display(Name = "Admin changed late flag")]
        AdminChangeLateFlag = 0,
        [Display(Name = "Admin changed organisation scope")]
        AdminChangeOrganisationScope = 1,
        [Display(Name = "Admin changed companies house opting")]
        AdminChangeCompaniesHouseOpting = 2,
        [Display(Name = "Admin changed organisation name")]
        AdminChangeOrganisationName = 3,
        [Display(Name = "Admin changed organisation address")]
        AdminChangeOrganisationAddress = 4,
        [Display(Name = "Admin changed organisation SIC code")]
        AdminChangeOrganisationSicCode = 5,
        [Display(Name = "Admin changed user contact preferences")]
        AdminChangeUserContactPreferences = 6,
        [Display(Name = "Admin re-sent verification email")]
        AdminResendVerificationEmail = 7,
        [Display(Name = "Admin changed organisation public sector classification")]
        AdminChangeOrganisationPublicSectorClassification = 8,
        [Display(Name = "User changed their email address")]
        UserChangeEmailAddress = 9,
        [Display(Name = "User changed their password")]
        UserChangePassword = 10,
        [Display(Name = "User changed their name")]
        UserChangeName = 11,
        [Display(Name = "User changed their job title")]
        UserChangeJobTitle = 12,
        [Display(Name = "User changed their phone number")]
        UserChangePhoneNumber = 13,
        [Display(Name = "User changed their contact preferences")]
        UserChangeContactPreferences = 14,
        [Display(Name = "User retired their account")]
        UserRetiredThemselves = 15,
        [Display(Name = "Admin removed a user from an organisation")]
        AdminRemoveUserFromOrganisation = 16,
        [Display(Name = "Admin changed organisation status")]
        AdminChangeOrganisationStatus = 17,
        [Display(Name = "Admin deleted return")]
        AdminDeleteReturn = 18,

        // Note:
        //   Lots of these Execute Manual Change enum values are no longer referenced in code
        //   However, we might have Audit Log entries in the database that reference these values
        //   So, we want to keep these values, so that they show up correctly in the Audit Log
        //   i.e. it's not possible to perform these actions NOW
        //        but that doesn't stop us having a record of them being performed in the past
        [Display(Name = "Execute Manual Change: Convert public to private")]
        ExecuteManualChangeConvertPublicToPrivate = 19,
        [Display(Name = "Execute Manual Change: Convert private to public")]
        ExecuteManualChangeConvertPrivateToPublic = 20,
        [Display(Name = "Execute Manual Change: Convert sector: set accounting date")]
        ExecuteManualChangeConvertSectorSetAccountingDate = 21,
        [Display(Name = "Execute Manual Change: Delete submissions")]
        /* See note above */
        ExecuteManualChangeDeleteSubmissions = 22,
        [Display(Name = "Execute Manual Change: Add organisations latest name")]
        /* See note above */
        ExecuteManualChangeAddOrganisationsLatestName = 23,
        [Display(Name = "Execute Manual Change: Reset organisation to only original name")]
        /* See note above */
        ExecuteManualChangeResetOrganisationToOnlyOriginalName = 24,
        [Display(Name = "Execute Manual Change: Set organisation company number")]
        ExecuteManualChangeSetOrganisationCompanyNumber = 25,
        [Display(Name = "Execute Manual Change: Set organisation SIC codes")]
        ExecuteManualChangeSetOrganisationSicCodes = 26,
        [Display(Name = "Execute Manual Change: Set organisation addresses")]
        /* See note above */
        ExecuteManualChangeSetOrganisationAddresses = 27,
        [Display(Name = "Execute Manual Change: Set public sector type")]
        /* See note above */
        ExecuteManualChangeSetPublicSectorType = 28,
        [Display(Name = "Execute Manual Change: Set organisation scope")]
        /* See note above */
        ExecuteManualChangeSetOrganisationScope = 29,
        [Display(Name = "Execute Manual Change: Create security code")]
        /* See note above */
        ExecuteManualChangeCreateOrExtendSecurityCode = 30,
        [Display(Name = "Execute Manual Change: Extend security code")]
        /* See note above */
        ExecuteManualChangeExpireSecurityCode = 31,
        [Display(Name = "Execute Manual Change: Create security codes for all active and pending orgs")]
        /* See note above */
        ExecuteManualChangeCreateOrExtendSecurityCodesForAllActiveAndPendingOrgs = 32,
        [Display(Name = "Execute Manual Change: Expire security codes for all active and pending orgs")]
        /* See note above */
        ExecuteManualChangeExpireSecurityCodesForAllActiveAndPendingOrgs = 33,

        [Display(Name = "Purge organisation")]
        PurgeOrganisation = 34,
        [Display(Name = "Purge registration")]
        PurgeRegistration = 35,
        [Display(Name = "Purge user")]
        PurgeUser = 36,
        [Display(Name = "Registraion log")]
        RegistrationLog = 37,

        [Display(Name = "Admin deleted organisation previous name")]
        AdminDeleteOrganisationPreviousName = 38,
        [Display(Name = "Admin updated SIC sections")]
        AdminUpdatedSicSections = 39,
        [Display(Name = "Admin updated SIC codes")]
        AdminUpdatedSicCodes = 40,

        [Display(Name = "Admin changed organisation company number")]
        AdminChangeOrganisationCompanyNumber = 41,
        [Display(Name = "Admin changed organisation sector")]
        AdminChangedOrganisationSector = 42,

        [Display(Name = "User automatically reactivated a deleted or retired organisation")]
        UserReactivatedOrganisationAutomatically = 43,

        [Display(Name = "Admin changed user status")]
        AdminChangeUserStatus = 44,
        [Display(Name = "Admin add admin user")]
        AdminAddAdminUser = 45,
        
        [Display(Name = "Admin deleted action plan")]
        AdminDeleteActionPlan = 46,
    }

    public enum HashingAlgorithm
    {
        Unknown = 0,
        SHA512 = 1,
        PBKDF2 = 2,
        PBKDF2AppliedToSHA512 = 3
    }

    public enum FeedbackStatus
    {
        New = 0,
        NotSpam = 1,
        Spam = 2
    }

    public enum ReminderEmailStatus : byte
    {
        InProgress = 0,
        Completed = 1
    }

    public static class EnumHelper
    {

        public static string DisplayNameOf(object obj)
        {
            return obj.GetType()
                ?
                .GetMember(obj.ToString())
                ?.First()
                ?
                .GetCustomAttribute<DisplayAttribute>()
                ?.Name;
        }

    }

    public static class CookieNames
    {
        public const string LastCompareQuery = "compare";
    }

    public enum ReportStatusTag
    {
        SubmittedVoluntarily,
        SubmittedLate,
        Submitted,
        NotRequiredDueToCovid,
        NotRequired,
        Overdue,
        Due,
    }

}
