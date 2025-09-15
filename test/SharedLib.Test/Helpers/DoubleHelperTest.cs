using System.Globalization;
using SharedLib.Helpers;

namespace SharedLib.Test.Helpers
{
    public class DoubleHelperTests
    {
        private const double Epsilon = 1E-15;

        #region TryParseInvariantCulture

        [Theory]
        [ClassData(typeof(PeriodDecimalPointCultureInfo))]
        [ClassData(typeof(CommaDecimalPointCultureInfo))]
        public void TryParse_Integer(string cultureName)
        {
            const string Input = "123";
            SetCurrentCulture(cultureName);

            var actual = DoubleHelper.TryParse(Input, out double result);
            Assert.True(actual);
            Assert.Equal(123, result, Epsilon);
        }

        [Theory]
        [ClassData(typeof(PeriodDecimalPointCultureInfo))]
        [ClassData(typeof(CommaDecimalPointCultureInfo))]
        public void TryParse_NegativeInteger(string cultureName)
        {
            const string Input = "-456789";
            SetCurrentCulture(cultureName);

            var actual = DoubleHelper.TryParse(Input, out double result);
            Assert.True(actual);
            Assert.Equal(-456789, result, Epsilon);
        }

        [Theory]
        [ClassData(typeof(PeriodDecimalPointCultureInfo))]
        public void TryParse_IntegerWithThousandPointForPointCulture(string cultureName)
        {
            const string Input = "1,205,000";
            SetCurrentCulture(cultureName);

            var actual = DoubleHelper.TryParse(Input, out double result);
            Assert.True(actual);
            Assert.Equal(1205000, result, Epsilon);
        }

        [Theory]
        [ClassData(typeof(CommaDecimalPointCultureInfo))]
        public void TryParse_IntegerWithThousandPointForCommaCulture(string cultureName)
        {
            const string Input = "1.205.000";
            SetCurrentCulture(cultureName);

            var actual = DoubleHelper.TryParse(Input, out double result);
            Assert.True(actual);
            Assert.Equal(1205000, result, Epsilon);
        }

        [Theory]
        [ClassData(typeof(PeriodDecimalPointCultureInfo))]
        public void TryParse_FloatForPeriodCulture(string cultureName)
        {
            const string Input = "19.625";
            SetCurrentCulture(cultureName);

            var actual = DoubleHelper.TryParse(Input, out double result);
            Assert.True(actual);
            Assert.Equal(19.625, result, Epsilon);
        }

        [Theory]
        [ClassData(typeof(CommaDecimalPointCultureInfo))]
        public void TryParse_FloatForCommaCulture(string cultureName)
        {
            const string Input = "19.625";
            SetCurrentCulture(cultureName);

            var actual = DoubleHelper.TryParse(Input, out double result);
            Assert.True(actual);
            Assert.Equal(19625, result, Epsilon);
        }

        [Theory]
        [ClassData(typeof(PeriodDecimalPointCultureInfo))]
        public void TryParse_NegativeFloatForPeriodCulture(string cultureName)
        {
            const string Input = "-19.625";
            SetCurrentCulture(cultureName);

            var actual = DoubleHelper.TryParse(Input, out double result);
            Assert.True(actual);
            Assert.Equal(-19.625, result, Epsilon);
        }

        [Theory]
        [ClassData(typeof(CommaDecimalPointCultureInfo))]
        public void TryParse_NegativeFloatForCommaCulture(string cultureName)
        {
            const string Input = "-19.625";
            SetCurrentCulture(cultureName);

            var actual = DoubleHelper.TryParse(Input, out double result);
            Assert.True(actual);
            Assert.Equal(-19625, result, Epsilon);
        }

        [Theory]
        [ClassData(typeof(PeriodDecimalPointCultureInfo))]
        public void TryParse_FloatWithCommaPointForPeriodCulture(string cultureName)
        {
            const string Input = "10,2";
            SetCurrentCulture(cultureName);

            var actual = DoubleHelper.TryParse(Input, out double result);
            Assert.True(actual);
            Assert.Equal(102, result, Epsilon);
        }

        [Theory]
        [ClassData(typeof(CommaDecimalPointCultureInfo))]
        public void TryParse_FloatWithCommaPointForCommaCulture(string cultureName)
        {
            const string Input = "10,2";
            SetCurrentCulture(cultureName);

            var actual = DoubleHelper.TryParse(Input, out double result);
            Assert.True(actual);
            Assert.Equal(10.2, result, Epsilon);
        }

        [Theory]
        [ClassData(typeof(PeriodDecimalPointCultureInfo))]
        public void TryParse_CommaAndPeriodValueForPeriodCulture(string cultureName)
        {
            const string Input = "1,234.56";
            SetCurrentCulture(cultureName);

            var actual = DoubleHelper.TryParse(Input, out double result);
            Assert.True(actual);
            Assert.Equal(1234.56, result, Epsilon);
        }

        [Theory]
        [ClassData(typeof(CommaDecimalPointCultureInfo))]
        public void TryParse_CommaAndPeriodValueForCommaCulture(string cultureName)
        {
            const string Input = "1.234,56";
            SetCurrentCulture(cultureName);

            var actual = DoubleHelper.TryParse(Input, out double result);
            Assert.True(actual);
            Assert.Equal(1234.56, result, Epsilon);
        }

        #endregion

        #region Helpers

        private static void SetCurrentCulture(string cultureName)
        {
            var culture = CultureInfo.GetCultureInfo(cultureName);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        public class PeriodDecimalPointCultureInfo : TheoryData<string>
        {
            public PeriodDecimalPointCultureInfo()
            {
                base.Add("en-US"); // United States
                base.Add("ja-JP"); // Japan
                base.Add("en-GB"); // United Kingdom
                base.Add("zh-CN"); // China
                base.Add("zh-TW"); // Taiwan
                base.Add("ko-KR"); // Korea
            }
        }

        public class CommaDecimalPointCultureInfo : TheoryData<string>
        {
            public CommaDecimalPointCultureInfo()
            {
                base.Add("fr-FR"); // France
                base.Add("de-DE"); // Germany
                base.Add("it-IT"); // Italy
                base.Add("es-ES"); // Spain
                base.Add("ru-RU"); // Russia
                base.Add("nl-NL"); // Netherlands
                base.Add("sv-SE"); // Sweden
                base.Add("fi-FI"); // Finland
                base.Add("da-DK"); // Denmark
                base.Add("pt-PT"); // Portugal
                base.Add("tr-TR"); // Turkey
                base.Add("pl-PL"); // Poland
                base.Add("cs-CZ"); // Czech Republic
                base.Add("hu-HU"); // Hungary
                base.Add("el-GR"); // Greece
                base.Add("nb-NO"); // Norway (Bokmål)
                base.Add("sk-SK"); // Slovakia
                base.Add("sl-SI"); // Slovenia
                base.Add("hr-HR"); // Croatia
                base.Add("bg-BG"); // Bulgaria
                base.Add("ro-RO"); // Romania
                base.Add("lt-LT"); // Lithuania
                base.Add("lv-LV"); // Latvia
                base.Add("et-EE"); // Estonia
                base.Add("uk-UA"); // Ukraine
                base.Add("sr-Latn-RS"); // Serbia (Latin)
                base.Add("sr-Cyrl-RS"); // Serbia (Cyrillic)
                base.Add("ca-ES"); // Catalan (Spain)
                base.Add("eu-ES"); // Basque (Spain)
                base.Add("gl-ES"); // Galician (Spain)
            }
        }

        #endregion
    }
}