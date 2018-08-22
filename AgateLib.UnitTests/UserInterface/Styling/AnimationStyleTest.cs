using AgateLib.UserInterface;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AgateLib.UserInterface.Styling
{
    public class AnimationStyleTest
    {
        [Fact]
        public void AnimationStyleInNameAndArgs()
        {
            var style = new AnimationStyle();

            style.Entry = "slide right";

            style.EntryName.Should().Be("slide");
            style.EntryArgs.Should().BeEquivalentTo(new[] { "right" });
        }

        [Fact]
        public void AnimationStyleOutNameAndArgs()
        {
            var style = new AnimationStyle();

            style.Exit = "slide right 0.34";

            style.ExitName.Should().Be("slide");
            style.ExitArgs.Should().BeEquivalentTo(new[] { "right", "0.34" });
        }

        [Fact]
        public void AnimationStyleStaticNameAndArgs()
        {
            var style = new AnimationStyle();

            style.Exit = "slide right 0.34 0.99";

            style.ExitName.Should().Be("slide");
            style.ExitArgs.Should().BeEquivalentTo(new[] { "right", "0.34", "0.99" });
        }
    }
}
