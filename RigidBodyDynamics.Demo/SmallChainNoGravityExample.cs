using System.Collections.Generic;
using System.Linq;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace RigidBodyDynamics
{
	public class SmallChainNoGravityExample : IKinematicsExample
	{
		private const int boxSize = 40;
		private const int startingVelocity = 50;
		
		private KinematicsSystem system;

		private Surface boxImage;

		public SmallChainNoGravityExample()
		{
			InitializeImages();
		}

		public string Name => "Chain no gravity";

		public float PotentialEnergy => 0;

		public KinematicsSystem Initialize(Size area)
		{
			var particlePosition = new Vector2(area.Width * .5f, area.Height * .1f);

			system = new KinematicsSystem();

			system.AddParticles(
				new PhysicalParticle
				{
					Position = particlePosition,
					Velocity = new Vector2(0, startingVelocity),
					InertialMoment = boxSize * boxSize / 12f,
				},
				new PhysicalParticle
				{
					Position = new Vector2(particlePosition.X + boxSize, particlePosition.Y),
					InertialMoment = boxSize * boxSize / 12f,
				}
			);
			
			system.AddConstraints(new JointConstraint(
				system.Particles.First(), new Vector2(boxSize * .5f, boxSize * .5f),
				system.Particles.Last(), new Vector2(-boxSize * .5f, boxSize * .5f)));

			return system;
		}

		public void ComputeExternalForces()
		{
			foreach (var box in system.Particles)
			{
				// Gravity force
				box.Force = Vector2.Empty;
			}
		}

		public void Draw()
		{
			foreach (var box in system.Particles)
			{
				boxImage.DisplayAlignment = OriginAlignment.Center;
				boxImage.RotationAngle = box.Angle;
				boxImage.Draw(box.Position);
			}
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