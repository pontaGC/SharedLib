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
                base.Add("en-US");
                base.Add("ja-JP");
                base.Add("en-GB");
                base.Add("zn-CN");
                base.Add("ko-KR");
            }
        }

        public class CommaDecimalPointCultureInfo : TheoryData<string>
        {
            public CommaDecimalPointCultureInfo()
            {
                base.Add("fr-FR");
                base.Add("de-DE");
                base.Add("it-IT");
                base.Add("es-ES");
                base.Add("ru-RU");
                base.Add("nl-NL");
                base.Add("sv-SE");
                base.Add("fi-FI");
                base.Add("da-DK");
                base.Add("pt-PT");
                base.Add("tr-TR");
                base.Add("pl-PL");
                base.Add("cs-CZ");
                base.Add("hu-HU");
                base.Add("el-GR");
                base.Add("nb-NO");
                base.Add("sk-SK");
                base.Add("sl-SI");
                base.Add("hr-HR");
                base.Add("bg-BG");
                base.Add("ro-RO");
                base.Add("lt-LT");
                base.Add("lv-LV");
                base.Add("et-EE");
                base.Add("uk-UA");
                base.Add("sr-Latn-RS");
                base.Add("sr-Cyrl-RS");
                base.Add("ca-ES");
                base.Add("eu-ES");
                base.Add("gl-ES");
            }
        }

        #endregion
    }
}