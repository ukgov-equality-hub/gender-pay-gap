using GenderPayGap.Core.Helpers;

namespace GenderPayGap.Database;

public partial class ActionInActionPlan
{
    public ActionDetails ActionDetails => ActionsHelper.DictionaryOfAllActions[Action];
}
