using AgateLib.Display;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace AgateLib.Tests.Display
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
    }
}
