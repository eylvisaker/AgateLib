using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Layout;
using FluentAssertions;
using Xunit;
using Moq;

namespace AgateLib.UserInterface.Widgets
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
                Children = { new MenuItem(new MenuItemProps { Text = "Hello" }) }
            });

            var element = menu.Finalize();

            element.OnInputEvent(InputEventArgs.ButtonDown(MenuInputButton.Cancel));
            element.OnInputEvent(InputEventArgs.ButtonUp(MenuInputButton.Cancel));

            canceled.Should().BeTrue("Menu did not exit when cancel was pressed.");
        }


        [Fact]
        public void MenuInputCancelShouldNotHappenIfCancelDownWasNotSent()
        {
            bool canceled = false;

            var menu = new Menu(new MenuProps
            {
                Cancel = () => canceled = true,
                Children = { new MenuItem(new MenuItemProps { Text = "Hello" }) }
            });

            var element = menu.Finalize();

            element.OnInputEvent(InputEventArgs.ButtonUp(MenuInputButton.Cancel));

            canceled.Should().BeFalse("Cancel should not have been called.");
        }
    }
}
