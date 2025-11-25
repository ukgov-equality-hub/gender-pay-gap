using GenderPayGap.Core;

namespace GenderPayGap.BusinessLogic.Tests.Services
{
    [TestFixture]
    public class EnumTests
    {

        [Test]
        public void SomeEnumsMustHaveUniqueNumericValues()
        {
            EnumMustHaveUniqueValues<AuditedAction, int>();
        }

        private static void EnumMustHaveUniqueValues<TEnum, TNumeric>()
        where TEnum : Enum
        {
            List<TEnum> enumOptions = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .ToList();

            foreach (TEnum enumOption in enumOptions)
            {
                TNumeric numericValueOfEnumOption = (TNumeric)(object) enumOption;

                int numberOfEnumOptionsWithThisNumericValue = enumOptions
                    .Select(eo => (TNumeric)(object) eo)
                    .Count(eo => eo.Equals(numericValueOfEnumOption));

                Assert.AreEqual(
                    1,
                    numberOfEnumOptionsWithThisNumericValue,
                    $"{typeof(TEnum)} enum has {numberOfEnumOptionsWithThisNumericValue} options with the numerical "
                    + $"value {numericValueOfEnumOption}. The numerical values of an enum must be unique.");
            }
        }

    }
}
