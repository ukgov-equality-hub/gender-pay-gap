using GenderPayGap.Core;
using GenderPayGap.Core.Helpers;
using GenderPayGap.Database;
using GenderPayGap.Database.Models;
using GenderPayGap.Extensions;
using GenderPayGap.WebUI.Helpers;
using GovUkDesignSystemDotNet;

namespace GenderPayGap.WebUI.Models.ManageOrganisations
{
    public class ManageOrganisationViewModel
    {

        public Organisation Organisation { get; }
        public User User { get; }

        private readonly List<DraftReturn> allDraftReturns;

        public ManageOrganisationViewModel(Organisation organisation, User user, List<DraftReturn> allDraftReturns)
        {
            Organisation = organisation;
            User = user;
            this.allDraftReturns = allDraftReturns;
        }

        public List<User> GetFullyRegisteredUsersForOrganisationWithCurrentUserFirst()
        {
            List<User> users = Organisation.UserOrganisations
                .Where(uo => uo.PINConfirmedDate.HasValue)
                .Select(uo => uo.User)
                .OrderBy(user => user.Fullname)
                .ToList();

            // The current user must be in this list (otherwise we wouldn't be able to visit this page)
            // So, remove the user from wherever they are n the list
            // And insert them at the start of the list
            users.Remove(User);
            users.Insert(0, User);

            return users;
        }

        public bool HasDraftGpgReport(int reportingYear)
        {
            return allDraftReturns.Any(d => d.SnapshotYear == reportingYear);
        }

        public bool OrganisationIsRequiredToSubmit(int reportingYear)
        {
            return OrganisationIsIsScope(reportingYear)
                   && !Global.ReportingStartYearsToExcludeFromLateFlagEnforcement.Contains(reportingYear);
        }

        public bool OrganisationIsIsScope(int reportingYear)
        {
            OrganisationScope scopeForYear = Organisation.GetScopeForYear(reportingYear);
            if (scopeForYear == null)
            {
                return true;
            }
            
            return scopeForYear.IsInScopeVariant();
        }

        public bool ShowScopeDeclarationStatus(int reportingYear)
        {
            return RequiredToDeclareScope(reportingYear) || IsScopeDeclared(reportingYear);
        }

        public TagViewModel GetScopeDeclarationStatusTag(int reportingYear)
        {
            if (IsScopeDeclared(reportingYear))
            {
                return new TagViewModel
                {
                    HtmlOrText = new("Completed"),
                    Classes = ["govuk-tag--green"]
                };
            }
            else if (DeadlineHasPassed(reportingYear))
            {
                return new TagViewModel
                {
                    HtmlOrText = new("Overdue"),
                    Classes = ["govuk-tag--red"]
                };
            }
            else
            {
                return new TagViewModel
                {
                    HtmlOrText = new("Not started"),
                    Classes = ["govuk-tag--blue"]
                };
            }
        }
        
        public bool ShowScopeStatus(int reportingYear)
        {
            return IsScopeDeclared(reportingYear) || !RequiredToDeclareScope(reportingYear) || IsInScope(reportingYear);
        }

        public TagViewModel GetScopeStatusTag(int reportingYear)
        {
            string scopeStatusText = Organisation.GetScopeStatusForYear(reportingYear).IsInScopeVariant()
                ? "Required to report"
                : "Not required to report";
            
            return new TagViewModel
            {
                HtmlOrText = new(scopeStatusText),
                Classes = ["govuk-tag--grey"],
                Attributes = new() {{"style", "max-width: unset;"}},
            };
        }

        private bool IsScopeDeclared(int reportingYear) => Organisation.GetScopeStatusForYear(reportingYear).IsScopeDeclared();

        private bool IsInScope(int reportingYear) => Organisation.GetScopeStatusForYear(reportingYear).IsInScopeVariant();

        private static bool RequiredToDeclareScope(int reportingYear) => reportingYear >= Global.FirstReportingYearForMandatoryScopeDeclaration;

        private bool DeadlineHasPassed(int reportingYear) => ReportingYearsHelper.DeadlineHasPassedForYearAndSector(reportingYear, Organisation.SectorType);

        public bool ShowCovidMessage(int reportingYear)
        {
            bool inScopeForYear = OrganisationIsIsScope(reportingYear);
            ReportStatusTag reportStatusTag = ReportStatusTagHelper.GetReportStatusTag(Organisation, reportingYear);

            return inScopeForYear
                   && ReportingYearsHelper.IsReportingYearExcludedFromLateFlagEnforcement(reportingYear)
                   && (reportStatusTag == ReportStatusTag.NotRequiredDueToCovid ||
                       reportStatusTag == ReportStatusTag.Overdue ||
                       reportStatusTag == ReportStatusTag.SubmittedLate);
        }
        
        public bool ShowActionPlanRow(int reportingYear)
        {
            return reportingYear >= Global.FirstReportingYearForActionPlans;
        }

