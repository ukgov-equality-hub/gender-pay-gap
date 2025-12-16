using GenderPayGap.Core.Interfaces;
using GenderPayGap.Database;
using GenderPayGap.WebUI.Helpers;
using GenderPayGap.WebUI.Models.ActionPlans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenderPayGap.WebUI.Controllers.ActionPlans;

[Authorize(Roles = LoginRoles.GpgEmployer)]
[Route("account/organisations")]
public class ActionPlanController: Controller
{
    private readonly IDataRepository dataRepository;
    
    public ActionPlanController(IDataRepository dataRepository)
    {
        this.dataRepository = dataRepository;
    }

    [HttpGet("{encryptedOrganisationId}/reporting-year-{reportingYear}/action-plan/intro")]
    public IActionResult ActionPlanIntroGet(string encryptedOrganisationId, int reportingYear)
    {
        long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
        ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
        ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
        ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);
        
        Organisation organisation = dataRepository.Get<Organisation>(organisationId);
        
        ActionPlanIntroViewModel viewModel = new ActionPlanIntroViewModel
        {
            Organisation = organisation,
            ReportingYear = reportingYear
        };
        
        return View("ActionPlanIntro", viewModel);
    }

    [HttpGet("{encryptedOrganisationId}/reporting-year-{reportingYear}/action-plan/actions-list")]
    public IActionResult ActionPlanListGet(string encryptedOrganisationId, int reportingYear)
    {
        long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
        ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
        ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
        ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);
        
        Organisation organisation = dataRepository.Get<Organisation>(organisationId);
        ActionPlan actionPlan = organisation.GetLatestSubmittedOrDraftActionPlan(reportingYear);
        
        ActionPlanListViewModel viewModel = new ActionPlanListViewModel
        {
            Organisation = organisation,
            ReportingYear = reportingYear,
            ActionPlan = actionPlan
        };
        
        return View("ActionPlanList", viewModel);
    }

}