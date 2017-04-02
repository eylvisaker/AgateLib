using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Mathematics.Geometry.Algorithms.CollisionDetection
{
	/// <summary>
	/// Im
	/// </summary>
	/// <remarks>
	/// Implements the Gilbert-Johnson-Keerthi algorithm described
	/// here:
	/// http://www.dyn4j.org/2010/04/gjk-gilbert-johnson-keerthi/
	/// http://www.dtecta.com/papers/jgt98convex.pdf
	/// https://www.toptal.com/game/video-game-physics-part-ii-collision-detection-for-solid-objects
	/// </remarks>
	public class GilbertJohnsonKeerthiAlgorithm
	{
		private double Tolerance => IterationControl.Tolerance;
		private int MaxIterations => IterationControl.MaxIterations;
		public IterativeAlgorithm IterationControl { get; } = new IterativeAlgorithm();

		public bool AreColliding(Polygon a, Polygon b)
		{
			var minkowskiDiff = FindMinkowskiSimplex(a.First(),
				p => PolygonSupport(a, p),
				p => PolygonSupport(b, p));

			return minkowskiDiff.DistanceFromOrigin < IterationControl.Tolerance;
		}

		public double DistanceBetween(Polygon a, Polygon b)
		{
			var result = FindMinkowskiSimplex(a, b);

			return result.DistanceFromOrigin;
		}

		public MinkowskiSimplex FindMinkowskiSimplex(Polygon a, Polygon b)
		{
			var result = FindMinkowskiSimplex(a.First(),
				p => PolygonSupport(a, p),
				p => PolygonSupport(b, p));

			return result;
		}
		
		public MinkowskiSimplex FindMinkowskiSimplex(Vector2 start,
			Func<Vector2, Vector2> supportA, Func<Vector2, Vector2> supportB)
		{
			MinkowskiSimplex result = new MinkowskiSimplex();

			Vector2 d = start;
			Vector2 dperp = new Vector2(-d.Y, d.X);

			int iter = 0;
			double diff = double.MaxValue;

			result.Simplex.Add(Support(supportA, supportB, dperp));
			result.Simplex.Add(Support(supportA, supportB, d));
			result.Simplex.Add(Support(supportA, supportB, -d));

			d = LineSegmentPointNearestOrigin(result.Simplex[1], result.Simplex[2]);

			while (iter < MaxIterations && diff > Tolerance)
			{
				iter++;

				if (d == Vector2.Zero)
				{
					result.DistanceFromOrigin = d.Magnitude;
					return result;
				}

				d = -d;

				var c = Support(supportA, supportB, d);
				var dotc = c.DotProduct(d);
				var dota = result.Last().DotProduct(d);

				diff = dotc - dota;
				if (diff < Tolerance)
				{
					result.DistanceFromOrigin = d.Magnitude;
					return result;
				}

				var ia = result.Simplex.Count - 2;
				var ib = result.Simplex.Count - 1;

				var p1 = LineSegmentPointNearestOrigin(c, result.Simplex[ia]);
				var p2 = LineSegmentPointNearestOrigin(c, result.Simplex[ib]);
				
				if (p1.MagnitudeSquared < p2.MagnitudeSquared)
				{
					result.Simplex[0] = result.Simplex[ib];
					result.Simplex[ib] = c;
					d = p1;
				}
				else
				{
					result.Simplex[0] = result.Simplex[ia];
					result.Simplex[ia] = c;
					d = p2;
				}
			}

			return result;
		}

		public Polygon ApproximateMinkowskiDifference(Vector2 start,
			Func<Vector2, Vector2> supportA, Func<Vector2, Vector2> supportB)
		{
			Polygon result = new Polygon();

			Vector2 d = start;

			int iter = 0;
			double diff = double.MaxValue;

			result.Add(Support(supportA, supportB, d));
			result.Add(Support(supportA, supportB, -d));

			d = FindPointNearestOrigin(result);

			while (iter < MaxIterations && diff > Tolerance)
			{
				iter++;

				d = -d;
				if (d == Vector2.Zero)
					return result;

				var c = Support(supportA, supportB, d);
				var dotc = c.DotProduct(d);
				var dota = result.Last().DotProduct(d);

				diff = dotc - dota;

				result.Add(c);

				diff = (d - c).Magnitude;

				d = c;

				if (result.Count > 2)
				{
					result = new QuickHull().FindConvexHull(result);
				}
			}

			return result;
		}

		private static Vector2 Support(Func<Vector2, Vector2> supportA, Func<Vector2, Vector2> supportB, Vector2 v)
		{
			var sa = supportA(v);
			var sb = supportB(-v);

			var w = sa - sb;
			return w;
		}

		public static Vector2 PolygonSupport(Polygon polygon, Vector2 d)
		{
			double highest = double.MinValue;
			Vector2 support = Vector2.Zero;

			foreach (var point in polygon)
			{
				var dot = point.DotProduct(d);

				if (dot > highest)
				{
					highest = dot;
					support = point;
				}
			}

			return support;
		}
		
		private Vector2 LineSegmentPointNearestOrigin(Vector2 start, Vector2 end)
		{
			var delta = end - start;
			var perp = new Vector2(delta.Y, -delta.X);

			if (delta.MagnitudeSquared < Tolerance)
				return start;

			var intersection = LineAlgorithms.LineSegmentIntersection(
				start, end, Vector2.Zero, perp);

			if (intersection.IntersectionWithinFirstSegment)
				return intersection.IntersectionPoint;

			return start.MagnitudeSquared < end.MagnitudeSquared
				? start
				: end;
		}

		private Vector2 FindPointNearestOrigin(IEnumerable<Vector2> points)
		{
			Vector2 result = Vector2.Zero;
			double distance = double.MaxValue;
			bool any = false;

			foreach (var point in points)
			{
				any = true;
				var thisDist = point.MagnitudeSquared;

				if (thisDist < distance)
				{
					distance = thisDist;
					result = point;
				}
			}

			if (!any)
				throw new ArgumentException();

			return result;
		}

	}
}
