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
    public class ActionAddStatusController : Controller
    {

        private readonly IDataRepository dataRepository;

// constructor method, specifies how you construct this method, specifies that you need a dataRepository
        public ActionAddStatusController(
            IDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }
        
        [HttpGet("{encryptedOrganisationId}/reporting-year-{reportingYear}/action-plan/action-status/action-{actionId}/")]
        public IActionResult ActionPlansActionStatusGet(string encryptedOrganisationId, int reportingYear, int actionId)
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


        [HttpPost("{encryptedOrganisationId}/reporting-year-{reportingYear}/action-plan/progress-made")]
        [ValidateAntiForgeryToken]
        public IActionResult ActionPlansProgressMadePost(string encryptedOrganisationId, int reportingYear, ActionPlansProgressMadeViewModel viewModel)
        {
            long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
            ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
            ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
            ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);

            Organisation organisation = dataRepository.Get<Organisation>(organisationId);

            // checking if the view model is not valid
            if (!ModelState.IsValid)
            {
                viewModel.Organisation = organisation;
                viewModel.ReportingYear = reportingYear;
                return View("ActionPlansProgressMade", viewModel);
            }

            ActionPlan actionPlan = organisation.ActionPlans.Where(a => a.ReportingYear == reportingYear).FirstOrDefault();
            if (actionPlan == null)
            {
                actionPlan = new ActionPlan
                {
                    Organisation = organisation,
                    ReportingYear = reportingYear,
                    Status = ActionPlanStatus.Draft
                };
                dataRepository.Insert(actionPlan);
            }

            actionPlan.ProgressMade = viewModel.ProgressMade;
            dataRepository.SaveChanges();


            return RedirectToAction("ManageOrganisationGet", "ManageOrganisations", new {encryptedOrganisationId});

        }

    }
}
