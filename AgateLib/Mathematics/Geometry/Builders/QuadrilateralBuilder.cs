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

namespace AgateLib.Mathematics.Geometry.Builders
{
	/// <summary>
	/// Contains methods used to build lists of points that represent
	/// rectangles, squares and other quadrilaterals.
	/// </summary>
	public class QuadrilateralBuilder
	{
		/// <summary>
		/// Returns a list of points that can be used to trace the perimeter
		/// of a rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public IEnumerable<Vector2> BuildRectangle(Rectangle rect)
		{
			return BuildRectangle((RectangleF)rect);
		}

		/// <summary>
		/// Returns a list of points that can be used to trace the perimeter
		/// of a rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public IEnumerable<Vector2> BuildRectangle(RectangleF rect)
		{
			return new[]
			{
				new Vector2(rect.Left, rect.Top),
				new Vector2(rect.Right, rect.Top),
				new Vector2(rect.Right, rect.Bottom),
				new Vector2(rect.Left, rect.Bottom)
			};
		}
	}
}
