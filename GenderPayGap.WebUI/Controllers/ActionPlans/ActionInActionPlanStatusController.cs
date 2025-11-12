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
    public class ActionInActionPlanStatusController : Controller
    {

        private readonly IDataRepository dataRepository;

// constructor method, specifies how you construct this method, specifies that you need a dataRepository
        public ActionInActionPlanStatusController(
            IDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        

        
        [HttpGet("{encryptedOrganisationId}/reporting-year-{reportingYear}/action-plan/action-status/{actionId}/")]
        public IActionResult ActionPlansActionStatusGet(string encryptedOrganisationId, int reportingYear, Actions actionId)
        {
            long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
            ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
            ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
            ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);

            Organisation organisation = dataRepository.Get<Organisation>(organisationId);

            // var viewModel = new ActionInActionPlanStatusViewModel
            // {
            //     Organisation = organisation,
            //     ReportingYear = reportingYear
            // };

            var viewModel = new ActionInActionPlanStatusViewModel
            {
                Organisation = organisation,
                ReportingYear = reportingYear
            };
            

            ActionPlan actionPlan = organisation.ActionPlans.Where(a => a.ReportingYear == reportingYear).FirstOrDefault();
            if (actionPlan != null)
            {
                ActionInActionPlan actionInActionPlan = actionPlan.ActionsinActionPlans.Where(a => a.ActionId == actionId).FirstOrDefault();
                if (actionInActionPlan != null)
                {
                    if (actionInActionPlan.NewStatus == ActionStatus.Embedded)
                    { viewModel.ActionPlanActionStatus = ActionPlanActionStatuses.Embedded; }
                    else if (actionInActionPlan.NewStatus == ActionStatus.InProgress)
                    { viewModel.ActionPlanActionStatus = ActionPlanActionStatuses.InProgress; }

                    // viewModel.NewSector = viewModel.Organisation.SectorType == SectorTypes.Private ? NewSectorTypes.Private : NewSectorTypes.Public;

                }
            }

            return View("ActionInActionPlanStatus", viewModel);
        }


        // [HttpPost("{encryptedOrganisationId}/reporting-year-{reportingYear}/action-plan/progress-made")]
        // [ValidateAntiForgeryToken]
        // public IActionResult ActionPlansProgressMadePost(string encryptedOrganisationId, int reportingYear, ActionPlansProgressMadeViewModel viewModel)
        // {
        //     long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
        //     ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
        //     ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
        //     ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);

        //     Organisation organisation = dataRepository.Get<Organisation>(organisationId);

        //     // checking if the view model is not valid
        //     if (!ModelState.IsValid)
        //     {
        //         viewModel.Organisation = organisation;
        //         viewModel.ReportingYear = reportingYear;
        //         return View("ActionPlansProgressMade", viewModel);
        //     }

        //     ActionPlan actionPlan = organisation.ActionPlans.Where(a => a.ReportingYear == reportingYear).FirstOrDefault();
        //     if (actionPlan == null)
        //     {
        //         actionPlan = new ActionPlan
        //         {
        //             Organisation = organisation,
        //             ReportingYear = reportingYear,
        //             Status = ActionPlanStatus.Draft
        //         };
        //         dataRepository.Insert(actionPlan);
        //     }

        //     actionPlan.ProgressMade = viewModel.ProgressMade;
        //     dataRepository.SaveChanges();


        //     return RedirectToAction("ManageOrganisationGet", "ManageOrganisations", new {encryptedOrganisationId});

        // }

    }
}
