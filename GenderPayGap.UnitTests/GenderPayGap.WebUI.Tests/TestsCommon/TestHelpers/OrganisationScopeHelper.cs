using GenderPayGap.Core;
using GenderPayGap.Core.Helpers;
using GenderPayGap.Database;

namespace GenderPayGap.Tests.Common.TestHelpers
{
    public class OrganisationScopeHelper
    {

        public static OrganisationScope GetOrganisationScope(int snapshotYear)
        {
            return new OrganisationScope {
                ReportingYear = snapshotYear,
                Status = ScopeRowStatuses.Active
            };
        }

        public static OrganisationScope GetOrgScopeWithThisScope(int snapshotYear, SectorTypes organisationSectorType, ScopeStatuses scope)
        {
            if (snapshotYear == 0)
            {
                snapshotYear = organisationSectorType.GetAccountingStartDate().Year;
            }

            OrganisationScope org = GetOrganisationScope(snapshotYear);
            org.ScopeStatus = scope;
            return org;
        }

    }
}
