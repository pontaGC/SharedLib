using System.Drawing;

using FluentAssertions;
using SharedLib.Extensions;

namespace SharedLib.Test.Extensions
{
    public class ColorExtensionsTests
    {
        #region GetRGBCode

        [Fact]
        public void GetRGBCode_Aquamarine()
        {
            var sut = Color.FromArgb(127, 255, 212);

            var actual = sut.GetRGBCode();

            actual.Should().Be("#7FFFD4");
        }

        [Fact]
        public void GetRGBCode_Black()
        {
            var sut = Color.FromArgb(0, 0, 0);

            var actual = sut.GetRGBCode();

            actual.Should().Be("#000000");
        }

        [Fact]
        public void GetRGBCode_White()
        {
            var sut = Color.FromArgb(255, 255, 255);

            var actual = sut.GetRGBCode();

            actual.Should().Be("#FFFFFF");
        }

        [Fact]
        public void GetRGBCode_Red()
        {
            var sut = Color.FromArgb(255, 0, 0);

            var actual = sut.GetRGBCode();

            actual.Should().Be("#FF0000");
        }

        [Fact]
        public void GetRGBCode_Empty()
        {
            var sut = Color.Empty;

            var actual = sut.GetRGBCode();

            actual.Should().Be("#000000");
        }

        #endregion

        #region GetARGBCode

        [Fact]
        public void GetARGBCode_TranslucentAquamarine()
        {
            var sut = Color.FromArgb(128, 127, 255, 212);

            var actual = sut.GetARGBCode();

            actual.Should().Be("#807FFFD4");
        }

        [Fact]
        public void GetARGBCode_OpaqueBlack()
        {
            var sut = Color.FromArgb(255, 0, 0, 0);

            var actual = sut.GetARGBCode();

            actual.Should().Be("#FF000000");
        }

        [Fact]
        public void GetARGBCode_WhiteWithDefaultAlpha()
        {
            var sut = Color.FromArgb(255, 255, 255);

            var actual = sut.GetARGBCode();

            actual.Should().Be("#FFFFFFFF");
        }

        [Fact]
        public void GetARGBCode_Empty()
        {
            var sut = Color.Empty;

            var actual = sut.GetARGBCode();

            actual.Should().Be("#00000000");
        }

        #endregion
    }
}
