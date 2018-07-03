using AgateLib.Display;
using FluentAssertions;
using System;
using Xunit;

namespace AgateLib.Tests.MathematicsTests.Geometry
{
    public class ColorTests
    {
        [Fact]
        public void HsvRed()
        {
            var clr1 = ColorX.FromHsv(0, 1, 1);
            var clr2 = ColorX.FromHsv(360, 1, 1);

            clr1.A.Should().Be(0xff);
            clr1.R.Should().Be(0xff);
            clr1.G.Should().Be(0);
            clr1.B.Should().Be(0);

            clr1.Should().Be(clr2);
        }

        [Fact]
        public void HsvYellow()
        {
            var clr1 = ColorX.FromHsv(60, 1, 1);
            var clr2 = ColorX.FromHsv(420, 1, 1);

            clr1.A.Should().Be(0xff);
            clr1.R.Should().Be(0xff);
            clr1.G.Should().Be(0xff);
            clr1.B.Should().Be(0);

            clr1.Should().Be(clr2);
        }

        [Fact]
        public void HsvGreen()
        {
            var clr1 = ColorX.FromHsv(120, 1, 1);
            var clr2 = ColorX.FromHsv(-240, 1, 1);

            clr1.A.Should().Be(0xff);
            clr1.R.Should().Be(0);
            clr1.G.Should().Be(0xff);
            clr1.B.Should().Be(0);

            clr1.Should().Be(clr2);
        }

        [Fact]
        public void HsvBlue()
        {
            var clr1 = ColorX.FromHsv(240, 1, 1);

            clr1.A.Should().Be(0xff);
            clr1.R.Should().Be(0);
            clr1.G.Should().Be(0);
            clr1.B.Should().Be(0xff);
        }

        [Fact]
        public void HsvCyan()
        {
            var clr1 = ColorX.FromHsv(200, 1, 1);

            clr1.A.Should().Be(0xff);
            clr1.R.Should().Be(0);
            clr1.G.Should().Be(0xa9);
            clr1.B.Should().Be(0xff);
        }

        [Fact]
        public void HsvMagenta()
        {
            var clr1 = ColorX.FromHsv(330, 1, 1);

            clr1.A.Should().Be(0xff);
            clr1.R.Should().Be(0xff);
            clr1.G.Should().Be(0);
            clr1.B.Should().Be(0x7f);
        }

        [Fact]
        public void FromArgbString()
        {
            Action badParse = () => ColorX.FromArgb("lkdsjflkdsj");
            badParse.Should().Throw<ArgumentException>();

            var clr = ColorX.FromArgb("0xaabbcc");

            clr.A.Should().Be(0xff);
            clr.R.Should().Be(0xaa);
            clr.G.Should().Be(0xbb);
            clr.B.Should().Be(0xcc);

            var clr1 = ColorX.FromArgb("0x99aabbcc");

            clr1.A.Should().Be(0x99);
            clr1.R.Should().Be(0xaa);
            clr1.G.Should().Be(0xbb);
            clr1.B.Should().Be(0xcc);
        }

        [Fact]
        public void ToAbgrString()
        {
            string[] colorsToTest = new[]
            {
                "ffaabbcc", "20f8f8f8", "ffaabbcc",
                "ffffffff", "01000000", "01020304"
            };

            foreach (var color in colorsToTest)
            {
                var text = ColorX.FromAbgr(color).PackedValue.ToString("x8");
                text.ToLowerInvariant().Should().Be(color, "Failed to convert color {0} correctly.", color);
            }

        }
    }

}
