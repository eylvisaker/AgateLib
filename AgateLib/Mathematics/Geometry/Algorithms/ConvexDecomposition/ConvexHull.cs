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

using System.Collections.Generic;
using System.Linq;
using AgateLib.Quality;

namespace AgateLib.Mathematics.Geometry.Algorithms.ConvexDecomposition
{
	/// <summary>
	/// An algorithm for finding the convex hull of a polygon.
	/// </summary>
	/// <remarks>
	/// QuickHull algorithm is implemented as described here:
	/// https://en.wikipedia.org/wiki/Quickhull
	/// </remarks>
	public class QuickHull
	{
		private double tolerance = 1e-6;

		public double Tolerance
		{
			get => tolerance;
			set
			{
				Require.ArgumentInRange(value > 0, nameof(Tolerance),
					"Value must be positive");

				tolerance = value;
			}
		}

		public Polygon FindConvexHull(Polygon polygon)
		{
			Polygon result = new Polygon();

			// Find left & right most points and add them to the hull.
			Microsoft.Xna.Framework.Vector2 left = Microsoft.Xna.Framework.Vector2.UnitX * float.MaxValue;
			Microsoft.Xna.Framework.Vector2 right = Microsoft.Xna.Framework.Vector2.UnitX * float.MinValue;

			foreach (var point in polygon)
			{
				if (point.X < left.X)
					left = point;
				if (point.X > right.X)
					right = point;
			}

			result.Add(left);
			result.Add(right);

			var leftSet = polygon.Where(v => LineAlgorithms.SideOf(left, right, v) < 0).ToList();
			var rightSet = polygon.Where(v => LineAlgorithms.SideOf(left, right, v) > 0).ToList();

			FindHull(result, rightSet, left, right);
			FindHull(result, leftSet, right, left);

			return result;
		}

		private void FindHull(Polygon result, List<Microsoft.Xna.Framework.Vector2> set, Microsoft.Xna.Framework.Vector2 P, Microsoft.Xna.Framework.Vector2 Q)
		{
			if (set.Count == 0)
				return;

			double max = double.MinValue;
			Microsoft.Xna.Framework.Vector2 C = set.First();

			foreach (var v in set)
			{
				var distance = LineAlgorithms.SideOf(P, Q, v);

				if (distance > max)
				{
					max = distance;
					C = v;
				}
			}

			var indexA = result.IndexOf(P);
			var indexB = result.IndexOf(Q);

			if (indexB > indexA)
			{
				result.Insert(indexB, C);
			}
			else
			{
				result.Add(C);
			}

			var S1 = set.FindAll(v => LineAlgorithms.SideOf(P, C, v) > tolerance);
			var S2 = set.FindAll(v => LineAlgorithms.SideOf(C, Q, v) > tolerance);

			FindHull(result, S1, P, C);
			FindHull(result, S2, C, Q);
		}
	}
}
