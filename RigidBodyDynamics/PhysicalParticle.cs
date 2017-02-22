using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

namespace RigidBodyDynamics
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
	}
}