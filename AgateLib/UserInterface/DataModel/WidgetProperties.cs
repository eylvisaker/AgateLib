//     The contents of this file are subject to the Mozilla Public License
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
using AgateLib.Geometry;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.DataModel
{
	public class WidgetProperties
	{
		public string Name { get; set; }
		public string Type { get; set; }

		public string Text { get; set; }

		public WidgetLayoutModel Layout { get; set; } = new WidgetLayoutModel();

		public WidgetChildCollection Children { get; private set; } = new WidgetChildCollection();

		public MenuItemModelCollection MenuItems { get; private set; } = new MenuItemModelCollection();

		public WidgetThemeModel Style { get; set; }

		public Overflow? Overflow { get; set; }

		/// <summary>
		/// Indicates the upper-left point of the widget's client area.
		/// If specified, the widget will not participate in the flow layout of controls. Instead it
		/// will be fixed at this position within its parent. 
		/// </summary><remarks>
		/// Specifying this value will prevent
		/// the UI system from touching the position of the widget, so supplying a value
		/// is a good for controls that will be programmatically moved.
		/// </remarks>
		public Point? Position { get; set; }

		/// <summary>
		/// Indicates the size of the client area for this widget. 
		/// If specified, the layout engine will not alter the size
		/// of the widget.
		/// </summary>
		public Size? Size { get; set; }


		public bool Enabled { get; set; } = true;

		public bool Visible { get; set; } = true;

		internal void Validate()
		{
			if (MenuItems.Count > 0 && !string.Equals(Type, "menu", StringComparison.OrdinalIgnoreCase))
				throw new AgateException($"Item {Name} has menu items but is type {Type} instead of type menu.");
		}
	}
}
