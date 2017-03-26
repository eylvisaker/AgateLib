using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.InputLib.GamepadModel;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Builders;
using AgateLib.Physics;
using AgateLib.Physics.TwoDimensions;
using AgateLib.Physics.TwoDimensions.Constraints;
using AgateLib.Physics.TwoDimensions.Forces;
using AgateLib.Platform;

namespace AgateLib.Tests.PhysicsTests.CollisionConstraintTest
{
	public class RigidCollisionConstrantExample : IPhysicsExample
	{
		private KinematicsSystem system;
		private Color[] colors;
		private Size screenArea;
		private int particleCount = 1;

		public RigidCollisionConstrantExample()
		{
			colors = new[]
			{
				Color.Red, Color.Yellow, Color.Green, Color.Blue, Color.Purple, Color.Violet, Color.Indigo,
				Color.Orange, Color.White,
			};

			for (int i = 0; i < colors.Length; i++)
				colors[i] = Color.FromArgb(196, colors[i]);
		}

		public string Name => "Collision";

		public double PotentialEnergy => 0;

		public KinematicsSystem Initialize(Size area)
		{
			screenArea = area;

			system = new KinematicsSystem();

			var random = new Random(2);

			for (int i = 0; i < particleCount; i++)
			{
				AddRandomParticle(random);
			}

			system.AddForceField(new ConstantGravityField());

			system.AddConstraints(new RectangleBoundaryConstraint(new Rectangle(Point.Zero, area)));

			return system;
		}

		public void Draw()
		{
			int index = 0;

			foreach (var particle in system.Particles)
			{
				Display.Primitives.FillPolygon(colors[index % colors.Length],
					particle.TransformedPolygon);

				index++;
			}
		}

		public void ComputeExternalForces()
		{
		}

		public void AddParticle()
		{
			particleCount++;
		}

		public void RemoveParticle()
		{
			particleCount--;

			if (particleCount <= 0)
				particleCount = 1;
		}

		private void AddRandomParticle(Random random)
		{
			var size = random.Next(15, 80);
			var innerSize = random.NextDouble() * 0.7 * size + 1;

			var particle = new PhysicalParticle
			{
				Polygon = new StarBuilder().BuildStar(random.Next(3, 8), size, innerSize),
				Position = new Vector2(screenArea.Width / 2, 100),
				Velocity = new Vector2(random.Next(-100, 100), random.Next(-100, 200)),
				AngularVelocity = random.NextDouble() * 4,
			};

			system.AddParticles(particle);
		}
	}
}
