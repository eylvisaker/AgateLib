using System;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Physics
{
	public class PhysicalParticle
	{
		/// <summary>
		/// Bounding polygon for this particle. This is not used by the kinematics integrator
		/// but it might be used by constraints that affect this particle.
		/// </summary>
		public Polygon Polygon;

		/// <summary>
		/// Particle position.
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// Particle velocity.
		/// </summary>
		public Vector2 Velocity;

		/// <summary>
		/// Particle force.
		/// </summary>
		public Vector2 Force;

		[Obsolete]
		public Vector2 ConstraintForce;

		/// <summary>
		/// Particle rotation angle.
		/// </summary>
		public double Angle;

		/// <summary>
		/// Particle angular velocity.
		/// </summary>
		public double AngularVelocity;

		/// <summary>
		/// Particle torque.
		/// </summary>
		public double Torque;

		[Obsolete]
		public double ConstraintTorque;

		/// <summary>
		/// Particle mass. Must not be zero.
		/// </summary>
		public double Mass = 1;

		/// <summary>
		/// Particle intertial moment. Must not be zero.
		/// </summary>
		public double InertialMoment = 1;

		/// <summary>
		/// Clones this PhysicalParticle object.
		/// </summary>
		/// <returns></returns>
		public PhysicalParticle Clone()
		{
			var result = new PhysicalParticle();
			CopyTo(result);

			return result;
		}

		/// <summary>
		/// Copies data to another PhysicalParticle object.
		/// </summary>
		/// <param name="target"></param>
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