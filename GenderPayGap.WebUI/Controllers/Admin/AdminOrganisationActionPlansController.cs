using GenderPayGap.Core;
using GenderPayGap.Core.Interfaces;
using GenderPayGap.Database;
using GenderPayGap.Extensions;
using GenderPayGap.WebUI.Helpers;
using GenderPayGap.WebUI.Models.Admin;
using GenderPayGap.WebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenderPayGap.WebUI.Controllers.Admin;

[Authorize(Roles = LoginRoles.GpgAdmin + "," + LoginRoles.GpgAdminReadOnly)]
[Route("admin")]
public class AdminOrganisationActionPlansController : Controller
{

    private readonly AuditLogger auditLogger;

    private readonly IDataRepository dataRepository;

    public AdminOrganisationActionPlansController(IDataRepository dataRepository, AuditLogger auditLogger)
    {
        this.dataRepository = dataRepository;
        this.auditLogger = auditLogger;
    }

    [HttpGet("organisation/{id}/action-plans")]
    public IActionResult ViewActionPlans(long id)
    {
        Organisation organisation = dataRepository.Get<Organisation>(id);

        return View("ViewActionPlans", organisation);
    }

    [HttpGet("organisation/{id}/action-plans/{year}")]
    public IActionResult ViewActionPlanDetailsForYear(long id, int year)
    {
        Organisation organisation = dataRepository.Get<Organisation>(id);

        var viewModel = new AdminOrganisationActionPlanDetailsViewModel
        {
            Organisation = organisation,
            Year = year
        };

        return View("ViewActionPlanDetails", viewModel);
    }

    [HttpGet("organisation/{id}/action-plans/{year}/delete")]
    [Authorize(Roles = LoginRoles.GpgAdmin)]
    public IActionResult DeleteActionPlansOfAYearGet(long id, int year)
    {
        var organisation = dataRepository.Get<Organisation>(id);
        List<long> actionPlanIds = organisation.ActionPlans
            .Where(ap => ap.ReportingYear == year)
            .Where(ap => ap.Status != ActionPlanStatus.Deleted && ap.Status != ActionPlanStatus.DeletedDraft)
            .Select(ap => ap.ActionPlanId)
            .ToList();

        var viewModel = new AdminDeleteActionPlanViewModel {Organisation = organisation, ActionPlanIds = actionPlanIds, Year = year};

        return View("DeleteActionPlans", viewModel);
    }

    [HttpGet("organisation/{id}/action-plans/{year}/delete/{actionPlanId}")]
    [Authorize(Roles = LoginRoles.GpgAdmin)]
    public IActionResult DeleteActionPlanGet(long id, int year, long actionPlanId)
    {
        var organisation = dataRepository.Get<Organisation>(id);

        if (!organisation.ActionPlans.Select(ap => ap.ActionPlanId).Contains(actionPlanId))
        {
            throw new Exception("The ActionPlanID that the user requested to be deleted does not belong to this Organisation");
        }
        var actionPlanIds = new List<long>{ actionPlanId };

        var viewModel = new AdminDeleteActionPlanViewModel {Organisation = organisation, ActionPlanIds = actionPlanIds, Year = year};

        return View("DeleteActionPlans", viewModel);
    }

    [HttpPost("organisation/{id}/action-plans/{year}/delete")]
    [Authorize(Roles = LoginRoles.GpgAdmin)]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteActionPlansPost(long id, int year, AdminDeleteActionPlanViewModel viewModel)
    {
        var organisation = dataRepository.Get<Organisation>(id);
        User currentUser = ControllerHelper.GetGpgUserFromAspNetUser(User, dataRepository);

        if (!ModelState.IsValid)
        {
            viewModel.Organisation = organisation;
            viewModel.Year = year;
            return View("DeleteActionPlans", viewModel);
        }

        foreach (long actionPlanId in viewModel.ActionPlanIds)
        {
            if (!organisation.ActionPlans.Select(ap => ap.ActionPlanId).Contains(actionPlanId))
            {
                throw new Exception("The ActionPlanID that the user requested to be deleted does not belong to this Organisation");
            }

            ActionPlan actionPlan = organisation.ActionPlans.Single(ap => ap.ActionPlanId == actionPlanId);
            if (actionPlan.Status == ActionPlanStatus.Deleted || actionPlan.Status == ActionPlanStatus.DeletedDraft)
            {
                throw new Exception("The ActionPlanID that the user requested to be deleted has already been deleted");
            }
        }
        
        foreach (long actionPlanId in viewModel.ActionPlanIds)
        {
            ActionPlan actionPlan = dataRepository.Get<ActionPlan>(actionPlanId);
            if (actionPlan.Status == ActionPlanStatus.Draft)
            {
                actionPlan.Status = ActionPlanStatus.DeletedDraft;
            }
            else
            {
                actionPlan.Status = ActionPlanStatus.Deleted;
            }

            actionPlan.DeletedDate = VirtualDateTime.Now;
        }
        
        // dataRepository.SaveChanges is called from within the auditLogger.AuditChangeToOrganisation method, so we don't need to save here
        // dataRepository.SaveChanges();

        // Audit log
        auditLogger.AuditChangeToOrganisation(
            AuditedAction.AdminDeleteActionPlan,
            organisation,
            new
            {
                ReportingYear= year,
                ActionPlanIds = string.Join(", ", viewModel.ActionPlanIds),
                Reason = viewModel.Reason
            },
            User);

        return RedirectToAction("ViewActionPlans", "AdminOrganisationActionPlans", new {id});
    }

}