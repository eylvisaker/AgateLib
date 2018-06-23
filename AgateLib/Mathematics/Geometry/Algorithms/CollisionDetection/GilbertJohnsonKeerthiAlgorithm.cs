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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry.Algorithms.Configuration;
using Microsoft.Xna.Framework;

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
		MinkowskiSimplex simplex = new MinkowskiSimplex();

		public GilbertJohnsonKeerthiAlgorithm(IterativeAlgorithm iterationControl = null)
		{
			IterationControl = iterationControl ?? new IterativeAlgorithm();
		}

		public void Initialize()
		{
			simplex.Initialize();
		}

		private double Tolerance => IterationControl.Tolerance;
		private int MaxIterations => IterationControl.MaxIterations;
		public IterativeAlgorithm IterationControl { get; }

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
			FindMinkowskiSimplex(a.First(),
				p => PolygonSupport(a, p),
				p => PolygonSupport(b, p));

			return simplex;
		}

		public MinkowskiSimplex FindMinkowskiSimplex(Microsoft.Xna.Framework.Vector2 start,
			Func<Microsoft.Xna.Framework.Vector2, Microsoft.Xna.Framework.Vector2> supportA, Func<Microsoft.Xna.Framework.Vector2, Microsoft.Xna.Framework.Vector2> supportB)
		{
			simplex.Initialize();

			Microsoft.Xna.Framework.Vector2 d = start;
			Microsoft.Xna.Framework.Vector2 dperp = new Microsoft.Xna.Framework.Vector2(-d.Y, d.X);

			double diff = double.MaxValue;

			simplex.Add(MinkowskiSimplex.Support(supportA, supportB, dperp));
			simplex.Add(MinkowskiSimplex.Support(supportA, supportB, d));
			simplex.Add(MinkowskiSimplex.Support(supportA, supportB, -d));

			d = LineSegmentPointNearestOrigin(simplex.Simplex[1], simplex.Simplex[2]);

			for (int i = 1; i <= 2; i++)
			{
				var newd = LineSegmentPointNearestOrigin(simplex.Simplex[0], simplex.Simplex[i]);
				if (newd.LengthSquared() < d.LengthSquared())
					d = newd;
			}

			Iterations = 0;
			Converged = false;
			while (Iterations < MaxIterations && diff > Tolerance)
			{
				Iterations++;

				if (simplex.ContainsOrigin)
				{
					Converged = true;
					simplex.DistanceFromOrigin = d.Length();
					return simplex;
				}

				d = -d;

				var c = MinkowskiSimplex.Support(supportA, supportB, d);
				var dotc = Vector2.Dot(c.Difference, d);
				var dota = Vector2.Dot(simplex.Last(), d);

				diff = dotc - dota;
				if (diff < Tolerance)
				{
					Converged = true;
					simplex.DistanceFromOrigin = d.Length();
					return simplex;
				}

				var ia = simplex.Simplex.Count - 2;
				var ib = simplex.Simplex.Count - 1;

				var p1 = LineSegmentPointNearestOrigin(c.Difference, simplex.Simplex[ia]);
				var p2 = LineSegmentPointNearestOrigin(c.Difference, simplex.Simplex[ib]);

				if (p1.LengthSquared() < p2.LengthSquared())
				{
					simplex.StaggerInsert(ib, c);
					d = p1;
				}
				else
				{
					simplex.StaggerInsert(ia, c);
					d = p2;
				}
			}

			simplex.DistanceFromOrigin = d.Length();
			return simplex;
		}

		public static Microsoft.Xna.Framework.Vector2 PolygonSupport(Polygon polygon, Microsoft.Xna.Framework.Vector2 d)
		{
			double highest = double.MinValue;
			Microsoft.Xna.Framework.Vector2 support = Microsoft.Xna.Framework.Vector2.Zero;

			foreach (var point in polygon)
			{
				var dot = Vector2.Dot(point, d);

				if (dot > highest)
				{
					highest = dot;
					support = point;
				}
			}

			return support;
		}

		private Microsoft.Xna.Framework.Vector2 LineSegmentPointNearestOrigin(Microsoft.Xna.Framework.Vector2 start, Microsoft.Xna.Framework.Vector2 end)
		{
			var delta = end - start;
			var perp = new Vector2(delta.Y, -delta.X);

			if (delta.LengthSquared() < Tolerance)
				return start;

			var intersection = LineAlgorithms.LineSegmentIntersection(
				start, end, Vector2.Zero, perp);

			if (intersection.WithinFirstSegment)
				return intersection.IntersectionPoint;

			return start.LengthSquared() < end.LengthSquared()
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
				var thisDist = point.LengthSquared();

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
		public Microsoft.Xna.Framework.Vector2 SupportA { get; set; }

		public Microsoft.Xna.Framework.Vector2 SupportB { get; set; }

		public Microsoft.Xna.Framework.Vector2 Difference => SupportA - SupportB;
	}

}
