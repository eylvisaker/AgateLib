using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Algorithms;

namespace AgateLib.Physics.Constraints
{
	public class RectangleBoundaryConstraint : IPhysicalConstraint
	{
		public RectangleBoundaryConstraint(Rectangle bounds)
		{
			Bounds = bounds;
			InnerBounds = bounds.Contract(bounds.Width / 8);
		}

		public ConstraintType ConstraintType => ConstraintType.Inequality;

		public Rectangle InnerBounds { get; private set; }

		public Rectangle Bounds { get; private set; }

		public double Magnitude { get; set; } = 100;

		public double Value(IReadOnlyList<PhysicalParticle> particles)
		{
			//double x = double.MinValue;
			//double y = double.MinValue;

			//foreach (var point in particles.First().TransformedPolygon)
			//{
			//	x = Math.Max(x, Bounds.Left - point.X);
			//	x = Math.Max(x, point.X - Bounds.Right);

			//	y = Math.Max(y, Bounds.Top - point.Y);
			//	y = Math.Max(y, point.Y - Bounds.Bottom);
			//}

			//if (x > 0 && y > 0)
			//	return x + y;
			//else
			//	return Math.Max(x, y);

			return particles.First().TransformedPolygon
					   .Max(pt => pt.Y) - Bounds.Bottom;
		}

		public bool AppliesTo(PhysicalParticle particle)
		{
			return true;
		}

		public ConstraintDerivative Derivative(PhysicalParticle particle)
		{
			double dx = 0;
			double dy = 0;
			double dphi = 0;

			int pointCount = 0;

			foreach (var point in particle.TransformedPolygon)
			{
				var magnitude = (point - particle.Position).Magnitude;
				var angle = (point - particle.Position).Angle;

				//if (rotatedPoint.X < Bounds.Left)
				//{
				//	dx -= 1;
				//	dphi += -Math.Sin(particle.Angle + angle);
				//}
				//if (rotatedPoint.X > Bounds.Right)
				//{
				//	dx += 1;
				//	dphi += -Math.Sin(particle.Angle + angle);
				//}

				//if (rotatedPoint.Y < Bounds.Top)
				//{
				//	dy -= 1;
				//	dphi += Math.Cos(particle.Angle + angle);
				//}
				if (point.Y > Bounds.Bottom)
				{
					pointCount++;

					dy += 1;
					dphi -= magnitude * Math.Cos(angle);
				}
			}

			return new ConstraintDerivative(dx, dy, dphi);
		}

		public IEnumerable<IReadOnlyList<PhysicalParticle>> ApplyTo(KinematicsSystem system)
		{
			List<PhysicalParticle> result = new List<PhysicalParticle>();

			result.Add(null);

			foreach (var particle in system.Particles)
			{
				result[0] = particle;
				yield return result;
			}
		}

		private Polygon ApproximateMinkowskiDifference(Vector2 start,
			Func<Vector2, Vector2> supportA, Func<Vector2, Vector2> supportB)
		{
			Polygon result = new Polygon();

			Vector2 v = start;

			while (true)
			{
				var w = supportA(v) - supportB(-v);

				result.Add(w);
				v = w;

				result = ConvexHull(result);
			}
		}

		private Polygon ConvexHull(Polygon polygon)
		{
			// QuickHull algorithm:
			// https://en.wikipedia.org/wiki/Quickhull

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
			Vector2 maxV = set.First();

			foreach (var v in set)
			{
				var distance = LineAlgorithms.SideOf(P, Q, v);

				if (distance > max)
				{
					max = distance;
					maxV = v;
				}
			}

			var indexA = result.IndexOf(P);
			var indexB = result.IndexOf(Q);

			if (indexB > indexA)
			{
				result.Insert(indexB, maxV);
			}
			else
			{
				result.Add(maxV);
			}

			var S1 = set.FindAll(v => LineAlgorithms.SideOf(P, maxV, v) > 0);
			var S2 = set.FindAll(v => LineAlgorithms.SideOf(maxV, Q, v) > 0);

			FindHull(result, S1, P, maxV);
			FindHull(result, S2, maxV, Q);
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
	}
}
