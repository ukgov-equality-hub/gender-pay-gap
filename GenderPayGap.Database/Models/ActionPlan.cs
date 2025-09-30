using GenderPayGap.Core;
using GenderPayGap.Extensions;
using Newtonsoft.Json;

namespace GenderPayGap.Database
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ActionPlan
    {

        [JsonProperty]
        public long ActionPlanId { get; set; }
        [JsonProperty]
        public long OrganisationId { get; set; }
        [JsonProperty]
        public int ReportingYear { get; set; }
        [JsonProperty]
        public DateTime DraftCreatedDate { get; set; } = VirtualDateTime.Now;

        // nullable type  e.g. https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/nullable-value-types
        [JsonProperty]
        public DateTime? SubmittedDate { get; set; } 
        [JsonProperty]
        public DateTime? DeletedDate { get; set; }
        
        [JsonProperty]
        public ActionPlanStatus Status { get; set; }
        [JsonProperty]
        public ActionPlanType? ActionPlanType { get; set; }
        [JsonProperty]
        public string ProgressMade { get; set; }
        [JsonProperty]
        public string MeasuringProgress { get; set; }
       
        [JsonProperty]
        public bool IsLateSubmission { get; set; }
        [JsonProperty]
        public string LateReason { get; set; }

        public virtual Organisation Organisation { get; set; }
    }
}
