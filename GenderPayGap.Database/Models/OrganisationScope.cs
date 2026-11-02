using GenderPayGap.Core;
using GenderPayGap.Extensions;
using Newtonsoft.Json;

namespace GenderPayGap.Database;

[JsonObject(MemberSerialization.OptIn)]
public class OrganisationScope
{

    #region IDs (to identify the row in the database)
    [JsonProperty]
    public long OrganisationScopeId { get; set; }
    [JsonProperty]
    public long OrganisationId { get; set; }
    [JsonProperty]
    public int ReportingYear;
    #endregion

    #region The status of this row in the database (and when it was created)
    [JsonProperty]
    public ScopeRowStatuses Status { get; set; }
    [JsonProperty]
    public DateTime ScopeStatusDate { get; set; } = VirtualDateTime.Now;
    #endregion

    #region Scope information (Scope Status, Read Guidance, Reason, Details)
    [JsonProperty]
    public ScopeStatuses ScopeStatus { get; set; }
    [JsonProperty]
    public bool? ReadGuidance { get; set; }
    [JsonProperty]
    public string Reason { get; set; }
    [JsonProperty]
    public string StatusDetails { get; set; }
    #endregion

    #region Sector information
    [JsonProperty]
    public Sectors? Sector { get; set; }
    [JsonProperty]
    public SectorStatuses SectorStatus { get; set; }
    #endregion
    
    public virtual Organisation Organisation { get; set; }

    public bool IsInScopeVariant()
    {
        return ScopeStatus.IsInScopeVariant();
    }

    public bool IsScopePresumed()
    {
        return ScopeStatus.IsScopePresumed();
    }
    public bool IsScopeDeclared()
    {
        return ScopeStatus.IsScopeDeclared();
    }
        
}