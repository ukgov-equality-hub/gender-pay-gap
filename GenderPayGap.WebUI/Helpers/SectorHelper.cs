using GenderPayGap.Core;

namespace GenderPayGap.WebUI.Helpers;

public class SectorDefinition
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Includes { get; set; }
}

public static class SectorHelper
{

    public static Dictionary<Sectors, SectorDefinition> SectorDefinitions = new()
    {
        {
            Sectors.ProfessionalAndConsultancyServices, new()
            {
                Name = "Professional and consultancy services",
                Description = "Firms providing specialized knowledge, expert advice, or creative services directly to businesses and individuals.",
                Includes = "Law firms, accounting practices, management consultancies, architectural studios, design agencies, and scientific research labs."
            }
        },
        {
            Sectors.RetailSalesAndConsumerServices, new()
            {
                Name = "Retail, sales and consumer services",
                Description = "Businesses that sell physical products to consumers or trade partners, along with personal day-to-day service providers.",
                Includes = "Shops, online retailers, wholesalers, car dealerships, repair garages, hair and beauty salons, and dry cleaners."
            }
        },
        {
            Sectors.BusinessOperationsAndSupport, new()
            {
                Name = "Business operations and support",
                Description = "Companies that provide day-to-day operational support to keep other businesses and facilities running smoothly.",
                Includes = "Recruitment agencies, security firms, commercial cleaning services, call centers, equipment rental, and office administration."
            }
        },
        {
            Sectors.EducationAndTraining, new()
            {
                Name = "Education and training",
                Description = "Institutions and organizations focused on learning, skill development, and academic research.",
                Includes = "Primary and secondary schools, universities, vocational colleges, corporate training providers, and early childhood care."
            }
        },
        {
            Sectors.ManufacturingAndProduction, new()
            {
                Name = "Manufacturing and production",
                Description = "Businesses that turn raw materials or components into physical products on an industrial scale.",
                Includes = "Food and beverage processing, pharmaceuticals, electronics, textiles, machinery, and consumer goods manufacturing."
            }
        },
        {
            Sectors.ConstructionLogisticsAndEnergy, new()
            {
                Name = "Construction, logistics and energy",
                Description = "The core industries that build physical infrastructure, transport goods, and supply vital essential resources.",
                Includes = "Builders, civil engineering, freight and warehousing, electricity and gas supply, water treatment, mining, and agriculture."
            }
        },
        {
            Sectors.PublicSectorAndNonProfit, new()
            {
                Name = "Public sector and non-profit",
                Description = "Publicly funded bodies, civil administration, civic groups, and international bodies serving the broader public interest.",
                Includes = "Local councils, emergency services, government agencies, defense bodies, and international non-governmental organizations."
            }
        },
        {
            Sectors.FinanceBankingAndRealEstate, new()
            {
                Name = "Finance, banking and real estate",
                Description = "Organizations that manage money, financial investments, insurance, and physical land or property assets.",
                Includes = "Commercial banks, insurance providers, investment funds, estate agencies, property management companies, and mortgage brokers."
            }
        },
        {
            Sectors.HealthcareAndSocialCare, new()
            {
                Name = "Healthcare and social care",
                Description = "Providers of medical treatment, clinical care, and personal social support for individuals and families.",
                Includes = "Hospitals, medical practices, dental clinics, residential care homes, mental health services, and community support orgs."
            }
        },
        {
            Sectors.HospitalityLeisureAndCulture, new()
            {
                Name = "Hospitality, leisure and culture",
                Description = "Venues and services dedicated to travel, dining, recreation, sports, entertainment, and the arts.",
                Includes = "Hotels, restaurants, bars, gyms, sports clubs, theaters, museums, and event venues."
            }
        },
        {
            Sectors.TechMediaAndTelecommunications, new()
            {
                Name = "Tech, media and telecommunications",
                Description = "Organizations that create digital products, manage communications infrastructure, or produce content and media.",
                Includes = "Software companies, IT support providers, telecom networks, digital publishers, broadcasting, and film production."
            }
        }
    };

}
