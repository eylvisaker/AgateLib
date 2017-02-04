﻿//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Diagnostics
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