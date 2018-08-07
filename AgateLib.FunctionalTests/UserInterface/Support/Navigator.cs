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
            this.context = context;
            this.instructor = instructor;
        }

        public Desktop Desktop => context.Scene.Desktop;

        public void GoTo(string menuItemText)
        {
            var menuItem = Desktop.ActiveWorkspace.Focus as MenuItemElement;
            var parent = menuItem.Parent as FlexBox;
            
            var target = parent.Children.OfType<MenuItemElement>().SingleOrDefault(
                w => (w.Id?.Equals(menuItemText, StringComparison.OrdinalIgnoreCase) ?? false)
                  || (w.Props.Text?.Equals(menuItemText, StringComparison.OrdinalIgnoreCase) ?? false));

            if (target == null)
                throw new InvalidOperationException($"Could not find {menuItemText}. Active workspace: {Desktop.ActiveWorkspace}");

            var targetIndex = parent.Children.IndexOf(target);

            if (targetIndex < 0)
                throw new InvalidOperationException($"Target index indicates menu item was not found in menu.");

            while (targetIndex > parent.FocusIndex)
            {
                parent.MoveNext();
            }
            while (targetIndex < parent.FocusIndex)
            {
                parent.MovePrevious();
            }
        }
    }
}
