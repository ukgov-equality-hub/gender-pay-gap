using System.Diagnostics;
using GenderPayGap.Core;
using GenderPayGap.Extensions;

namespace GenderPayGap.Database;

[Serializable]
[DebuggerDisplay("{OrganisationName},{Status}")]
public partial class ActionPlan
{

    public void DeleteActionPlan()
    {
        switch (Status)
        {
            case ActionPlanStatus.Draft:
                Status = ActionPlanStatus.DeletedDraft;
                DeletedDate = VirtualDateTime.Now;
                break;
            
            case ActionPlanStatus.Submitted:
            case ActionPlanStatus.Retired:
                Status = ActionPlanStatus.Deleted;
                DeletedDate = VirtualDateTime.Now;
                break;
            
            case ActionPlanStatus.Deleted:
            case ActionPlanStatus.DeletedDraft:
            default:
                break;
        }
    }
    
}