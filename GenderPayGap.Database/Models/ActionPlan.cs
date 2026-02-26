using GenderPayGap.Core;
using GenderPayGap.Extensions;
using Newtonsoft.Json;

namespace GenderPayGap.Database;

[JsonObject(MemberSerialization.OptIn)]
public partial class ActionPlan
{
    public ActionPlan()
    {
        ActionsInActionPlans = new HashSet<ActionInActionPlan>();
    }

    [JsonProperty]
    public long ActionPlanId { get; set; }
    [JsonProperty]
    public long OrganisationId { get; set; }
    [JsonProperty]
    public int ReportingYear { get; set; }

    [JsonProperty]
    public DateTime DraftCreatedDate { get; set; } = VirtualDateTime.Now;
    [JsonProperty]
    public DateTime? SubmittedDate { get; set; }
    [JsonProperty]
    public DateTime? DeletedDate { get; set; }

    [JsonProperty]
    public ActionPlanStatus Status { get; set; }
    [JsonProperty]
    public ActionPlanType ActionPlanType { get; set; }

    [JsonProperty]
    public string SupportingNarrative { get; set; }
    [JsonProperty]
    public string LinkToReport { get; set; }

    [JsonProperty]
    public string ResponsiblePersonFirstName { get; set; }
    [JsonProperty]
    public string ResponsiblePersonLastName { get; set; }
    [JsonProperty]
    public string ResponsiblePersonJobTitle { get; set; }

    public virtual Organisation Organisation { get; set; }

    public virtual ICollection<ActionInActionPlan> ActionsInActionPlans { get; set; }
}
