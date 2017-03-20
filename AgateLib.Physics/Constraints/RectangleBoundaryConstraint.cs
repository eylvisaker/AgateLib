using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

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
			double x = double.MinValue;
			double y = double.MinValue;

			foreach (var point in particles.First().TransformedPolygon)
			{
				x = Math.Max(x, Bounds.Left - point.X);
				x = Math.Max(x, point.X - Bounds.Right);

				y = Math.Max(y, Bounds.Top - point.Y);
				y = Math.Max(y, point.Y - Bounds.Bottom);
			}

			if (x > 0 && y > 0)
				return x + y;
			else
				return Math.Max(x, y);
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

			foreach (var point in particle.Polygon)
			{
				var rotatedPoint = point.Rotate(particle.Angle);
				var angle = rotatedPoint.Angle;

				if (rotatedPoint.X < Bounds.Left)
				{
					dx -= 1;
					dphi += -Math.Sin(particle.Angle + angle);
				}
				if (rotatedPoint.X > Bounds.Right)
				{
					dx += 1;
					dphi += -Math.Sin(particle.Angle + angle);
				}

				if (rotatedPoint.Y < Bounds.Top)
				{
					dy -= 1;
					dphi += Math.Cos(particle.Angle + angle);
				}
				if (rotatedPoint.Y > Bounds.Bottom)
				{
					dy += 1;
					dphi += Math.Cos(particle.Angle + angle);
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
