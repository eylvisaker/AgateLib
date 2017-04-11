using System;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Quality;

namespace AgateLib.Physics.TwoDimensions
{
	public class PhysicalParticle
	{
		private Polygon untransformed;
		private Polygon transformed;

		private double mass = 1;
		private double inertialMoment = 1;

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

		/// <summary>
		/// Force exerted by constraints.
		/// </summary>
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

		/// <summary>
		/// Torque exerted by constraints.
		/// </summary>
		public double ConstraintTorque;

		/// <summary>
		/// The untransformed bounding polygon for this particle. This can be used by a rigid body constraint
		/// to handle collisions.
		/// </summary>
		public Polygon Polygon
		{
			get { return untransformed; }
			set
			{
				untransformed = value;
				transformed = null;
			}
		}

		/// <summary>
		/// The transformed polygon representing this object's physical location in space.
		/// </summary>
		public Polygon TransformedPolygon
		{
			get
			{
				if (transformed == null)
					UpdatePolygonTransformation();

				return transformed;
			}
		}

		/// <summary>
		/// Particle mass. Must not be positive.
		/// </summary>
		public double Mass
		{
			get { return mass; }
			set
			{
				Require.ArgumentInRange(mass > 0, nameof(mass), "Mass must be positive.");
				mass = value;
			}
		}

		/// <summary>
		/// Particle intertial moment. Must not be positive.
		/// </summary>
		public double InertialMoment
		{
			get { return inertialMoment; }
			set
			{
				Require.ArgumentInRange(inertialMoment > 0, nameof(inertialMoment), "Inertial moment must be positive.");
				inertialMoment = value;
			}
		}

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
		/// Integrates the equations of motion for this particle alone. 
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

			UpdatePolygonTransformation();
		}

		internal void UpdatePolygonTransformation()
		{
			if (untransformed == null)
				return;

			if (transformed == null)
				transformed = new Polygon();

			untransformed.CopyTo(transformed);

			transformed.RotateSelf(Angle);
			transformed.TranslateSelf(Position);
		}
	}
}