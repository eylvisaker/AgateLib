using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets;
using AgateLib.UserInterface.Layout;

namespace AgateLib.FunctionalTests.UserInterface.Support.Initialization
{
    public static class IninitializerExtensions
    {
        public static void AddMenuItem(this IWidgetLayout layout, 
            string text, Action method)
        {
            var item = new ContentMenuItem { Text = text, Name = text };
            item.PressAccept += (sender, e) => method();

            layout.Add(item);
        }
    }
}
