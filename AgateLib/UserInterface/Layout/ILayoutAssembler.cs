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

using System.Collections.Generic;
using AgateLib.UserInterface.StyleModel;

namespace AgateLib.UserInterface.Layout
{
	public interface ILayoutAssembler
	{
		/// <summary>
		/// Returns true if this layout assembler can do the layout for the specified widget.
		/// </summary>
		/// <param name="containerStyle">WidgetStyle object for the container.</param>
		/// <returns></returns>
		bool CanDoLayoutFor(WidgetStyle containerStyle);

		/// <summary>
		/// Performs layout fo the specified container and children.
		/// </summary>
		/// <param name="layoutBuilder">The layout builder that will be used to perform layout of child container contents.</param>
		/// <param name="container">The container this object is doing layout for.</param>
		/// <param name="layoutChildren">The container children that participate in layout.</param>
		/// <param name="maxWidth">The maximum width of the container client area.</param>
		/// <param name="maxHeight">The maximum height of the container client area.</param>
		void DoLayout(ILayoutBuilder layoutBuilder, WidgetStyle container, ICollection<WidgetStyle> layoutChildren,
			int? maxWidth = null, int? maxHeight = null);

		/// <summary>
		/// Computes the natural size of the widget and assigns it to the widget.Metrics.NaturalBoxSize property.
		/// Returns true if this value is different from the stored value.
		/// </summary>
		/// <param name="layoutBuilder">The layout builder that will be used to perform layout of child container contents.</param>
		/// <param name="widget">The container layout is being done on.</param>
		/// <returns></returns>
		bool ComputeNaturalSize(ILayoutBuilder layoutBuilder, WidgetStyle widget);
	}
}