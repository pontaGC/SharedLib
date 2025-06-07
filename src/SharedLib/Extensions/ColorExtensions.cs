using System.Diagnostics;
using System.Drawing;

namespace SharedLib.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Color" />
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Gets a RGB color code for the given color.
        /// </summary>
        /// <param name="color">The source color.</param>
        /// <returns>The RGB color code (e.g. "#1E90FF").</returns>
        [DebuggerStepThrough]
        public static string GetRGBCode(this Color color)
        {
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        /// <summary>
        /// Gets a ARGB color code with alpha value for the given color.
        /// </summary>
        /// <param name="color">The source color.</param>
        /// <returns>The ARGB code (e.g. "#FF1E90FF").</returns>
        [DebuggerStepThrough]
        public static string GetARGBCode(this Color color)
        {
            return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }
    }
}
