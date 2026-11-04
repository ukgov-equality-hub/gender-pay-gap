using GenderPayGap.Core;
using GenderPayGap.Core.Helpers;
using GenderPayGap.Core.Interfaces;
using GenderPayGap.Database;
using GenderPayGap.WebUI.Helpers;
using GenderPayGap.WebUI.Models.Scope;
using GenderPayGap.WebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenderPayGap.WebUI.Controllers;

[Authorize(Roles = LoginRoles.GpgEmployer)]
[Route("organisation")]
public class ScopeController : Controller
{

    private readonly EmailSendingService emailSendingService;
    private readonly IDataRepository dataRepository;

    public ScopeController(EmailSendingService emailSendingService, IDataRepository dataRepository)
    {
        this.emailSendingService = emailSendingService;
        this.dataRepository = dataRepository;
    }

    [HttpGet("{encryptedOrganisationId}/reporting-year/{reportingYear}/declare-scope")]
    public IActionResult DeclareScopeForYearGet(string encryptedOrganisationId, int reportingYear)
    {
        long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
        ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
        ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
        ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);
        
        Organisation organisation = dataRepository.Get<Organisation>(organisationId);
        OrganisationScope organisationScope = organisation.GetScopeForYear(reportingYear);

        ScopeStatuses? displayedScope = null;
        if (organisationScope.IsScopeDeclared())
        {
            displayedScope = organisationScope.ScopeStatus;
        }
        else if (organisationScope.IsInScopeVariant())
        {
            displayedScope = ScopeStatuses.InScope;
        }
            
        var viewModel = new DeclareScopeForYearViewModel
        {
            Organisation = organisation,
            ReportingYear = reportingYear,
            Scope = displayedScope,
            WhyOutOfScope =
                organisationScope.Reason != null
                    ? (organisationScope.Reason == "Under250" ? DeclareScopeForYearWhyOutOfScope.Under250 : DeclareScopeForYearWhyOutOfScope.Other)
                    : null,
            WhyOutOfScopeDetails =
                organisationScope.Reason != "Under250"
                    ? organisationScope.Reason
                    : null,
            ReadGuidance =
                organisationScope.ReadGuidance.HasValue
                    ? (organisationScope.ReadGuidance == true ? ReadGuidanceYesNo.Yes : ReadGuidanceYesNo.No)
                    : null
        };
            
