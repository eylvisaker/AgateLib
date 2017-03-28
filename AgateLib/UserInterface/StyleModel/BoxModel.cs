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
