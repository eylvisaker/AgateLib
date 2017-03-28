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

using AgateLib.Mathematics.Geometry;

namespace AgateLib.UserInterface.StyleModel
{
	public class WidgetMetrics
	{
		/// <summary>
		/// The minimum dimensions of the widget, including borders, padding and margins.
		/// </summary>
		public Size MinTotalSize { get; set; }
		/// <summary>
		/// The maximum dimensions of the widget, including borders, padding and margins.
		/// </summary>
		public Size MaxTotalSize { get; set; }

		/// <summary>
		/// The actual size of the content area.
		/// </summary>
		public Size ContentSize { get; set; }
		
		/// <summary>
		/// The size of the control, including margins, borders and padding.
		/// </summary>
		public Size BoxSize { get; set; }

		/// <summary>
		/// The size of the control, including margins, borders and padding in 
		/// the absence of any constraints.
		/// </summary>
		public Size NaturalBoxSize { get; set; }
	}
}