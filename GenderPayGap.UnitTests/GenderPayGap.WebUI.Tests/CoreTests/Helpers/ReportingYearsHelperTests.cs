using GenderPayGap.Core.Helpers;

namespace GenderPayGap.Core.Tests.Helpers
{
    [TestFixture]
    public class ReportingYearsHelperTests
    {

        private static List<int> reportingStartYearsWithFurloughScheme = Global.ReportingStartYearsWithFurloughScheme;

        private static List<int> reportingStartYearsWithoutFurloughScheme =
            ReportingYearsHelper.GetReportingYears()
                .Except(reportingStartYearsWithFurloughScheme)
                .ToList();

        [TestCaseSource(nameof(reportingStartYearsWithFurloughScheme))]
        public void IsReportingYearWithFurloughScheme_Returns_True_Given_A_Year_With_Furlough_Scheme(int year)
        {
            // Act
            bool actualResult = ReportingYearsHelper.IsReportingYearWithFurloughScheme(year);

            // Assert
            Assert.AreEqual(true, actualResult);
        }

        [TestCaseSource(nameof(reportingStartYearsWithoutFurloughScheme))]
        public void IsReportingYearWithFurloughScheme_Returns_False_Given_A_Year_Without_Furlough_Scheme(int year)
        {
            // Act
            bool actualResult = ReportingYearsHelper.IsReportingYearWithFurloughScheme(year);

            // Assert
            Assert.AreEqual(false, actualResult);
        }

    }
}
