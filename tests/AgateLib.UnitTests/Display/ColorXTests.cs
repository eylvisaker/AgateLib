using AgateLib.Display;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace AgateLib.Demo.Display
{
    public class ColorXTests
    {
        [Fact]
        public void ToArgb()
        {
            var clr = new Color(0x12, 0x34, 0x56, 0x78);

            clr.ToArgb().Should().Be("78123456");
        }

        [Fact]
        public void ToArgbZeroGreen()
        {
            var clr = new Color(0x88, 0x00, 0x77, 0x66);

            clr.ToArgb().Should().Be("66880077");
        }

        [Fact]
        public void TryParseRgb()
        {
            bool result = ColorX.TryParse("123456", out Color clr);

            result.Should().BeTrue();

            clr.A.Should().Be(0xff);
            clr.R.Should().Be(0x12);
            clr.G.Should().Be(0x34);
            clr.B.Should().Be(0x56);
        }

        [Fact]
        public void TryParseArgb()
        {
            bool result = ColorX.TryParse("44123456", out Color clr);

            result.Should().BeTrue();

            clr.A.Should().Be(0x44);
            clr.R.Should().Be(0x12);
            clr.G.Should().Be(0x34);
            clr.B.Should().Be(0x56);
        }

        [Fact]
        public void FromRgb()
        {
            Color clr = ColorX.FromArgb("123456");

            clr.A.Should().Be(0xff);
            clr.R.Should().Be(0x12);
            clr.G.Should().Be(0x34);
            clr.B.Should().Be(0x56);
        }

        [Fact]
        public void FromArgb()
        {
            Color clr = ColorX.FromArgb("44123456");

            clr.A.Should().Be(0x44);
            clr.R.Should().Be(0x12);
            clr.G.Should().Be(0x34);
            clr.B.Should().Be(0x56);
        }

        [Theory]
        [InlineData(ColorSpace.Yuv, -0.1, 0, 0, 0)]
        [InlineData(ColorSpace.Yuv, 1.1, 255, 255, 255)]
        [InlineData(ColorSpace.Yuv, 0.5, 127, 127, 127)]
        [InlineData(ColorSpace.Rgb, -0.1, 0, 0, 0)]
        [InlineData(ColorSpace.Rgb, 1.1, 255, 255, 255)]
        [InlineData(ColorSpace.Rgb, 0.5, 127, 127, 127)]
        public void InterpolateBlackWhite(ColorSpace space,
                                          double step,
                                          byte expectedR,
                                          byte expectedG,
                                          byte expectedB)
        {
            Color clr = ColorX.Interpolate(Color.Black, Color.White, step, space);

            clr.R.Should().Be(expectedR);
            clr.G.Should().Be(expectedG);
            clr.B.Should().Be(expectedB);
        }

        [Theory]
        [InlineData(ColorSpace.Yuv, 0.5, 127, 64, 0)]
        [InlineData(ColorSpace.Rgb, 0.5, 127, 64, 0)]
        public void InterpolateRedGreen(ColorSpace space,
                                        double step,
                                        byte expectedR,
                                        byte expectedG,
                                        byte expectedB)
        {
            Color clr = ColorX.Interpolate(Color.Red, Color.Green, step, space);

            clr.R.Should().Be(expectedR);
            clr.G.Should().Be(expectedG);
            clr.B.Should().Be(expectedB);
        }

        [Theory]
        [InlineData(ColorSpace.Yuv, 0.5, 127, 127, 127)]
        [InlineData(ColorSpace.Rgb, 0.5, 127, 127, 127)]
        public void InterpolateRedCyan(ColorSpace space,
                                        double step,
                                        byte expectedR,
                                        byte expectedG,
                                        byte expectedB)
        {
            Color clr = ColorX.Interpolate(Color.Red, Color.Cyan, step, space);

            clr.R.Should().Be(expectedR);
            clr.G.Should().Be(expectedG);
            clr.B.Should().Be(expectedB);
        }
    }
}
