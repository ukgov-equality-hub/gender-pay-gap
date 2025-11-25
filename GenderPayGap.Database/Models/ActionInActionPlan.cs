using GenderPayGap.Core;
using Newtonsoft.Json;

namespace GenderPayGap.Database;

[JsonObject(MemberSerialization.OptIn)]
public class ActionInActionPlan
{
    [JsonProperty]
    public long ActionInActionPlanId { get; set; }
    [JsonProperty]
    public long ActionPlanId { get; set; }
    [JsonProperty]
    public Actions Action { get; set; }

    [JsonProperty]
    public ActionStatus? OldStatus { get; set; }
    [JsonProperty]
    public ActionStatus NewStatus { get; set; }

    [JsonProperty]
    public string SupportingText { get; set; }

    public virtual ActionPlan ActionPlan { get; set; }

}
