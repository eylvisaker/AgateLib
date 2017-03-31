using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Algorithms;

namespace AgateLib.Physics.TwoDimensions.Constraints
{
	public class RectangleBoundaryConstraint : IPhysicalConstraint
	{
		public RectangleBoundaryConstraint(Rectangle bounds)
		{
			Bounds = bounds;
			InnerBounds = bounds.Contract(bounds.Width / 8);
		}

		public double MultiplierMin => double.MinValue;

		public double MultiplierMax => 0;

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

	}
}
