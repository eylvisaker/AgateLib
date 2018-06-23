using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace AgateLib.UserInterface.Styling.Themes
{
    public class PatternWidgetSelectorTests
    {
        [Fact]
        public void TransformPattern()
        {
            Matches("something", "*");
            Matches("anything", "*");

            Matches("inventory", "inventory");
            Matches("window.inventory", "inventory");

            Matches("inventory.item", "inventory.item");
            Matches("window.inventory.item", "inventory.item");

            Matches("window.inventory.item", "*.item");
            Matches("window.item", "*.item");

            Matches("inventory.item", "inventory..item");
            Matches("window.inventory.item", "inventory..item");
            Matches("window.inventory.menu.somethingelse.item", "inventory..item");

        }

        private void Matches(string stateId, string pattern, bool expectedResult = true)
        {
            var selector = new PatternWidgetSelector(pattern);

            selector.Matches(stateId).Should().Be(expectedResult,
                $"Expected {expectedResult} match with pattern {pattern} and state {stateId}");
        }
    }
}
