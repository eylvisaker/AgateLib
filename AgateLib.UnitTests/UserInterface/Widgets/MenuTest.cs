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
            var layout = new Mock<IWidgetLayout>();
            var menu = new Menu();

            bool exitCalled = false;

            menu.Layout = layout.Object;

            menu.Exit += () => exitCalled = true;

            menu.ProcessEvent(WidgetEventArgs.ButtonDown(MenuInputButton.Cancel));
            menu.ProcessEvent(WidgetEventArgs.ButtonUp(MenuInputButton.Cancel));

            exitCalled.Should().BeTrue("Menu did not exit when cancel was pressed.");
        }

        [Fact]
        public void MenuSwitchLayoutTypesPreservesItems()
        {
            var menu = new Menu { LayoutType = LayoutType.SingleColumn };

            menu.Add("Item 1", () => { });
            menu.Add("Item 2", () => { });
            menu.Add("Item 3", () => { });

            var item1 = menu.Layout.Items.First();
            var item3 = menu.Layout.Items.Last();

            menu.Layout.Should().BeAssignableTo<SingleColumnLayout>();

            menu.LayoutType = LayoutType.SingleRow;

            menu.Layout.Should().BeAssignableTo<SingleRowLayout>();

            menu.Layout.Items.First().Should().BeSameAs(item1);
            menu.Layout.Items.Last().Should().BeSameAs(item3);
        }

        [Fact]
        public void MenuInputExitShouldNotHappenIfCancelDownWasNotSent()
        {
            var layout = new Mock<IWidgetLayout>();
            var menu = new Menu();

            bool exitCalled = false;

            menu.Layout = layout.Object;

            menu.Exit += () => exitCalled = true;

            menu.ProcessEvent(WidgetEventArgs.ButtonUp(MenuInputButton.Cancel));

            exitCalled.Should().BeFalse("Exit should not have been called.");
        }
    }
}
