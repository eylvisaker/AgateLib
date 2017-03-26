using System;
using System.Linq;
using AgateLib.DisplayLib;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Physics;
using AgateLib.Physics.TwoDimensions;
using AgateLib.Physics.TwoDimensions.Constraints;
using AgateLib.Physics.TwoDimensions.Forces;

namespace AgateLib.Tests.PhysicsTests.JointConstraintTest
{
	public class ChainOnCircleExample : IPhysicsExample
	{
		private const int boxSize = 40;
		private const float gravity = 1000f;

		private KinematicsSystem system;

		private Surface boxImage;

		private Vector2 circlePosition;
		private float circleRadius;

		public ChainOnCircleExample()
		{
			InitializeImages();
		}

		public string Name => "Chain on a circle";

		public double PotentialEnergy => system.Particles.Sum(p => p.Mass * p.Position.Y * -gravity);

		public int BoxCount { get; set; } = 2;

		public KinematicsSystem Initialize(Size area)
		{
			circleRadius = area.Height * 0.375f;
			circlePosition = new Vector2(area.Width * .5f, area.Height * .5f);

			var particlePosition = circlePosition + Vector2.FromPolarDegrees(circleRadius, -90)
			                       + new Vector2(0, boxSize * .5f);

			system = new KinematicsSystem();

			GeometryBuilder.CreateChain(system, BoxCount, boxSize, particlePosition);

			system.AddConstraints(new CirclePerimeterOffcenterConstraint(
				system.Particles.First(), circlePosition, circleRadius, new Vector2(-boxSize * .5f, -boxSize * .5f)));

			system.AddForceField(new ConstantGravityField());

			return system;
		}

		public void ComputeExternalForces()
		{
			foreach (var box in system.Particles)
			{
				// Gravity force
				box.Force = new Vector2(0, gravity * box.Mass);
			}
		}

		public void Draw()
		{
			foreach (var box in system.Particles)
			{
				boxImage.DisplayAlignment = OriginAlignment.Center;
				boxImage.RotationAngle = box.Angle;
				boxImage.Draw(box.Position);

				Display.Primitives.DrawEllipse(Color.Blue,
					(Rectangle) RectangleF.FromLTRB(
						(float)circlePosition.X - circleRadius,
						(float)circlePosition.Y - circleRadius,
						(float)circlePosition.X + circleRadius,
						(float)circlePosition.Y + circleRadius));
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