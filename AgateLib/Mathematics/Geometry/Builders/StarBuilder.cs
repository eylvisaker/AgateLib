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
using AgateLib.Mathematics.Geometry.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Mathematics.Geometry.Builders
{
	/// <summary>
	/// Builds a pointed star
	/// </summary>
	public class StarBuilder
	{
		/// <summary>
		/// Builds a pointed star.
		/// </summary>
		/// <param name="pointCount">Number of points in the star.</param>
		/// <param name="size">The size of the star.</param>
		/// <param name="innerSize">The inner size of the star. It is recommended that this be between 0.2 and 0.7 times the value of size.</param>
		/// <param name="location"></param>
		/// <param name="angle">The rotation angle for the star.</param>
		/// <returns></returns>
		public Polygon BuildStar(int pointCount, double size, double innerSize, Vector2 location = new Vector2(), double angle = 0)
		{
			var outerpoints = RegularShape(angle, pointCount, size);
			var innerPoints = RegularShape(Math.PI / pointCount + angle, pointCount, innerSize);

			var result = new Polygon();

			for (int i = 0; i < outerpoints.Count; i++)
			{
				result.Add(outerpoints[i]);
				result.Add(innerPoints[i]);
			}

			return result.Translate(location);
		}

		private static Polygon RegularShape(double angle, int pointCount, double size)
		{
			double step = 2 * Math.PI / pointCount;

			Polygon result = new Polygon();

			for (int i = 0; i < pointCount; i++)
			{
				result.Add(Vector2.FromPolar(size, angle));
				angle += step;
			}

			return result;
		}
	}
}
