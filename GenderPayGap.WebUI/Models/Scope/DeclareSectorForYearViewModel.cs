using GenderPayGap.Core;
using GenderPayGap.Database;
using GovUkDesignSystemDotNet;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenderPayGap.WebUI.Models.Scope;

public class DeclareSectorForYearViewModel
{
        
    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public Organisation Organisation { get; set; }

    [BindNever /* Output Only - only used for sending data from the Controller to the View */]
    public int ReportingYear { get; set; }

    [GovUkValidateRequiredIf(IsRequiredPropertyName = nameof(SectorRequired), ErrorMessageIfMissing = "Choose a sector")]
    public Sectors? Sector { get; set; }
    public bool SectorRequired => !SkippedDeclaringSector;

    public bool SkippedDeclaringSector { get; set; }

}
