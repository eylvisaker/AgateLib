using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets;
using FluentAssertions;
using Microsoft.Xna.Framework.Input;
using Xunit;

namespace AgateLib.Tests.UserInterface.Widgets
{
    public class MenuTest
    {
        [Fact]
        public void MenuInputCancel()
        {
            bool canceled = false;

            var menu = new Menu(new MenuProps
            {
                Cancel = () => canceled = true,
                MenuItems = { new MenuItem(new MenuItemProps { Text = "Hello" }) }
            });

            TestUIDriver driver = new TestUIDriver(menu);

            driver.Press(Buttons.B);

            canceled.Should().BeTrue("Menu did not exit when cancel was pressed.");
        }
    }
}
