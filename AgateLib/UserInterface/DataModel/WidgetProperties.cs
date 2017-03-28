//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
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
