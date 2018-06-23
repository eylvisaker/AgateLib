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
	public class ParticleOnCircleOffCenterExample : IPhysicsExample
	{
		private const int boxSize = 40;
		private const float gravity = 1000f;

		private KinematicsSystem system;

		private Surface boxImage;

		private Vector2 circlePosition;
		private float circleRadius;

		public ParticleOnCircleOffCenterExample()
		{
			InitializeImages();
		}

		private PhysicalParticle Box => system.Particles.First();

		public string Name => "Particle corner on a circle";

		public double PotentialEnergy => system.Particles.Sum(p => p.Mass * p.Position.Y * -gravity);

		public KinematicsSystem Initialize(Size area)
		{
			circleRadius = area.Height * 0.375f;
			circlePosition = new Vector2(area.Width * .5f, area.Height * .5f);

			var particlePosition = circlePosition
			                       + Vector2.FromPolarDegrees(circleRadius, -60)
			                       + new Vector2(boxSize * .5f, boxSize * .5f)
				;//+ new Vector2(0, boxSize * .5f);

			system = new KinematicsSystem();

			system.AddParticles(new PhysicalParticle
			{
				Position = particlePosition,
				//Angle = -(float)Math.PI / 4,
				InertialMoment = boxSize * boxSize / 12f,
			});

			system.AddConstraints(new CirclePerimeterOffcenterConstraint(
				system.Particles[0], circlePosition, circleRadius, new Vector2(-boxSize * .5f, -boxSize * .5f)));

			system.AddForceField(new ConstantGravityField());

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

			Display.Primitives.DrawEllipse(Color.Blue,
				(Rectangle)RectangleF.FromLTRB(
					(float)circlePosition.X - circleRadius,
					(float)circlePosition.Y - circleRadius,
					(float)circlePosition.X + circleRadius,
					(float)circlePosition.Y + circleRadius));
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
		}
		
		public void RemoveParticle()
		{
		}
	}
}