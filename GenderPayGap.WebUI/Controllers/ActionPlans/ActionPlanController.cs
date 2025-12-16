using GenderPayGap.Core;
using GenderPayGap.Core.Helpers;
using GenderPayGap.Core.Interfaces;
using GenderPayGap.Database;
using GenderPayGap.WebUI.ErrorHandling;
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

    [HttpGet("{encryptedOrganisationId}/reporting-year-{reportingYear}/action-plan/actions/{whichAction}")]
    public IActionResult ActionPlanActionGet(string encryptedOrganisationId, int reportingYear, Actions whichAction)
    {
        long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
        ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
        ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
        ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);
        
        Organisation organisation = dataRepository.Get<Organisation>(organisationId);
        ActionPlan actionPlan = organisation.GetLatestSubmittedOrDraftActionPlan(reportingYear);
        ActionInActionPlan actionInActionPlan = actionPlan?.ActionsInActionPlans.FirstOrDefault(a => a.Action == whichAction);
        
        ActionPlanEditActionViewModel viewModel = new()
        {
            Organisation = organisation,
            ReportingYear = reportingYear,
            Action = whichAction,
            Status = actionInActionPlan?.NewStatus,
            SupportingText = actionInActionPlan?.SupportingText,
        };
        
        return View("ActionPlanAction", viewModel);
    }

    [ValidateAntiForgeryToken]
    [HttpPost("{encryptedOrganisationId}/reporting-year-{reportingYear}/action-plan/actions/{whichAction}")]
    public IActionResult ActionPlanActionPost(string encryptedOrganisationId, int reportingYear, Actions whichAction, ActionPlanEditActionViewModel viewModel)
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
            viewModel.Action = whichAction;
            return View("ActionPlanAction", viewModel);
        }
        
        ActionPlan submittedOrDraftActionPlan = organisation.GetLatestSubmittedOrDraftActionPlan(reportingYear);
        ActionInActionPlan actionInSubmittedOrDraftActionPlan = submittedOrDraftActionPlan?.ActionsInActionPlans.FirstOrDefault(a => a.Action == whichAction);
        
        if (UserHasMadeChangesToActionInActionPlan(actionInSubmittedOrDraftActionPlan, viewModel))
        {
            ActionPlan draftActionPlan = GetOrCreateDraftActionPlan(organisation, reportingYear, ActionPlanType.Original);
            ActionInActionPlan actionInDraftActionPlan = draftActionPlan?.ActionsInActionPlans.FirstOrDefault(a => a.Action == whichAction);
            
            switch (viewModel.Status)
            {
                case ActionStatus.DoNotAddToPlan:
                case null:
                    // Status is empty or DoNotAddToPlan
                    if (actionInDraftActionPlan != null)
                    {
                        dataRepository.Delete(actionInDraftActionPlan);
                    }
                    break;

                case ActionStatus.NewOrInProgress:
                case ActionStatus.Completed:
                    if (actionInDraftActionPlan == null)
                    {
                        actionInDraftActionPlan = new ActionInActionPlan
                        {
                            ActionPlan = draftActionPlan,
                            Action = whichAction,
                            NewStatus = viewModel.Status.Value,
                            SupportingText = viewModel.SupportingText,
                        };
                        dataRepository.Insert(actionInDraftActionPlan);
                    }
                    else
                    {
                        actionInDraftActionPlan.NewStatus = viewModel.Status.Value;
                        actionInDraftActionPlan.SupportingText = viewModel.SupportingText;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            dataRepository.SaveChanges();
        } 
         
        return RedirectToAction("ActionPlanListGet", new {encryptedOrganisationId, reportingYear = reportingYear}); 
    }

    private bool UserHasMadeChangesToActionInActionPlan(ActionInActionPlan actionInActionPlan, ActionPlanEditActionViewModel viewModel)
    {
        return actionInActionPlan?.NewStatus != viewModel.Status ||
               actionInActionPlan?.SupportingText != viewModel.SupportingText;
    }

    [HttpGet("{encryptedOrganisationId}/reporting-year-{reportingYear}/action-plan/supporting-narrative-and-link")]
    public IActionResult ActionPlanSupportingNarrativeAndLinkGet(string encryptedOrganisationId, int reportingYear)
    {
        long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
        ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
        ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
        ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);
        
        Organisation organisation = dataRepository.Get<Organisation>(organisationId);
        ActionPlan actionPlan = organisation.GetLatestSubmittedOrDraftActionPlan(reportingYear);
        
        ActionPlanSupportingNarrativeAndLinkViewModel viewModel = new()
        {
            Organisation = organisation,
            ReportingYear = reportingYear,
            SupportingNarrative = actionPlan?.SupportingNarrative,
            LinkToReport = actionPlan?.LinkToReport,
        };
        
        return View("ActionPlanSupportingNarrativeAndLink", viewModel);
    }

    [ValidateAntiForgeryToken]
    [HttpPost("{encryptedOrganisationId}/reporting-year-{reportingYear}/action-plan/supporting-narrative-and-link")]
    public IActionResult ActionPlanSupportingNarrativeAndLinkPost(string encryptedOrganisationId, int reportingYear, ActionPlanSupportingNarrativeAndLinkViewModel viewModel)
    {
        long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
        ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
        ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
        ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);
        
        Organisation organisation = dataRepository.Get<Organisation>(organisationId);

        if (!string.IsNullOrEmpty(viewModel.LinkToReport) &&
            !UriSanitiser.IsValidHttpOrHttpsLink(viewModel.LinkToReport))
        {
            ModelState.AddModelError(nameof(viewModel.LinkToReport), "Please enter a valid web address");
        }
        
        if (!ModelState.IsValid)
        {
            viewModel.Organisation = organisation;
            viewModel.ReportingYear = reportingYear;
            return View("ActionPlanSupportingNarrativeAndLink", viewModel);
        }
        
        ActionPlan submittedOrDraftActionPlan = organisation.GetLatestSubmittedOrDraftActionPlan(reportingYear);
        
        if (UserHasMadeChangesToSupportingNarrativeOrLink(submittedOrDraftActionPlan, viewModel))
        {
            ActionPlan draftActionPlan = GetOrCreateDraftActionPlan(organisation, reportingYear, ActionPlanType.Original);
            
            draftActionPlan.SupportingNarrative = viewModel.SupportingNarrative;
            draftActionPlan.LinkToReport = viewModel.LinkToReport;
            
            dataRepository.SaveChanges();
        }
        
        return RedirectToAction("ActionPlanListGet", new {encryptedOrganisationId, reportingYear = reportingYear});
    }

    private bool UserHasMadeChangesToSupportingNarrativeOrLink(ActionPlan actionPlan, ActionPlanSupportingNarrativeAndLinkViewModel viewModel)
    {
        return actionPlan?.SupportingNarrative != viewModel.SupportingNarrative ||
               actionPlan?.LinkToReport != viewModel.LinkToReport;
    }

    [HttpGet("{encryptedOrganisationId}/reporting-year-{reportingYear}/action-plan/discard-draft")]
    public IActionResult ActionPlanDiscardDraftGet(string encryptedOrganisationId, int reportingYear)
    {
        long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
        ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
        ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
        ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);
        
        Organisation organisation = dataRepository.Get<Organisation>(organisationId);
        ActionPlan actionPlan = organisation.GetLatestDraftActionPlan(reportingYear);

        if (actionPlan == null)
        {
            throw new PageNotFoundException();
        }
        
        return View("ActionPlanDiscardDraft", actionPlan);
    }

    [ValidateAntiForgeryToken]
    [HttpPost("{encryptedOrganisationId}/reporting-year-{reportingYear}/action-plan/discard-draft")]
    public IActionResult ActionPlanDiscardDraftPost(string encryptedOrganisationId, int reportingYear)
    {
        long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
        ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
        ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
        ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);
        
        Organisation organisation = dataRepository.Get<Organisation>(organisationId);
        ActionPlan actionPlan = organisation.GetLatestDraftActionPlan(reportingYear);
        
        if (actionPlan == null)
        {
            throw new PageNotFoundException();
        }

        actionPlan.DeleteActionPlan();
        dataRepository.SaveChanges();

        return View("ActionPlanDraftDiscarded", actionPlan);
    }

    [HttpGet("{encryptedOrganisationId}/reporting-year-{reportingYear}/action-plan/provisional-plan")]
    public IActionResult ActionPlanProvisionalPlanGet(string encryptedOrganisationId, int reportingYear)
    {
        long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
        ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
        ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
        ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);
        
        Organisation organisation = dataRepository.Get<Organisation>(organisationId);
        ActionPlan actionPlan = organisation.GetLatestSubmittedOrDraftActionPlan(reportingYear);
        
        return View("ActionPlanProvisionalPlan", actionPlan);
    }

    [ValidateAntiForgeryToken]
    [HttpPost("{encryptedOrganisationId}/reporting-year-{reportingYear}/action-plan/provisional-plan")]
    public IActionResult ActionPlanProvisionalPlanPost(string encryptedOrganisationId, int reportingYear)
    {
        long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
        ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
        ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
        ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);
        
        Organisation organisation = dataRepository.Get<Organisation>(organisationId);
        ActionPlan actionPlan = organisation.GetLatestSubmittedOrDraftActionPlan(reportingYear);

        if (actionPlan == null)
        {
            throw new PageNotFoundException();
        }
        else if (actionPlan.Status == ActionPlanStatus.Submitted)
        {
            ModelState.AddModelError(
                "edit-action-plan-link",
                "This equality action plan has already been published. Please make some changes before trying to publish it again"
            );
            return View("ActionPlanProvisionalPlan", actionPlan);
        }
        else if (!actionPlan.HasFulfilledRequirementsToPublish())
        {
            ModelState.AddModelError(
                "edit-action-plan-link",
                $"You must select at least one action from the \"{ActionCategories.SupportingStaffDuringMenopause.GetDisplayName()}\" category "
                + "and at least one action from any other category. "
                + "Actions that are already fully embedded do not count for this purpose. "
                + "Your plan must have actions with which you can make further progress."
            );
            return View("ActionPlanProvisionalPlan", actionPlan);
        }
        else
        {
            actionPlan.SubmitActionPlan();
            dataRepository.SaveChanges();

            return RedirectToAction("ActionPlanSubmittedConfirmationGet", new {encryptedOrganisationId, reportingYear = reportingYear});
        }
    }

    [HttpGet("{encryptedOrganisationId}/reporting-year-{reportingYear}/action-plan/submitted-confirmation")]
    public IActionResult ActionPlanSubmittedConfirmationGet(string encryptedOrganisationId, int reportingYear)
    {
        long organisationId = ControllerHelper.DecryptOrganisationIdOrThrow404(encryptedOrganisationId);
        ControllerHelper.ThrowIfUserAccountRetiredOrEmailNotVerified(User, dataRepository);
        ControllerHelper.ThrowIfUserDoesNotHavePermissionsForGivenOrganisation(User, dataRepository, organisationId);
        ControllerHelper.ThrowIfReportingYearIsOutsideOfRange(reportingYear, organisationId, dataRepository);
        
        Organisation organisation = dataRepository.Get<Organisation>(organisationId);
        ActionPlan actionPlan = organisation.GetLatestSubmittedOrDraftActionPlan(reportingYear);

        if (actionPlan?.Status != ActionPlanStatus.Submitted)
        {
            throw new PageNotFoundException();
        }
        
        return View("ActionPlanSubmittedConfirmation", actionPlan);
    }


    private ActionPlan GetOrCreateDraftActionPlan(Organisation organisation, int reportingYear, ActionPlanType actionPlanType)
    {
        ActionPlan submittedOrDraftActionPlan = organisation.GetLatestSubmittedOrDraftActionPlan(reportingYear);

        switch (submittedOrDraftActionPlan?.Status)
        {
            case ActionPlanStatus.Draft:
                // This is the Draft Action Plan - let's use it
                return submittedOrDraftActionPlan;

            case ActionPlanStatus.Submitted:
                // This is a Submitted Action Plan - take a clone of it and make that the new Draft Action Plan 
                ActionPlan newDraftActionPlan = new()
                {
                    Organisation = organisation,
                    ReportingYear = reportingYear,
                    Status = ActionPlanStatus.Draft,
                        
                    ActionPlanType = actionPlanType,
                };
                dataRepository.Insert(newDraftActionPlan);

                foreach (ActionInActionPlan actionInSubmittedActionPlan in submittedOrDraftActionPlan.ActionsInActionPlans)
                {
                    ActionInActionPlan actionInDraftActionPlan = new ActionInActionPlan
                    {
                        ActionPlan = newDraftActionPlan,
                        Action = actionInSubmittedActionPlan.Action,
                        NewStatus = actionInSubmittedActionPlan.NewStatus,
                        SupportingText = actionInSubmittedActionPlan.SupportingText,
                    };
                    newDraftActionPlan.ActionsInActionPlans.Add(actionInDraftActionPlan);
                    dataRepository.Insert(actionInDraftActionPlan);
                }
                dataRepository.SaveChanges();
                return newDraftActionPlan;

            case null:
                // There is no Submitted or Draft Action Plan - create a new Draft Action Plan
                ActionPlan draftActionPlan = new()
                {
                    Organisation = organisation,
                    ReportingYear = reportingYear,
                    Status = ActionPlanStatus.Draft,
                        
                    ActionPlanType = actionPlanType,
                };
                dataRepository.Insert(draftActionPlan);
                dataRepository.SaveChanges();
                return draftActionPlan;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

}