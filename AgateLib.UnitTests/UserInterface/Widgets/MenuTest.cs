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

        [Fact]
        public void SkipDisabledMenuItems()
        {
            var menu = new Window(new WindowProps
            {
                Children =
                {
                    new Menu(new MenuProps
                    {
                        MenuItems =
                        {
                            new MenuItem(new MenuItemProps { Text = "Hello" }),
                            new MenuItem(new MenuItemProps { Text = "Some" }),
                            new MenuItem(new MenuItemProps { Text = "Items", Enabled = false, }),
                            new MenuItem(new MenuItemProps { Text = "Are" }),
                            new MenuItem(new MenuItemProps { Text = "Disabled", Enabled = false }),
                        }
                    }),
                    new Menu(new MenuProps
                    {
                        MenuItems =
                        {
                            new MenuItem(new MenuItemProps { Text = "Testing if we" }),
                            new MenuItem(new MenuItemProps { Text = "Can navigate out" }),
                            new MenuItem(new MenuItemProps { Text = "of a menu who's last" }),
                            new MenuItem(new MenuItemProps { Text = "item is disabled." }),
                        }
                    })
                }
            });

            TestUIDriver driver = new TestUIDriver(menu);

            driver.Press(Buttons.DPadDown);
            FocusItemHasText(driver, "Some");

            driver.Press(Buttons.DPadDown);
            FocusItemHasText(driver, "Are");

            driver.Press(Buttons.DPadDown);
            FocusItemHasText(driver, "Testing if we");
        }

        private static void FocusItemHasText(TestUIDriver driver, string text)
        {
            driver.Focus.Props.Should().BeOfType(typeof(MenuItemElementProps));
            (driver.Focus.Props as MenuItemElementProps).Text.Should().Be(text);
        }
    }
}
