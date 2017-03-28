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
using AgateLib.UserInterface.StyleModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Layout
{
	public interface ILayoutBuilder
	{
		/// <summary>
		/// Calculates the box size of the widget, given the passed constraints.
		/// The box size includes with margin, padding and border.
		/// </summary>
		/// <param name="widget">The widget who's box size is to be computed</param>
		/// <param name="maxWidth">The maximum width of the widget's box</param>
		/// <param name="maxHeight">The maximum height of the widget's box</param>
		/// <returns></returns>
		void ComputeBoxSize(WidgetStyle widget, int? maxWidth = null, int? maxHeight = null);

		/// <summary>
		/// Calculates the size of the widget in the absence of any constraints.
		/// </summary>
		/// <param name="widget"></param>
		bool ComputeNaturalSize(WidgetStyle widget);

		/// <summary>
		/// Returns the style object of the specified widget.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		WidgetStyle StyleOf(Widget widget);
	}
}
