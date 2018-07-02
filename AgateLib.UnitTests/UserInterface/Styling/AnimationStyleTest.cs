using AgateLib.UserInterface.Widgets;
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

            style.In = "slide right";

            style.InName.Should().Be("slide");
            style.InArgs.Should().BeEquivalentTo(new[] { "right" });
        }

        [Fact]
        public void AnimationStyleOutNameAndArgs()
        {
            var style = new AnimationStyle();

            style.Out = "slide right 0.34";

            style.OutName.Should().Be("slide");
            style.OutArgs.Should().BeEquivalentTo(new[] { "right", "0.34" });
        }

        [Fact]
        public void AnimationStyleStaticNameAndArgs()
        {
            var style = new AnimationStyle();

            style.Out = "slide right 0.34 0.99";

            style.OutName.Should().Be("slide");
            style.OutArgs.Should().BeEquivalentTo(new[] { "right", "0.34", "0.99" });
        }
    }
}
