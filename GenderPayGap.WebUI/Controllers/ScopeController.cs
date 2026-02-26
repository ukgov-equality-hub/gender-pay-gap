using GenderPayGap.Core;
using GenderPayGap.Core.Helpers;
using GenderPayGap.Core.Interfaces;
using GenderPayGap.Database;
using GenderPayGap.WebUI.Helpers;
using GenderPayGap.WebUI.Models.ManageOrganisations;
using GenderPayGap.WebUI.Models.Scope;
using GenderPayGap.WebUI.Models.ScopeNew;
using GenderPayGap.WebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenderPayGap.WebUI.Controllers
{
    
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
        
        [HttpGet("{encryptedOrganisationId}/reporting-year/{reportingYear}/change-scope")]
        public IActionResult ChangeOrganisationScope(string encryptedOrganisationId, int reportingYear)
        {
            long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
            ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
            ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
            ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);

            // Get Organisation and OrganisationScope for reporting year
            Organisation organisation = dataRepository.Get<Organisation>(organisationId);
            OrganisationScope organisationScope = organisation.GetScopeForYear(reportingYear);
            
            var viewModel = new ScopeViewModel
            {
                Organisation = organisation,
                ReportingYear = ReportingYearsHelper.GetAccountingStartDate(organisation.SectorType, reportingYear),
                IsToSetInScope = !organisationScope.IsInScopeVariant()
            };
            
            return View(organisationScope.IsInScopeVariant() ? "OutOfScopeQuestions" : "ConfirmScope", viewModel);
        }
        
        [HttpPost("{encryptedOrganisationId}/reporting-year/{reportingYear}/change-scope/out")]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitOutOfScopeAnswers(string encryptedOrganisationId, int reportingYear, ScopeViewModel viewModel)
        {
            long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
            ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
            ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
            ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);

            // Get Organisation and OrganisationScope for reporting year
            Organisation organisation = dataRepository.Get<Organisation>(organisationId);
            OrganisationScope organisationScope = organisation.OrganisationScopes.FirstOrDefault(s => s.ReportingYear == reportingYear);

            viewModel.Organisation = organisation;
            viewModel.ReportingYear = ReportingYearsHelper.GetAccountingStartDate(organisation.SectorType, reportingYear);
            viewModel.IsToSetInScope = false;
            
            if (!ModelState.IsValid)
            {
                return View("OutOfScopeQuestions", viewModel);
            }
            return View("ConfirmScope", viewModel);
        }

        [HttpPost("{encryptedOrganisationId}/reporting-year/{reportingYear}/change-scope/out/confirm")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmOutOfScopeAnswers(string encryptedOrganisationId, int reportingYear, ScopeViewModel viewModel)
        {
            long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
            ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
            ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
            ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);

            // Get Organisation
            Organisation organisation = dataRepository.Get<Organisation>(organisationId);
            
            // Update OrganisationScope
            var reasonForChange = viewModel.WhyOutOfScope == WhyOutOfScope.Under250
                ? "Under250"
                : viewModel.WhyOutOfScopeDetails;
            
            RetireOldScopes(organisation, reportingYear);
            
            UpdateScopes(organisation, ScopeStatuses.OutOfScope, reportingYear, reasonForChange, viewModel.HaveReadGuidance == HaveReadGuidance.Yes);
            
            dataRepository.SaveChanges();

            SendScopeChangeEmails(organisation, viewModel.ReportingYear.Year, ScopeStatuses.OutOfScope);

            OrganisationScope organisationScope = organisation.OrganisationScopes.FirstOrDefault(s => s.ReportingYear == reportingYear);

            viewModel.Organisation = organisation;
            viewModel.ReportingYear = ReportingYearsHelper.GetAccountingStartDate(organisation.SectorType, reportingYear);

            return View("FinishOutOfScopeJourney", viewModel);
        }
        
        [HttpPost("{encryptedOrganisationId}/reporting-year/{reportingYear}/change-scope/in/confirm")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmInScopeAnswers(string encryptedOrganisationId, int reportingYear, ScopeViewModel viewModel)
        {
            long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
            ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
            ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
            ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);

            // Get Organisation and OrganisationScope for reporting year
            Organisation organisation = dataRepository.Get<Organisation>(organisationId);
            
            RetireOldScopes(organisation, reportingYear);
            
            UpdateScopes(organisation, ScopeStatuses.InScope, reportingYear, null, null);
            
            dataRepository.SaveChanges();

            SendScopeChangeEmails(organisation, viewModel.ReportingYear.Year, ScopeStatuses.InScope);

            return RedirectToAction("ScopeDeclared", "Scope", new {encryptedOrganisationId = encryptedOrganisationId, reportingYear = reportingYear});
        }

        [HttpGet("{encryptedOrganisationId}/reporting-year/{reportingYear}/scope-declared")]
        public IActionResult ScopeDeclared(string encryptedOrganisationId, int reportingYear)
        {
            long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
            ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
            ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
            ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);

            // Get Organisation and OrganisationScope for reporting year
            Organisation organisation = dataRepository.Get<Organisation>(organisationId);
            OrganisationScope organisationScope = organisation.GetScopeForYear(reportingYear);

            var viewModel = new ScopeDeclaredViewModel
            {
                Organisation = organisation,
                ReportingYear = organisationScope.ReportingYear,
                ScopeStatus = organisationScope.ScopeStatus
            };
            
            return View("ScopeDeclared", viewModel);
        }
        
        [HttpGet("{encryptedOrganisationId}/declare-scope")]
        [Authorize(Roles = LoginRoles.GpgEmployer)]
        public IActionResult DeclareScopeGet(string encryptedOrganisationId)
        {
            long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
            ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
            ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
            
            var organisation = dataRepository.Get<Organisation>(organisationId);
            if (!OrganisationIsNewThisYearAndHasNotProvidedScopeForLastYear(organisation))
            {
                return RedirectToAction("ManageOrganisationGet", "ManageOrganisations", new { encryptedOrganisationId = encryptedOrganisationId });
            }
            
            var viewModel = new DeclareScopeViewModel
            {
                Organisation = organisation,
                PreviousReportingYear = organisation.SectorType.GetAccountingStartDate().AddYears(-1).Year
            };

            return View("DeclareScope", viewModel);
        }

        [HttpPost("{encryptedOrganisationId}/declare-scope")]
        [ValidateAntiForgeryToken]
        public IActionResult DeclareScopePost(string encryptedOrganisationId, DeclareScopeViewModel viewModel)
        {
            long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
            ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
            ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
            
            var organisation = dataRepository.Get<Organisation>(organisationId);
            if (!OrganisationIsNewThisYearAndHasNotProvidedScopeForLastYear(organisation))
            {
                return RedirectToAction("ManageOrganisationGet", "ManageOrganisations", new { encryptedOrganisationId = encryptedOrganisationId });
            }

            if (ModelState.IsValid)
            {
                int currentReportingYear = ReportingYearsHelper.GetCurrentReportingYear(organisation.SectorType);
                int previousReportingYear = currentReportingYear - 1;

                ScopeStatuses newStatus = viewModel.DeclareScopeRequiredToReport == DeclareScopeRequiredToReportOptions.Yes
                    ? ScopeStatuses.InScope
                    : ScopeStatuses.OutOfScope;
                
                RetireOldScopes(organisation, previousReportingYear);
            
                UpdateScopes(organisation, newStatus, previousReportingYear, null, null);
            
                dataRepository.SaveChanges();
            
                SendScopeChangeEmails(organisation, previousReportingYear, ScopeStatuses.InScope);

                return RedirectToAction("ScopeDeclared", "Scope", new {encryptedOrganisationId = encryptedOrganisationId, reportingYear = previousReportingYear});
            }

            viewModel.Organisation = organisation;
            viewModel.PreviousReportingYear = organisation.SectorType.GetAccountingStartDate().AddYears(-1).Year;

            return View("DeclareScope", viewModel);
        }

        private static bool OrganisationIsNewThisYearAndHasNotProvidedScopeForLastYear(Organisation organisation)
        {
            DateTime currentYearSnapshotDate = organisation.SectorType.GetAccountingStartDate();
            bool organisationCreatedInCurrentReportingYear = organisation.Created >= currentYearSnapshotDate;

            if (organisationCreatedInCurrentReportingYear)
            {
                int previousReportingYear = currentYearSnapshotDate.AddYears(-1).Year;
                OrganisationScope scope = organisation.GetScopeForYear(previousReportingYear);

                if (scope.IsScopePresumed())
                {
                    return true;
                }
            }

            return false;
        }

        public void RetireOldScopes(Organisation organisation, int reportingYear)
        {
            foreach (OrganisationScope s in organisation.OrganisationScopes.Where(o => o.ReportingYear == reportingYear))
            {
                s.Status = ScopeRowStatuses.Retired;
            }
        }

        private void UpdateScopes(Organisation organisation, ScopeStatuses newStatus, int reportingYear, string reasonForChange, bool? haveReadGuidance)
        {
            organisation.SetScopeForYear(
                reportingYear,
                newStatus,
                "Generated by the system",
                reasonForChange,
                haveReadGuidance
                );
        }

        public void SendScopeChangeEmails(Organisation organisation, int reportingYear, ScopeStatuses newScope)
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
}