        public TagViewModel ActionPlanStatusTag(int reportingYear)
        {
            if (!AbleToCreateActionPlan(reportingYear))
            {
                return new TagViewModel
                {
                    HtmlOrText = new("Cannot start yet"),
                    Classes = ["govuk-tag--grey"],
                    Attributes = new() {{"style", "max-width: unset;"}},
                };
            }
            else if (Organisation.GetLatestSubmittedActionPlan(reportingYear) != null)
            {
                string statusText = "Submitted";
                if (Organisation.GetLatestSubmittedOrDraftActionPlan(reportingYear).Status == ActionPlanStatus.Draft)
                {
                    statusText += ", draft amendments";
                }

                return new TagViewModel
                {
                    HtmlOrText = new(statusText),
                    Classes = ["govuk-tag--green"],
                    Attributes = new() {{"style", "max-width: unset;"}},
                };
            }
            else if (Organisation.GetLatestSubmittedOrDraftActionPlan(reportingYear) != null)
            {
                return new TagViewModel
                {
                    HtmlOrText = new("Draft created, not submitted"),
                    Classes = ["govuk-tag--blue"],
                    Attributes = new() {{"style", "max-width: unset;"}},
                };
            }
            else
            {
                return new TagViewModel
                {
                    HtmlOrText = new("Not started"),
                    Classes = ["govuk-tag--blue"],
                    Attributes = new() {{"style", "max-width: unset;"}},
                };
            }
        }

        public bool IsActionPlanOptional(int reportingYear)
        {
            return reportingYear >= Global.FirstReportingYearForActionPlans
                   && reportingYear < Global.FirstReportingYearForMandatoryActionPlans;
        }

        public bool AbleToReportGpg(int reportingYear)
        {
            return IsScopeDeclared(reportingYear) || !RequiredToDeclareScope(reportingYear);
        }

        public bool AbleToCreateActionPlan(int reportingYear)
        {
            return IsScopeDeclared(reportingYear) || !RequiredToDeclareScope(reportingYear);
        }

        private bool AllReportingCompleteForYear(int reportingYear)
        {
            if (RequiredToDeclareScope(reportingYear) && !IsScopeDeclared(reportingYear))
            {
                return false;
            }

            if (!OrganisationIsRequiredToSubmit(reportingYear))
            {
                return true;
            }

            return !GpgReportingIncomplete(reportingYear) && !ActionPlanReportingIncomplete(reportingYear);
        }

        private bool GpgReportingIncomplete(int reportingYear)
        {
            return !Organisation.HasSubmittedReturn(reportingYear);
        }

        private bool ActionPlanReportingIncomplete(int reportingYear)
        {
            if (!ShowActionPlanRow(reportingYear))
            {
                return false;
            }

            if (IsActionPlanOptional(reportingYear))
            {
                return false;
            }

            return Organisation.GetLatestSubmittedActionPlan(reportingYear) == null;
        }

        public string GetDeadlineDateFormatted(int reportingYear)
        {
            DateTime deadlineDate = ReportingYearsHelper.GetDeadline(Organisation.SectorType, reportingYear);
            return deadlineDate.ToString("d MMMM yyyy");
        }

        public string GetDeadlineRelativeDaysIfNeeded(int reportingYear)
        {
            DateTime deadlineDate = ReportingYearsHelper.GetDeadline(Organisation.SectorType, reportingYear);
            DateTime today = VirtualDateTime.Now.Date;
            
            int currentReportingYear = ReportingYearsHelper.GetCurrentReportingYear(Organisation.SectorType);
            bool isCurrentReportingYear = reportingYear == currentReportingYear;
            bool isPreviousReportingYear = reportingYear == currentReportingYear - 1;
            
            if (isCurrentReportingYear || isPreviousReportingYear)
            {
                if (deadlineDate < today)
                {
                    int daysPastDeadline = (int) ((today - deadlineDate).TotalDays);
                    return $"({daysPastDeadline} {(daysPastDeadline == 1 ? "day" : "days")} ago)";
                }
                else if (deadlineDate == today)
                {
                    return "(today)";
                }
                else
                {
                    int daysUntilDeadline = (int) ((deadlineDate - today).TotalDays);
                    return $"({daysUntilDeadline} {(daysUntilDeadline == 1 ? "day" : "days")} until the deadline)";
                }
            }

            return null;
        }

        public TagViewModel GetOverallStatusTagForYear(int reportingYear)
        {
            DateTime deadlineDate = ReportingYearsHelper.GetDeadline(Organisation.SectorType, reportingYear);
            DateTime today = VirtualDateTime.Now.Date;
            
            int currentReportingYear = ReportingYearsHelper.GetCurrentReportingYear(Organisation.SectorType);
            bool isCurrentReportingYear = reportingYear == currentReportingYear;
            
            if (AllReportingCompleteForYear(reportingYear))
            {
                return new TagViewModel
                {
                    HtmlOrText = new("Complete"),
                    Classes = ["govuk-tag--green"],
                    Attributes = new(){{"style", "max-width: unset;"}},
                };
            }
            else if (isCurrentReportingYear)
            {
                int daysUntilDeadline = (int) ((deadlineDate - today).TotalDays);

                return new TagViewModel
                {
                    HtmlOrText = new($"Due: You have {daysUntilDeadline} {(daysUntilDeadline == 1 ? "day" : "days")} until the deadline"),
                    Classes = ["govuk-tag--blue"],
                    Attributes = new(){{"style", "max-width: unset;"}},
                };
            }
            else
            {
                int daysOverdue = (int) ((today - deadlineDate).TotalDays);
                
                return new TagViewModel
                {
                    HtmlOrText = new($"Overdue: your reports are {daysOverdue} {(daysOverdue == 1 ? "day" : "days")} overdue"),
                    Classes = ["govuk-tag--red"],
                    Attributes = new(){{"style", "max-width: unset;"}},
                };
            }
        }

    }
}
