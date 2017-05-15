using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry.Algorithms.Configuration;

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

		public int Iterations { get; private set; }
		public bool Converged { get; private set; }

		public bool AreColliding(Polygon a, Polygon b)
		{
			var minkowskiDiff = FindMinkowskiSimplex(a.First(),
				p => PolygonSupport(a, p),
				p => PolygonSupport(b, p));

			return minkowskiDiff.ContainsOrigin;
		}

		public double DistanceBetween(Polygon a, Polygon b)
		{
			var result = FindMinkowskiSimplex(a, b);

			return result.ContainsOrigin ? 0 : result.DistanceFromOrigin;
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

			double diff = double.MaxValue;

			result.Add(MinkowskiSimplex.Support(supportA, supportB, dperp));
			result.Add(MinkowskiSimplex.Support(supportA, supportB, d));
			result.Add(MinkowskiSimplex.Support(supportA, supportB, -d));

			d = LineSegmentPointNearestOrigin(result.Simplex[1], result.Simplex[2]);

			Iterations = 0;
			Converged = false;
			while (Iterations < MaxIterations && diff > Tolerance)
			{
				Iterations++;

				if (result.ContainsOrigin)
				{
					Converged = true;
					result.DistanceFromOrigin = d.Magnitude;
					return result;
				}

				d = -d;

				var c = MinkowskiSimplex.Support(supportA, supportB, d);
				var dotc = c.Difference.DotProduct(d);
				var dota = result.Last().DotProduct(d);

				diff = dotc - dota;
				if (diff < Tolerance)
				{
					Converged = true;
					result.DistanceFromOrigin = d.Magnitude;
					return result;
				}

				var ia = result.Simplex.Count - 2;
				var ib = result.Simplex.Count - 1;

				var p1 = LineSegmentPointNearestOrigin(c.Difference, result.Simplex[ia]);
				var p2 = LineSegmentPointNearestOrigin(c.Difference, result.Simplex[ib]);
				
				if (p1.MagnitudeSquared < p2.MagnitudeSquared)
				{
					result.StaggerInsert(ib, c);
					d = p1;
				}
				else
				{
					result.StaggerInsert(ia, c);
					d = p2;
				}
			}

			result.DistanceFromOrigin = d.Magnitude;
			return result;
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

			if (intersection.WithinFirstSegment)
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


	internal class SupportData
	{
		public Vector2 SupportA { get; set; }

		public Vector2 SupportB { get; set; }

		public Vector2 Difference => SupportA - SupportB;
	}

}
