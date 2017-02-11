using AgateLib.Geometry;

namespace RigidBodyDynamics
{
	public class Physical
	{
		public Polygon Polygon { get; set; }

		public Vector2 Position { get; set; }
		public Vector2 Velocity { get; set; }
		public Vector2 Force { get; set; }

		public double Angle { get; set; }
		public double AngularVelocity { get; set; }
		public double Torque { get; set; }

		public double Mass { get; set; }

		public double IntertialMoment { get; set; }

	}
}