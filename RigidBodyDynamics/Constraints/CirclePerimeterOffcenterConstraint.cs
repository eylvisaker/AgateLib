using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;

namespace RigidBodyDynamics
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

		private Vector2 ConstrainedPointLocalPosition => offset.Rotate(particle.Angle);

		/// <summary>
		/// Returns the position of the constrained point on the particle, relative to the circle's center.
		/// </summary>
		/// <remarks>\f$\vec{r}\f$ from the math.</remarks>
		private Vector2 ConstrainedPointPosition => particle.Position + ConstrainedPointLocalPosition - circlePosition;

		private Vector2 OffsetDerivative => offset.Rotate(particle.Angle + (float)Math.PI * .5f);

		public float Value => .5f * (ConstrainedPointPosition.MagnitudeSquared - circleRadius * circleRadius);

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


		public ConstraintDerivative MixedPartialDerivative(PhysicalParticle particle)
		{
			return new ConstraintDerivative(
				particle.Velocity.X,
				particle.Velocity.Y,
				Vector2.DotProduct(particle.Velocity, OffsetDerivative) - 
				Vector2.DotProduct(ConstrainedPointPosition, ConstrainedPointLocalPosition));
		}
	}
}
