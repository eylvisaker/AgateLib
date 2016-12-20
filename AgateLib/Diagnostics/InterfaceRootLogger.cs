using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.Diagnostics
{
	/// <summary>
	/// Provides extensions which can be used to log information about internal AgateLib structures.
	/// </summary>
	public static class InterfaceRootLogger
	{
		/// <summary>
		/// Outputs the structure of the widgets to the console.
		/// </summary>
		/// <param name="ui"></param>
		public static void LogWidgetStructure(this Gui ui)
		{
			Log.WriteLine($"Facet: {ui.FacetName}");

			LogUserInterfaceChildren(ui.Desktop);
		}

		private static void LogUserInterfaceChildren(Widget container)
		{
			Log.Indent();

			foreach (var child in container.LayoutChildren)
			{
				Log.WriteLine($"{child.GetType().Name}: {child.Name}");

				Log.Indent();
				Log.WriteLine($"ClientRect: {child.Parent.ClientToScreen(child.ClientRect)}");
				Log.WriteLine($"WidgetRect: {child.Parent.ClientToScreen(child.WidgetRect)}");
				
				if (child.LayoutChildren.Any())
				{
					Log.WriteLine("Children:");
					LogUserInterfaceChildren(child);
				}
				Log.Unindent();
			}

			Log.Unindent();
		}
	}
}
