using FluentAssertions;
using Xunit;

namespace AgateLib.UserInterface.Styling
{
    public class SizeConstraintsUnitTests
    {
        [Fact]
        public void SettingWidthSetsMinAndMaxWidth()
        {
            SizeConstraints sc = new SizeConstraints
            {
                Width = 888
            };

            sc.MinWidth.Should().Be(888);
            sc.MaxWidth.Should().Be(888);
        }

        [Fact]
        public void SettingHeightSetsMinAndMaxHeight()
        {
            SizeConstraints sc = new SizeConstraints
            {
                Height = 888
            };

            sc.MinHeight.Should().Be(888);
            sc.MaxHeight.Should().Be(888);
        }
    }
}
