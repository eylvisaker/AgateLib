using FluentAssertions;
using Xunit;

namespace AgateLib.UserInterface.Styling
{
    public class AnimationStyleUnitTests
    {
        [Fact]
        public void AnimationStyleInNameAndArgs()
        {
            var style = new AnimationStyle
            {
                Entry = "slide right"
            };

            style.EntryName.Should().Be("slide");
            style.EntryArgs.Should().BeEquivalentTo(new[] { "right" });
        }

        [Fact]
        public void AnimationStyleOutNameAndArgs()
        {
            var style = new AnimationStyle
            {
                Exit = "slide right 0.34"
            };

            style.ExitName.Should().Be("slide");
            style.ExitArgs.Should().BeEquivalentTo(new[] { "right", "0.34" });
        }

        [Fact]
        public void AnimationStyleStaticNameAndArgs()
        {
            var style = new AnimationStyle
            {
                Exit = "slide right 0.34 0.99"
            };

            style.ExitName.Should().Be("slide");
            style.ExitArgs.Should().BeEquivalentTo(new[] { "right", "0.34", "0.99" });
        }
    }
}
