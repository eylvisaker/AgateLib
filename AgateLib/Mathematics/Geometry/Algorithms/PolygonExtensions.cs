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

using Microsoft.Xna.Framework;

namespace AgateLib.Mathematics.Geometry.Algorithms
{
	public static class PolygonExtensions
	{
		/// <summary>
		/// Returns the vertex of a polygon with wrap-around in the vertex index.
		/// </summary>
		/// <param name="polygon"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static Microsoft.Xna.Framework.Vector2 At(this Polygon polygon, int index)
		{
			while (index < 0)
				index += polygon.Count;

			index %= polygon.Count;

			return polygon[index];
		}

		/// <summary>
		/// Indicates if the vertices are in counter clockwise order.
		/// Warning: If the area of the polygon is 0, it is unable to determine the winding.
		/// </summary>
		public static bool IsCounterClockwise(this Polygon polygon)
		{
			if (polygon.Count < 3)
				return false;

			return (polygon.GetSignedArea() > 0.0f);
		}

		/// <summary>
		/// Gets the signed area, which is positive if the winding of the polygon is counterclockwise.
		/// </summary>
		public static double GetSignedArea(this Polygon polygon)
		{
			if (polygon.Count < 3)
				return 0;

			int i;
			double area = 0;

			for (i = 0; i < polygon.Count; i++)
			{
				int j = (i + 1) % polygon.Count;

				Microsoft.Xna.Framework.Vector2 vi = polygon[i];
				Microsoft.Xna.Framework.Vector2 vj = polygon[j];

				area += vi.X * vj.Y;
				area -= vi.Y * vj.X;
			}

			area /= 2.0;
			return area;
		}

		/// <summary>
		/// Copies a range of vertices from a polygon with wrap-around on the indices.
		/// </summary>
		/// <param name="polygon"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public static Polygon CopyRange(this Polygon polygon, int start, int end)
		{
			Polygon p = new Polygon();
			while (end < start)
				end += polygon.Count;

			for (; start <= end; ++start)
			{
				p.Add(polygon.At(start));
			}

			return p;
		}

	}
}
