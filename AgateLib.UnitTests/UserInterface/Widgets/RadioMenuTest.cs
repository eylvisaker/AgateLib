using AgateLib.Tests.UserInterface.RadioButtons;
using FluentAssertions;
using Microsoft.Xna.Framework.Input;
using Xunit;

namespace AgateLib.UserInterface.Widgets
{
    public class RadioMenuTest
    {
        [Fact]
        public void RadioButtonTest()
        {
            string value = null;
            string selected = null;

            var app = new RadioButtonApp(new RadioButtonAppProps
            {
                OnValueSet = v => value = v,
                OnSelectionSet = v => selected = v,
                Items = { "Foo", "Bar", "Gra", "Hoh" }
            });

            UserInterfaceTestDriver driver = new UserInterfaceTestDriver(app);

            driver.Press(Buttons.DPadRight);
            driver.Press(Buttons.A);

            value.Should().Be("Bar");
            selected.Should().Be("Bar");

            driver.Press(Buttons.DPadRight);

            value.Should().Be("Bar");
            selected.Should().Be("Gra");
        }
    }
}