        return View("DeclareScopeForYear", viewModel);
    }

    [HttpPost("{encryptedOrganisationId}/reporting-year/{reportingYear}/declare-scope")]
    [ValidateAntiForgeryToken]
    public IActionResult DeclareScopeForYearPost(string encryptedOrganisationId, int reportingYear, DeclareScopeForYearViewModel viewModel)
    {
        long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
        ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
        ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
        ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);
        
        Organisation organisation = dataRepository.Get<Organisation>(organisationId);
        OrganisationScope currentOrganisationScope = organisation.GetScopeForYear(reportingYear);

        viewModel.Organisation = organisation;
        viewModel.ReportingYear = reportingYear;

        if (viewModel.Scope.HasValue && !viewModel.Scope.Value.IsScopeDeclared())
        {
            ModelState.AddModelError<DeclareScopeForYearViewModel>(m => m.Scope, "Select whether or not this employer is required to report.");
        }
        if (!ModelState.IsValid)
        {
            return View("DeclareScopeForYear", viewModel);
        }

        ScopeStatuses newScope = viewModel.Scope.Value;
        string newReason =
            (viewModel.Scope == ScopeStatuses.OutOfScope && viewModel.WhyOutOfScope.HasValue)
                ? (viewModel.WhyOutOfScope.Value == DeclareScopeForYearWhyOutOfScope.Under250 ? "Under250" : viewModel.WhyOutOfScopeDetails)
                : null;
        bool? newHaveReadGuidance =
            (viewModel.Scope == ScopeStatuses.OutOfScope && viewModel.ReadGuidance.HasValue)
                ? (viewModel.ReadGuidance.Value == ReadGuidanceYesNo.Yes)
                : null;

        if (currentOrganisationScope.ScopeStatus != newScope ||
            currentOrganisationScope.Reason != newReason ||
            currentOrganisationScope.ReadGuidance != newHaveReadGuidance)
        {
            organisation.SetScopeForYear(
                reportingYear,
                newScope,
                "Generated by the system",
                newReason,
                newHaveReadGuidance
            );
            dataRepository.SaveChanges();

            SendScopeChangeEmails(organisation, reportingYear, newScope);
        }

        return View("DeclaredScopeForYear", viewModel);
    }

    [HttpGet("{encryptedOrganisationId}/reporting-year/{reportingYear}/declare-sector")]
    public IActionResult DeclareSectorForYearGet(string encryptedOrganisationId, int reportingYear)
    {
        long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
        ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
        ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
        ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);
        
        Organisation organisation = dataRepository.Get<Organisation>(organisationId);
        OrganisationScope organisationScope = organisation.GetScopeForYear(reportingYear);

        var viewModel = new DeclareSectorForYearViewModel
        {
            Organisation = organisation,
            ReportingYear = reportingYear,
            Sector = organisationScope.Sector
        };
            
        return View("DeclareSectorForYear", viewModel);
    }

    [HttpPost("{encryptedOrganisationId}/reporting-year/{reportingYear}/declare-sector")]
    [ValidateAntiForgeryToken]
    public IActionResult DeclareSectorForYearPost(string encryptedOrganisationId, int reportingYear, DeclareSectorForYearViewModel viewModel)
    {
        long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
        ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
        ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
        ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);
        
        Organisation organisation = dataRepository.Get<Organisation>(organisationId);
        
        if (!ModelState.IsValid)
        {
            viewModel.Organisation = organisation;
            viewModel.ReportingYear = reportingYear;
            return View("DeclareSectorForYear", viewModel);
        }
        
        OrganisationScope currentOrganisationScope = organisation.GetScopeForYear(reportingYear);

        if (AnySectorChangesToSave(currentOrganisationScope, viewModel))
        {
            OrganisationScope newOrganisationScope = organisation.CreateNewScopeForYear(reportingYear);
        
            if (viewModel.SkippedDeclaringSector)
            {
                newOrganisationScope.SectorStatus = SectorStatuses.Skipped;
            }
            else
            {
                newOrganisationScope.Sector = viewModel.Sector;
                newOrganisationScope.SectorStatus = SectorStatuses.Declared;
            }
            
            dataRepository.SaveChanges();
        }

        return RedirectToAction("DeclareScopeForYearGet", "Scope", new {encryptedOrganisationId, reportingYear = reportingYear});
    }

    private static bool AnySectorChangesToSave(OrganisationScope organisationScope, DeclareSectorForYearViewModel viewModel)
    {
        switch (organisationScope.SectorStatus)
        {
            case SectorStatuses.Presumed:
                // The current Sector Status (as saved in the database) is Presumed.
                // The user has taken an action (either choosing a Sector, or skipping).
                // So there is a change to save.
                return true;

            case SectorStatuses.Skipped:
                if (!viewModel.SkippedDeclaringSector)
                {
                    // The current Sector Status (as saved in the database) is SkippedDeclaringSector.
                    // Now the user has chosen a Sector.
                    // So there is a change to save.
                    return true;
                }
                else
                {
                    // No change here
                    return false;
                }

            case SectorStatuses.Declared:
                if (viewModel.SkippedDeclaringSector)
                {
                    // The current Sector Status (as saved in the database) is Declared.
                    // Now the user has skipped declaring their Sector.
                    // So there is a change to save.
                    return true;
                }
                else
                {
                    // Check if the Sector has changed
                    return organisationScope.Sector != viewModel.Sector;
                }

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SendScopeChangeEmails(Organisation organisation, int reportingYear, ScopeStatuses newScope)
    {
        int currentReportingYear = ReportingYearsHelper.GetCurrentReportingYear(organisation.SectorType);
            
        // Send emails if scope changed on current or previous reporting year
        if (reportingYear == currentReportingYear || reportingYear == (currentReportingYear - 1))
        {
            // Find all email addresses associated with the organisation - only the active ones (who have confirmed their PIN)
            IEnumerable<string> emailAddressesForOrganisation = organisation.UserOrganisations
                .Where(uo => uo.PINConfirmedDate.HasValue)
                .Where(uo => !uo.User.HasBeenAnonymised)
                .Select(uo => uo.User.EmailAddress);
                
            // Send email of correct type to each email address associated with organisation
            foreach (string emailAddress in emailAddressesForOrganisation)
            {
                if (newScope == ScopeStatuses.InScope)
                {
                    // Use Notify to send in scope email
                    emailSendingService.SendScopeChangeInEmail(emailAddress, organisation.OrganisationName);
                }
                else
                {
                    // Use Notify to send out of scope email
                    emailSendingService.SendScopeChangeOutEmail(emailAddress, organisation.OrganisationName);
                }
                    
            }
        }
    }
}
