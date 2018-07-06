using AgateLib.Tests.UserInterface.RadioButtons;
using AgateLib.UserInterface;
using FluentAssertions;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AgateLib.Tests.UserInterface.Widgets
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

            TestUIDriver driver = new TestUIDriver(app);

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
