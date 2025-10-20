using GenderPayGap.Core;
using GenderPayGap.Extensions;
using Newtonsoft.Json;

namespace GenderPayGap.Database
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ActionInActionPlan
    {
        [JsonProperty]
        public long ActioninActionPlanId { get; set; }
        [JsonProperty]
        public long ActionPlanId { get; set; }
        [JsonProperty]
        public Actions ActionId { get; set; }
        [JsonProperty]
        public ActionStatus OldStatus { get; set; }
        [JsonProperty]
        public ActionStatus NewStatus { get; set; }
        [JsonProperty]
        public string SupportingNarrative { get; set; }
        [JsonProperty]
        public string EvaluationThreeYear { get; set; }
        [JsonProperty]
        public string SupportingInformationThreeYear { get; set; }
        [JsonProperty]
        public virtual ActionPlan ActionPlan { get; set; }

    }
}
