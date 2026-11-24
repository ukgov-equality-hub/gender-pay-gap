using GenderPayGap.Core;

namespace GenderPayGap.WebUI.Models.CaseStudies;

public class CaseStudyViewModel
{

    public string Slug { get; set; }
    public string Employer { get; set; }
    public int OrganisationId { get; set; }
    public Sectors Sector { get; set; }
    
    public string NumberOfEmployees { get; set; }
    public OrganisationSizes NumberOfEmployeesParsed => OrganisationSizesExtensions.TryParseFromDisplayNameKebabCase(NumberOfEmployees);
    
    public string ActionCategory { get; set; }
    public string ActionCategoryParsed =>
        ActionCategory switch
        {
            "gender-pay-gap" => "Gender pay gap",
            "menopause" => "Menopause",
            _ => ActionCategory
        };

    public string Title { get; set; }
    public string ShortSummary { get; set; }
    public string LongSummary { get; set; }
    public string Html { get; set; }

}
