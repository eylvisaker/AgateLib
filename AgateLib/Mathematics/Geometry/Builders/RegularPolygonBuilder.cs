//
//    Copyright (c) 2006-2018 Erik Ylvisaker
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
using Microsoft.Xna.Framework;

namespace AgateLib.Mathematics.Geometry.Builders
{
	/// <summary>
	/// Builds a pointed star
	/// </summary>
	public class RegularPolygonBuilder
	{
		/// <summary>
		/// Builds a pointed star.
		/// </summary>
		/// <param name="pointCount">Number of points in the polygon.</param>
		/// <param name="size">The size of the polygon.</param>
		/// <param name="location"></param>
		/// <param name="angle">The rotation angle for the polygon.</param>
		/// <returns></returns>
		public Polygon BuildPolygon(int pointCount, float size, Vector2 location = new Vector2(), float angle = 0)
		{
			return RegularShape(angle, pointCount, size).Translate(location);
		}

		private static Polygon RegularShape(float angle, int pointCount, float size)
		{
			float step = 2 * (float)Math.PI / pointCount;

			Polygon result = new Polygon();

			for (int i = 0; i < pointCount; i++)
			{
				result.Add(Vector2X.FromPolar(size, angle));
				angle += step;
			}

			return result;
		}
	}
}
