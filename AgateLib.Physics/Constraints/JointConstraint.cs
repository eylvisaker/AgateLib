using System;
using AgateLib.Mathematics;

namespace AgateLib.Physics.Constraints
{
	/// <summary>
	/// Implements a constraint which forces the two points relative to each particle be kept at a single point.
	/// </summary>
	/// <remarks>
	/// Math goes here!
	/// </remarks>
	public class JointConstraint : IPhysicalConstraint
	{
		private PhysicalParticle particle1;
		private PhysicalParticle particle2;
		private Vector2 point1;
		private Vector2 point2;

		public JointConstraint(PhysicalParticle particle1, Vector2 point1, PhysicalParticle particle2, Vector2 point2)
		{
			this.particle1 = particle1;
			this.particle2 = particle2;
			this.point1 = point1;
			this.point2 = point2;
		}

		/// <summary>
		/// The distance between the two points that should be fixed.
		/// When the constraint is satisfied, this should be zero.
		/// </summary>
		private Vector2 Displacement
		{
			get
			{
				var abs1 = PointPosition(particle1, point1);
				var abs2 = PointPosition(particle2, point2);
				return abs1 - abs2;
			}
		}

		/// <summary>
		/// The rate of change of Displacement with respect to time.
		/// </summary>
		private Vector2 DisplacementVelocity
		{
			get
			{
				var abs1 = PointVelocity(particle1, point1);
				var abs2 = PointVelocity(particle2, point2);
				return abs1 - abs2;
			}
		}
		/// <summary>
		/// Returns the current value of the constraint equation. This can be used to measure the amount of error in the
		/// algorithm.
		/// </summary>
		/// <returns></returns>
		public double Value => .5f * Displacement.MagnitudeSquared;

		public bool AppliesTo(PhysicalParticle obj)
		{
			if (particle1 == obj)
				return true;
			if (particle2 == obj)
				return true;

			return false;
		}

		public ConstraintDerivative Derivative(PhysicalParticle particle)
		{
			int sign = particle == particle1 ? 1 : -1;
			var point = particle == particle1 ? point1 : point2;

			var B = Displacement;
			var dA = PointDerivativeAngle(particle, point);
			var dot = B.DotProduct(dA);

			return sign * new ConstraintDerivative(
				       B.X,
				       B.Y,
				       dot);
		}

		/// <summary>
		/// Calculates the position of the particle's point taking into account rotation angle.
		/// </summary>
		/// <param name="particle"></param>
		/// <param name="point"></param>
		/// <remarks>This value is \f$\vec{R}\f$ in the math.</remarks>
		/// <returns></returns>
		private Vector2 PointRelativePosition(PhysicalParticle particle, Vector2 point)
		{
			return point.Rotate(particle.Angle);
		}

		/// <summary>
		/// Calculates the derivative of the particle's point with respect to its angle coordinate.
		/// </summary>
		/// <param name="particle"></param>
		/// <param name="point"></param>
		/// <returns></returns>
		private Vector2 PointDerivativeAngle(PhysicalParticle particle, Vector2 point)
		{
			var result = point.RotationDerivative(particle.Angle);
			return result;
		} 

		/// <summary>
		/// Calculates the position of the particle's point in global coordinates.
		/// </summary>
		/// <param name="particle"></param>
		/// <param name="point"></param>
		/// <remarks>This value is \f$\vec{R}\f$ in the math.</remarks>
		/// <returns></returns>
		private Vector2 PointPosition(PhysicalParticle particle, Vector2 point)
		{
			return particle.Position + PointRelativePosition(particle, point);
		}

		/// <summary>
		/// Calculates the velocity of the particle's point, taking into account angular motion.
		/// </summary>
		/// <param name="particle"></param>
		/// <param name="point"></param>
		/// <remarks>This value is \f$\dot{\vec{R}}\f$ in the math.</remarks>
		/// <returns></returns>
		private Vector2 PointVelocity(PhysicalParticle particle, Vector2 point)
		{
			return particle.Velocity + particle.AngularVelocity * PointDerivativeAngle(particle, point);
		}
	}
}