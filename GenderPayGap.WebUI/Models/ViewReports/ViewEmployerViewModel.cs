using GenderPayGap.Core;
using GenderPayGap.Core.Helpers;
using GenderPayGap.Database;
using GenderPayGap.WebUI.Helpers;
using GovUkDesignSystemDotNet;

namespace GenderPayGap.WebUI.Models.ViewReports;

public class ViewEmployerViewModel
{
    
    public Organisation Organisation { get; set; }

    public bool ShowOrganisationRetiredMessage()
    {
        return Organisation.GetSubmittedReports().Any() && Organisation.Status == OrganisationStatuses.Retired;
    }

    public bool ShowActionPlanRow(int reportingYear)
    {
        return reportingYear >= Global.FirstReportingYearForActionPlans;
    }

    public string GetActionPlanSubmissionType(int reportingYear)
    {
        if (IsActionPlanOptionalForThisReportingYear(reportingYear))
        {
            return "Action plan (optional)";
        }
        else
        {
            return "Action plan";
        }
        // TODO: This is where we will add "Action plan (1-year review)" or "Action plan (3-year review)"
    }

    public TagViewModel GetActionPlanStatusTag(int reportingYear)
    {
        if (Organisation.GetLatestSubmittedActionPlan(reportingYear) != null)
        {
            string tagText = "Submitted";

            if (!OrganisationIsIsScope(reportingYear) ||
                IsActionPlanOptionalForThisReportingYear(reportingYear))
            {
                tagText += " voluntarily";
            }
            // TODO: This is where we will add an "else" to add "submitted late" for the mandatory reporting years
            // We should use a SubmittedLate flag, similar to Returns
            
            return new TagViewModel
            {
                HtmlOrText = new(tagText),
                Classes = ["govuk-tag--green"],
                Attributes = new() {{"style", "max-width: unset;"}},
            };
        }
        else if (!OrganisationIsIsScope(reportingYear))
        {
            return new TagViewModel
            {
                HtmlOrText = new("Not required"),
                Classes = ["govuk-tag--grey"],
                Attributes = new() {{"style", "max-width: unset;"}},
            };
        }
        else if (IsActionPlanOptionalForThisReportingYear(reportingYear))
        {
            return new TagViewModel
            {
                HtmlOrText = new($"Optional for {ReportingYearsHelper.FormatYearAsReportingPeriod(reportingYear)}"),
                Classes = ["govuk-tag--grey"],
                Attributes = new() {{"style", "max-width: unset;"}},
            };
        }
        else
        {
            DateTime deadlineDate = ReportingYearsHelper.GetDeadline(Organisation.SectorType, reportingYear);

            if (ReportingYearsHelper.DeadlineHasPassedForYearAndSector(reportingYear, Organisation.SectorType))
            {
                return new TagViewModel
                {
                    HtmlOrText = new($"Overdue on {deadlineDate.ToString("d MMMM yyyy")}"),
                    Classes = ["govuk-tag--orange"],
                    Attributes = new() {{"style", "max-width: unset;"}},
                };
            }
            else
            {
                return new TagViewModel
                {
                    HtmlOrText = new($"Due by {deadlineDate.ToString("d MMMM yyyy")}"),
                    Classes = ["govuk-tag--blue"],
                    Attributes = new() {{"style", "max-width: unset;"}},
                };
            }
        }
    }

    public bool ActionPlanAvailableToView(int reportingYear)
    {
        return Organisation.GetLatestSubmittedActionPlan(reportingYear) != null;
    }

    private bool IsActionPlanOptionalForThisReportingYear(int reportingYear)
    {
        return reportingYear >= Global.FirstReportingYearForActionPlans
               && reportingYear < Global.FirstReportingYearForMandatoryActionPlans;
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

    public bool ShowCovidMessage(int reportingYear)
    {
        ReportStatusTag reportStatusTag = ReportStatusTagHelper.GetReportStatusTag(Organisation, reportingYear);

        return reportStatusTag == ReportStatusTag.NotRequiredDueToCovid
               || (reportStatusTag == ReportStatusTag.Overdue
                   && ReportingYearsHelper.IsReportingYearExcludedFromLateFlagEnforcement(reportingYear));
    }

}
