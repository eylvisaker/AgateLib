using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Physics
{
	public class PhysicalParticle
	{
		public Polygon Polygon { get; set; }

		public Vector2 Position { get; set; }
		public Vector2 Velocity { get; set; }
		public Vector2 Force { get; set; }
		public Vector2 ConstraintForce { get; set; }

		public double Angle { get; set; }
		public double AngularVelocity { get; set; }
		public double Torque { get; set; }
		public double ConstraintTorque { get; set; }

		public double Mass { get; set; }

		public double InertialMoment { get; set; }

		public PhysicalParticle Clone()
		{
			var result = new PhysicalParticle();
			CopyTo(result);

			return result;
		}

		public void CopyTo(PhysicalParticle target)
		{
			target.Polygon = Polygon;

			target.Position = Position;
			target.Velocity = Velocity;
			target.Force = Force;
			target.ConstraintForce = ConstraintForce;

			target.Angle = Angle;
			target.AngularVelocity = AngularVelocity;
			target.Torque = Torque;
			target.ConstraintTorque = ConstraintTorque;

			target.Mass = Mass;
			target.InertialMoment = InertialMoment;
		}

		/// <summary>
		/// Integrates the dynamics for this particle.
		/// </summary>
		/// <param name="dt"></param>
		public virtual void Integrate(double dt)
		{
			var oldVelocity = Velocity;
			var oldAngularVelocity = AngularVelocity;

			AngularVelocity += dt * Torque / InertialMoment;
			Velocity += dt * Force / Mass;

			// Cheap way to improve the integrator: use the average of the 
			// start & final velocities for the position integration.
			Angle += dt * 0.5 * (AngularVelocity + oldAngularVelocity);
			Position += dt * 0.5 * (Velocity + oldVelocity);
		}
	}
}