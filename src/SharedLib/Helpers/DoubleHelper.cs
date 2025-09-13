using System.Globalization;

namespace SharedLib.Helpers
{
    /// <summary>
    /// The utility class for the double struct type.
    /// </summary>
    public static class DoubleHelper
    {
        private const NumberStyles DefaultNumberStyle = NumberStyles.Float | NumberStyles.AllowThousands;

        /// <summary>
        /// Try to convert the given string to double value.
        /// </summary>
        /// <param name="input">The string to covnert.</param>
        /// <param name="result">The double value after converting.</param>
        /// <returns>
        /// <c>true</c> if converting <paramref name="input"/> to the double value is successful,
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool TryParse(string input, out double result)
        {
            if (double.TryParse(input, DefaultNumberStyle, CultureInfo.CurrentCulture, out result))
            {
                return true;
            }

            var integerInput = SwapCommaToPeriod(input);
            if (double.TryParse(integerInput, CultureInfo.InvariantCulture, out result))
            {
                return true;
            }

            return false;
        }

        private static string SwapCommaToPeriod(string input)
        {
            return input.Replace(',', '#').Replace('.', ',').Replace('#', '.');
        }
    }
}
