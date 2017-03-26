using System.Collections.Generic;
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
	public class BoxChainExample : IPhysicsExample
	{
		private const int boxSize = 40;
		private const float gravity = 100f;

		private KinematicsSystem system;

		private Surface boxImage;

		private List<PhysicalParticle> boxes = new List<PhysicalParticle>();
		private PhysicalParticle sphere = new PhysicalParticle();

		private IReadOnlyList<IPhysicalConstraint> constraints => system.Constraints;

		public BoxChainExample()
		{
			InitializeImages();
		}

		public string Name => "Box Chain";

		public double PotentialEnergy => boxes.Sum(p => p.Mass * p.Position.Y * -gravity);

		public int BoxCount { get; set; } = 8;

		public KinematicsSystem Initialize(Size area)
		{
			boxes.Clear();

			system = new KinematicsSystem();

			for (int i = 0; i < BoxCount; i++)
			{
				boxes.Add(new PhysicalParticle
				{
					Polygon = new Rectangle(-boxSize / 2, -boxSize / 2, boxSize, boxSize).ToPolygon(),
					Position = new Vector2(
						area.Width * 0.5 + (BoxCount / 2 - i) * boxSize,
						area.Height * 0.3
					),
					Velocity = new Vector2(0, (BoxCount / 2 - i) * 100)
				});

				if (i > 0)
				{
					var constraint = new JointConstraint(boxes[i - 1], new Vector2(-boxSize * 0.5, 0),
						boxes[i], new Vector2(boxSize * 0.5, 0));

					system.AddConstraints(constraint);
				}
			}

			sphere = new PhysicalParticle
			{
				Position = new Vector2(area.Width * 0.5, area.Height * 0.25)
			};

			var constraintBox = boxes[boxes.Count / 2];

			system.AddConstraints(
				new CirclePerimeterOffcenterConstraint(constraintBox,
					sphere.Position, 40, constraintBox.Polygon[0]));

			system.AddParticles(boxes.ToArray());

			system.AddForceField(new ConstantGravityField());

			return system;
		}

		public void ComputeExternalForces()
		{
			foreach (var box in boxes)
			{
				// Gravity force
				box.Force = new Vector2(0, gravity * box.Mass);
			}
		}

		public void Draw()
		{
			foreach (var box in boxes)
				DrawBox(box);

			DrawSphere(sphere);
		}

		private void DrawBox(PhysicalParticle box)
		{
			boxImage.DisplayAlignment = OriginAlignment.Center;
			boxImage.RotationAngle = box.Angle;
			boxImage.Draw(box.Position);
		}

		private void DrawSphere(PhysicalParticle physicalParticle)
		{
			Display.Primitives.FillCircle(Color.Blue, physicalParticle.Position, 40);
		}

		private void InitializeImages()
		{
			var pixels = new PixelBuffer(Display.DefaultSurfaceFormat, new Size(boxSize, boxSize / 2));
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
			BoxCount--;
			if (BoxCount < 2)
				BoxCount = 2;
		}
	}
}