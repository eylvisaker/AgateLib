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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.DataModel;

namespace AgateLib.UserInterface.StyleModel
{
	public class BoxModel
	{
		public LayoutBox Margin { get; set; }
		public LayoutBox Padding { get; set; }
		public LayoutBox Border { get; set; }

		public int Top { get { return Margin.Top + Padding.Top + Border.Top; } }
		public int Left { get { return Margin.Left + Padding.Left + Border.Left; } }
		public int Right { get { return Margin.Right + Padding.Right + Border.Right; } }
		public int Bottom { get { return Margin.Bottom + Padding.Bottom + Border.Bottom; } }

		/// <summary>
		/// The total amount the box model adds to the box width.
		/// </summary>
		public int Width { get { return Left + Right; } }
		/// <summary>
		/// The total amount the box model adds to the box height.
		/// </summary>
		public int Height { get { return Top + Bottom; } }

		/// <summary>
		/// Total amount the box model adds to the widget width.
		/// </summary>
		public int WidgetWidth { get { return Padding.Left + Padding.Right + Border.Left + Border.Right; } }
		/// <summary>
		/// Total amount the box model adds to the widget height.
		/// </summary>
		public int WidgetHeight { get { return Padding.Top + Padding.Bottom + Border.Top + Border.Bottom; } }

		public void Clear()
		{
			Margin = new LayoutBox();
			Padding = new LayoutBox();
			Border = new LayoutBox();
		}
	}
}
