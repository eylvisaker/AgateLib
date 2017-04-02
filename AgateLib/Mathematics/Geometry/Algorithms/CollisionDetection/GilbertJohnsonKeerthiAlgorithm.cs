using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;

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
		private int maxIterations = 50;
		private double tolerance = 1e-6;

		public int MaxIterations
		{
			get { return maxIterations; }
			set
			{
				Require.ArgumentInRange(value > 1, nameof(MaxIterations), "Value must be greater than 1.");
				maxIterations = value;
			}
		}

		public double Tolerance
		{
			get { return tolerance; }
			set
			{
				Require.ArgumentInRange(value > 0, nameof(Tolerance), "Value must be positive.");
				tolerance = value;
			}
		}


		public bool AreColliding(Polygon a, Polygon b)
		{
			var minkowskiDiff = FindMinkowskiSimplex(a.First(),
				p => PolygonSupport(a, p),
				p => PolygonSupport(b, p));

			return minkowskiDiff.DistanceFromOrigin < tolerance;
		}

		public double DistanceBetween(Polygon a, Polygon b)
		{
			var result = FindMinkowskiSimplex(a.First(),
				p => PolygonSupport(a, p),
				p => PolygonSupport(b, p));

			return result.DistanceFromOrigin;
		}

		public class MinkowskiSimplex : IEnumerable<Vector2>
		{
			public Vector2 Start;
			public Vector2 End;
			public double DistanceFromOrigin;

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public IEnumerator<Vector2> GetEnumerator()
			{
				yield return Start;
				yield return End;
			}
		}

		public MinkowskiSimplex FindMinkowskiSimplex(Vector2 start,
			Func<Vector2, Vector2> supportA, Func<Vector2, Vector2> supportB)
		{
			MinkowskiSimplex result = new MinkowskiSimplex();

			Vector2 d = start;

			int iter = 0;
			double diff = double.MaxValue;

			result.Start = Support(supportA, supportB, d);
			result.End = Support(supportA, supportB, -d);

			d = LineSegmentPointNearestOrigin(result.Start, result.End);

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
				if (diff < tolerance)
				{
					result.DistanceFromOrigin = d.Magnitude;
					return result;
				}

				var p1 = LineSegmentPointNearestOrigin(c, result.Start);
				var p2 = LineSegmentPointNearestOrigin(c, result.End);

				if (p1.MagnitudeSquared < p2.MagnitudeSquared)
				{
					result.End = c;
					d = p1;
				}
				else
				{
					result.Start = c;
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

		private Vector2 PolygonSupport(Polygon polygon, Vector2 d)
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

		private Vector2 RectangleSupport(Rectangle r, Vector2 d)
		{
			return PolygonSupport(r.ToPolygon(), d);
		}

		private Vector2 LineSegmentPointNearestOrigin(Vector2 start, Vector2 end)
		{
			var delta = end - start;
			var perp = new Vector2(delta.Y, -delta.X);

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
