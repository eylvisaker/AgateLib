using System;
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
			var minkowskiDiff = ApproximateMinkowskiDifference(
				a.First() - b.First(),
				pa => PolygonSupport(a, pa),
				pb => PolygonSupport(b, pb));

			return minkowskiDiff.AreaContains(Vector2.Zero);
		}

		public Polygon ApproximateMinkowskiDifference(Vector2 start,
			Func<Vector2, Vector2> supportA, Func<Vector2, Vector2> supportB)
		{
			Polygon result = new Polygon();

			Vector2 v = start;

			int iter = 0;
			double diff = double.MaxValue;

			while (iter < MaxIterations && diff > Tolerance)
			{
				iter++;
				var sa = supportA(v);
				var sb = supportB(-v);

				var w = sa - sb;

				result.Add(w);

				diff = (v - w).Magnitude;

				v = w;

				if (result.Count > 2)
				{
					result = new QuickHull().FindConvexHull(result);
				}
			}

			return result;
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

		private Vector2 FindPointNearOrigin(Polygon minkowskiDiff)
		{
			Vector2 result = minkowskiDiff.First();
			double distance = result.MagnitudeSquared;

			foreach (var point in minkowskiDiff)
			{
				var thisDist = point.MagnitudeSquared;

				if (thisDist < distance)
				{
					distance = thisDist;
					result = point;
				}
			}

			return result;
		}

	}
}
