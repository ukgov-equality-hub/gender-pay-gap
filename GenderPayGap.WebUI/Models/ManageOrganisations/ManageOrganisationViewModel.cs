using GenderPayGap.Core;
using GenderPayGap.Core.Helpers;
using GenderPayGap.Database;
using GenderPayGap.Database.Models;
using GenderPayGap.Extensions;
using GenderPayGap.WebUI.Helpers;
using GovUkDesignSystemDotNet;

namespace GenderPayGap.WebUI.Models.ManageOrganisations;

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

    public int GetFirstReportingYearToShow()
    {
        int currentReportingYear = ReportingYearsHelper.GetCurrentReportingYear(Organisation.SectorType);
        int minimumNumberOfReportingYearsToShow = 2;
        int minimumFirstReportingYearToShow = currentReportingYear - minimumNumberOfReportingYearsToShow + 1;
            
        foreach (int reportingYear in ReportingYearsHelper.GetReportingYears(Organisation.SectorType))
        {
            if (reportingYear < minimumFirstReportingYearToShow)
            {
                if (!AllReportingCompleteForYear(reportingYear))
                {
                    return reportingYear;
                }
            }
        }
            
        return minimumFirstReportingYearToShow;
    }

    #region Deadline
    public bool ShowDeadline(int reportingYear)
    {
        int firstReportingYearToShowDeadline = GetFirstReportingYearToShow();
        bool showDeadline = reportingYear >= firstReportingYearToShowDeadline;
        return showDeadline;
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
    #endregion

    #region Reporting status
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

    public string GetScopeStatusText(int reportingYear)
    {
        return Organisation.GetScopeStatusForYear(reportingYear).IsInScopeVariant()
            ? "Required to report"
            : "Not required to report";
    }
    #endregion

    #region Overall progress
    private bool AllReportingCompleteForYear(int reportingYear)
    {
        if (!OrganisationIsRequiredToSubmit(reportingYear))
        {
            return true;
        }

        return !GpgReportingIncomplete(reportingYear) && !ActionPlanReportingIncomplete(reportingYear);
    }
    #endregion

    #region Gender Pay Gap reports
    public bool HasDraftGpgReport(int reportingYear)
    {
        return allDraftReturns.Any(d => d.SnapshotYear == reportingYear);
    }

    public string GenderPayGapLinkText(int reportingYear)
    {
        if (Organisation.HasSubmittedReturn(reportingYear))
        {
            if (HasDraftGpgReport(reportingYear))
            {
                return "Continue editing";
            }
            else
            {
                return "Edit";
            }
        }
        else if (HasDraftGpgReport(reportingYear))
        {
            return "Continue creating";
        }
        else
        {
            return "Create your gender pay gap report";
        }
    }

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

    private bool GpgReportingIncomplete(int reportingYear)
    {
        return !Organisation.HasSubmittedReturn(reportingYear);
    }
    #endregion

    #region Action Plans
    public bool ShowActionPlanRow(int reportingYear)
    {
        return reportingYear >= Global.FirstReportingYearForActionPlans;
    }

    public TagViewModel ActionPlanStatusTag(int reportingYear)
    {
        if (Organisation.GetLatestSubmittedActionPlan(reportingYear) != null)
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
                HtmlOrText = new("Draft created"),
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

    public string ActionPlanLinkText(int reportingYear)
    {
        if (Organisation.GetLatestSubmittedActionPlan(reportingYear) != null)
        {
            if (Organisation.GetLatestSubmittedOrDraftActionPlan(reportingYear).Status == ActionPlanStatus.Draft)
            {
                return "Continue editing";
            }
            else
            {
                return "Edit";
            }
        }
        else if (Organisation.GetLatestSubmittedOrDraftActionPlan(reportingYear) != null)
        {
            return "Continue creating";
        }
        else
        {
            return "Create your action plan";
        }
    }

    public bool IsActionPlanOptional(int reportingYear)
    {
        return reportingYear >= Global.FirstReportingYearForActionPlans
               && reportingYear < Global.FirstReportingYearForMandatoryActionPlans;
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
    #endregion

    #region Users
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
    #endregion
    
}
