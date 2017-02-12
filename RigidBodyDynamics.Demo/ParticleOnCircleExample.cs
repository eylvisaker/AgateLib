using System.Collections.Generic;
using System.Linq;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace RigidBodyDynamics
{
	public class ParticleOnCircleExample : IKinematicsExample
	{
		private const int boxSize = 40;
		private const float gravity = 1000f;

		private KinematicsSystem system;

		private Surface boxImage;

		private Vector2 circlePosition;
		private float circleRadius;

		public ParticleOnCircleExample()
		{
			InitializeImages();
		}

		private PhysicalParticle Box => system.Particles.First();

		public string Name => "Particle on a circle";

		public float PotentialEnergy => system.Particles.Sum(p => p.Mass * p.Position.Y * -gravity);

		public KinematicsSystem Initialize(Size area)
		{
			circleRadius = area.Height * 0.375f;
			circlePosition = new Vector2(area.Width * 0.5f, area.Height * 0.5f);

			var particlePosition = circlePosition + Vector2.FromPolarDegrees(circleRadius, -60);

			system = new KinematicsSystem();

			system.AddParticles(new PhysicalParticle
			{
				Position = particlePosition,
				Angle = 1.5f,
				AngularVelocity = 20f,
			});

			system.AddConstraints(new CirclePerimeterConstraint(system.Particles[0], circlePosition, circleRadius));

			return system;
		}

		public void ComputeExternalForces()
		{
			// Gravity force
			Box.Force = new Vector2(0, gravity * Box.Mass);
		}

		public void Draw()
		{
			var box = system.Particles.First();

			boxImage.DisplayAlignment = OriginAlignment.Center;
			boxImage.RotationAngle = box.Angle;
			boxImage.Draw(box.Position);

			Display.DrawEllipse(
				(Rectangle)RectangleF.FromLTRB(
					circlePosition.X - circleRadius,
					circlePosition.Y - circleRadius,
					circlePosition.X + circleRadius,
					circlePosition.Y + circleRadius), Color.Blue);
		}

		private void InitializeImages()
		{
			var pixels = new PixelBuffer(Display.DefaultSurfaceFormat, new Size(boxSize, boxSize));
			pixels.FillRectangle(Color.FromArgb(220, Color.LightBlue), new Rectangle(Point.Empty, pixels.Size));
			pixels.FillRectangle(Color.FromArgb(220, Color.White), new Rectangle(1, 1, pixels.Width - 2, pixels.Height - 2));

			boxImage = new Surface(pixels);
		}
	}
}