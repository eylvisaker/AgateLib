using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;

namespace AgateLib.Mathematics.Geometry.Algorithms
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
			get { return tolerance; }
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
			Vector2 left = Vector2.UnitX * double.MaxValue;
			Vector2 right = Vector2.UnitX * double.MinValue;

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

		private void FindHull(Polygon result, List<Vector2> set, Vector2 P, Vector2 Q)
		{
			if (set.Count == 0)
				return;

			double max = double.MinValue;
			Vector2 C = set.First();

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
