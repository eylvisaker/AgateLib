using System;
using System.Linq;
using AgateLib.DisplayLib;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Physics;
using AgateLib.Physics.TwoDimensions;

namespace AgateLib.Tests.PhysicsTests.JointConstraintTest
{
	public class SmallChainNoGravityExample : IPhysicsExample
	{
		private const int boxSize = 40;
		private const int startingVelocity = 350;
		
		private KinematicsSystem system;

		private Surface boxImage;

		public SmallChainNoGravityExample()
		{
			InitializeImages();
		}

		public string Name => "Chain no gravity";

		public double PotentialEnergy => 0;

		public int BoxCount { get; set; } = 2;

		public KinematicsSystem Initialize(Size area)
		{
			var particlePosition = new Vector2(area.Width * .5f, area.Height * .1f);

			system = new KinematicsSystem();

			GeometryBuilder.CreateChain(system, BoxCount, boxSize, particlePosition);

			system.Particles.First().Velocity = new Vector2(-startingVelocity * 0.2, startingVelocity);
			return system;
		}

		public void ComputeExternalForces()
		{
			foreach (var box in system.Particles)
			{
				// Gravity force
				box.Force = Vector2.Zero;
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

			pixels.FillRectangle(Color.FromArgb(220, Color.LightBlue), new Rectangle(Point.Zero, pixels.Size));
			pixels.FillRectangle(Color.FromArgb(220, Color.White), new Rectangle(1, 1, pixels.Width - 2, pixels.Height - 2));

			boxImage = new Surface(pixels);
		}

		public void AddParticle()
		{
			BoxCount++;
		}

		public void RemoveParticle()
		{
			BoxCount = Math.Max(2, BoxCount - 1);
		}
	}
}