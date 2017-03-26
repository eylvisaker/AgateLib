using System;
using System.Collections.Generic;
using AgateLib.Mathematics;

namespace AgateLib.Physics.TwoDimensions.Constraints
{
	/// <summary>
	/// Constrains a particle to exist on a circle.
	/// </summary>
	public class CirclePerimeterOffcenterConstraint : IPhysicalConstraint
	{
		private Vector2 circlePosition;
		private float circleRadius;
		private PhysicalParticle particle;
		private Vector2 offset;

		public CirclePerimeterOffcenterConstraint(PhysicalParticle particle, Vector2 circlePosition, float circleRadius, Vector2 offset)
		{
			this.particle = particle;
			this.circlePosition = circlePosition;
			this.circleRadius = circleRadius;
			this.offset = offset;
		}

		public double MultiplierMin => double.MinValue;
		public double MultiplierMax => double.MaxValue;

		public ConstraintType ConstraintType => ConstraintType.Equality;

		private Vector2 ConstrainedPointLocalPosition => offset.Rotate(particle.Angle);

		/// <summary>
		/// Returns the position of the constrained point on the particle, relative to the circle's center.
		/// </summary>
		/// <remarks>\f$\vec{r}\f$ from the math.</remarks>
		private Vector2 ConstrainedPointPosition => particle.Position + ConstrainedPointLocalPosition - circlePosition;

		private Vector2 OffsetDerivative => offset.Rotate(particle.Angle + (float)Math.PI * .5f);

		public double Value(IReadOnlyList<PhysicalParticle> particles) => .5f * (ConstrainedPointPosition.MagnitudeSquared - circleRadius * circleRadius);

		public bool AppliesTo(PhysicalParticle particle)
		{
			return ReferenceEquals(this.particle, particle);
		}

		public ConstraintDerivative Derivative(PhysicalParticle particle)
		{
			return new ConstraintDerivative(
				ConstrainedPointPosition.X,
				ConstrainedPointPosition.Y,
				Vector2.DotProduct(ConstrainedPointPosition, OffsetDerivative));
		}

		public IEnumerable<IReadOnlyList<PhysicalParticle>> ApplyTo(KinematicsSystem system)
		{
			yield return new List<PhysicalParticle> { particle };
		}

	}
}
