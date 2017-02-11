using AgateLib.Geometry;

namespace RigidBodyDynamics
{
	public class PhysicalParticle
	{
		public Polygon Polygon { get; set; }

		public Vector2 Position { get; set; }
		public Vector2 Velocity { get; set; }
		public Vector2 Force { get; set; }

		public float Angle { get; set; }
		public float AngularVelocity { get; set; }
		public float Torque { get; set; }

		public float Mass { get; set; }

		public float InertialMoment { get; set; }

	}
}