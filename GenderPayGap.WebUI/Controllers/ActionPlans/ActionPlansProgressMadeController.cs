using GenderPayGap.Core;
using GenderPayGap.Core.Interfaces;
using GenderPayGap.Database;
using GenderPayGap.Database.Models;
using GenderPayGap.WebUI.Helpers;
using GenderPayGap.WebUI.Models.ActionPlans;
using GenderPayGap.WebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenderPayGap.WebUI.Controllers.ActionPlans
{
    [Authorize(Roles = LoginRoles.GpgEmployer)]
    [Route("account/organisations")]
    public class ActionPlansProgressMadeController : Controller
    {

        private readonly IDataRepository dataRepository;

// constructor method, specifies how you construct this method, specifies that you need a dataRepository
        public ActionPlansProgressMadeController(
            IDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }
        
        [HttpGet("{encryptedOrganisationId}/reporting-year-{reportingYear}/action-plan/progress-made")]
        public IActionResult ActionPlansProgressMadeGet(string encryptedOrganisationId, int reportingYear)
        {
            long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
            ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
            ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
            ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);

            Organisation organisation = dataRepository.Get<Organisation>(organisationId);

            var viewModel = new ActionPlansProgressMadeViewModel
            {
                Organisation = organisation,
                ReportingYear = reportingYear
            };

            ActionPlan actionPlan = organisation.ActionPlans.Where(a => a.ReportingYear == reportingYear).FirstOrDefault();
            if (actionPlan != null)
            {
                viewModel.ProgressMade = actionPlan.ProgressMade;
            }

            return View("ActionPlansProgressMade", viewModel);
        }
        
        
        [HttpPost("{encryptedOrganisationId}/reporting-year-{reportingYear}/report/responsible-person")]
        [ValidateAntiForgeryToken]
        public IActionResult ActionPlansProgressMadePost(string encryptedOrganisationId, int reportingYear, ActionPlansProgressMadeViewModel viewModel)
        {
            long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
            ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
            ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
            ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);

            Organisation organisation = dataRepository.Get<Organisation>(organisationId);
            if (organisation.SectorType == SectorTypes.Public)
            {
                string nextPagePublicSectorUrl = Url.Action("ReportOverview", "ReportOverview", new { encryptedOrganisationId, reportingYear });
                StatusMessageHelper.SetStatusMessage(Response, "Public authority employers are not required to provide a person responsible", nextPagePublicSectorUrl);
                return LocalRedirect(nextPagePublicSectorUrl);
            }

            if (!ModelState.IsValid)
            {
                PopulateViewModel(viewModel, organisationId, reportingYear, viewModel.IsEditingForTheFirstTime);
                return View("ReportResponsiblePerson", viewModel);
            }
            SaveChangesToDraftReturn(viewModel, organisationId, reportingYear);

            var actionValues = new { encryptedOrganisationId, reportingYear, initialSubmission = viewModel.IsEditingForTheFirstTime };
            string nextPageUrl = viewModel.IsEditingForTheFirstTime
                ? Url.Action("ReportSizeOfOrganisationGet", "ReportSizeOfOrganisation", actionValues)
                : Url.Action("ReportOverview", "ReportOverview", actionValues);

            StatusMessageHelper.SetStatusMessage(Response, "Saved changes to draft", nextPageUrl);
            return LocalRedirect(nextPageUrl);
        }

        private void SaveChangesToDraftReturn(ReportResponsiblePersonViewModel viewModel, long organisationId, int reportingYear)
        {
            DraftReturn draftReturn = draftReturnService.GetOrCreateDraftReturn(organisationId, reportingYear);

            draftReturn.FirstName = viewModel.ResponsiblePersonFirstName;
            draftReturn.LastName = viewModel.ResponsiblePersonLastName;
            draftReturn.JobTitle = viewModel.ResponsiblePersonJobTitle;

            draftReturnService.SaveDraftReturnOrDeleteIfNotRelevant(draftReturn);
        }

    }
}
