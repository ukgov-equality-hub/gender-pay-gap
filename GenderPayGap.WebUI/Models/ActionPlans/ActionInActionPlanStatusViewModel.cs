using GenderPayGap.Database;
using GenderPayGap.Core;
using GovUkDesignSystemDotNet;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenderPayGap.WebUI.Models.ActionPlans
{
    // public class ActionInActionPlanStatusViewModel 
    // {

    //         [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    //         public Organisation Organisation { get; set; }
    //         [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    //         public int ReportingYear { get; set; }

    //         [GovUkValidateRequired(ErrorMessageIfMissing = "Please select a status for this action")]
    //         public ActionStatus? ActionStatus { get; set; }

    //     }



    //     public enum ActionStatusesLabels : byte
    //     {
    //         [GovUkRadioCheckboxLabelText(Text = "We have already embedded this in our working culture")]
    //         Embedded = 0,

    //         [GovUkRadioCheckboxLabelText(Text = "We are working on embedding this but it is not yet routine")]
    //         InProgress = 1,

    //         [GovUkRadioCheckboxLabelText(Text = "We would like to consider this practice - provisionally add it to our action plan")]
    //         AddToPlan = 2,

    //         [GovUkRadioCheckboxLabelText(Text = "We are not currently pursuing this practice")]
    //         NotPursuing = 3
    //     }

    //     public enum ActionStatusesLabels2
    //     {
    //         Embedded,
    //         InProgress,
    //         AddToPlan,
    //         NotPursuing
    //     }ActionInActionPlanStatusViewModel


    public class ActionInActionPlanStatusViewModel
    {
        [BindNever /* Output Only - only used for sending data from the Controller to the View */]
        public Organisation Organisation { get; set; }
        [BindNever /* Output Only - only used for sending data from the Controller to the View */]
        public int ReportingYear { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Please select a status for this action")]
        public ActionPlanActionStatuses? ActionPlanActionStatus { get; set; }
        
        public ActionStatus GetDBActionStatus()
        {
            switch (ActionPlanActionStatus)
            {
                case ActionPlanActionStatuses.Embedded:
                    return ActionStatus.Embedded;
                case ActionPlanActionStatuses.InProgress:
                    return ActionStatus.InProgress;
                case ActionPlanActionStatuses.AddToPlan:
                    return ActionStatus.AddToPlan;
                case ActionPlanActionStatuses.NotPursuing:
                    return ActionStatus.NotPursuing;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        

    }


    public enum ActionPlanActionStatuses : byte
    {
        [GovUkRadioCheckboxLabelText(Text = "We have already embedded this in our working culture")]
        Embedded = 1,
        [GovUkRadioCheckboxLabelText(Text = "We are working on embedding this but it is not yet routine")]
        InProgress = 2,
        [GovUkRadioCheckboxLabelText(Text = "We would like to consider this practice - provisionally add it to our action plan")]
        AddToPlan = 3,
        [GovUkRadioCheckboxLabelText(Text = "We are not currently pursuing this practice")]
        NotPursuing = 4
    }
    
     

}

