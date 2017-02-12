using AgateLib.Geometry;

namespace RigidBodyDynamics
{
	public class PhysicalParticle
	{
		public Polygon Polygon { get; set; }

		public Vector2 Position { get; set; }
		public Vector2 Velocity { get; set; }
		public Vector2 Force { get; set; }
		public Vector2 ConstraintForce { get; set; }

		public float Angle { get; set; }
		public float AngularVelocity { get; set; }
		public float Torque { get; set; }
		public float ConstraintTorque { get; set; }

		public float Mass { get; set; }

		public float InertialMoment { get; set; }

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

			target.Mass = Mass;
			target.InertialMoment = InertialMoment;
		}
	}
}