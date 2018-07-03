using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;
using AgateLib.UserInterface.Layout;
using AgateLib.Tests;

namespace AgateLib.Tests.UserInterface.Support
{
    public class Navigator
    {
        private readonly UIContext context;
        private readonly Instructor instructor;

        private Dictionary<Type, Action<string>> gotoMethods;

        public Navigator(UIContext context, Instructor instructor)
        {
            //gotoMethods = new Dictionary<Type, Action<string>>
            //{
            //    { typeof(SingleColumnLayout), SingleColumnGoTo },
            //    { typeof(SingleRowLayout), SingleRowGoTo }
            //};
            this.context = context;
            this.instructor = instructor;
        }

        public Desktop Desktop => context.Desktop;

        public void GoTo(string menuItem)
        {
            Desktop.ClearAnimations();

            var menu = context.Desktop.ActiveWorkspace.Focus as MenuElement;

            SingleColumnGoTo(menuItem);

            //if (!gotoMethods.ContainsKey(menu.Layout.GetType()))
            //    throw new InvalidOperationException($"Menu {menu.Name} has unsupported layout type: {menu.Layout.GetType().Name}");

            //var method = gotoMethods[menu.Layout.GetType()];

            //method(menuItem);
        }

        private void SingleColumnGoTo(string menuItem)
        {
            NavigateListLayout(menuItem, MenuInputButton.Down, MenuInputButton.Up);
        }

        private void SingleRowGoTo(string menuItem)
        {
            //NavigateListLayout(menuItem, MenuInputButton.Right, MenuInputButton.Left);
        }

        private void NavigateListLayout(string menuItem, MenuInputButton nextButton, MenuInputButton prevButton)
        {
            var menu = context.Desktop.ActiveWorkspace.Focus as MenuElement;

            var target = menu.MenuItems.SingleOrDefault(
                w => w.Props.Text.Equals(menuItem, StringComparison.OrdinalIgnoreCase));

            if (target == null)
                throw new InvalidOperationException($"Could not find {menuItem} in {menu.Name}. Active workspace: {context.Desktop.ActiveWorkspace}");

            var targetIndex = menu.MenuItems.ToList().IndexOf(target);

            if (targetIndex < 0)
                throw new InvalidOperationException($"Target index indicates menu item was not found in menu.");

            while (targetIndex > menu.SelectedIndex)
            {
                Desktop.ClearAnimations();

                instructor.SendButtonPress(nextButton);
            }
            while (targetIndex < menu.SelectedIndex)
            {
                Desktop.ClearAnimations();

                instructor.SendButtonPress(prevButton);
            }
        }
    }
}
