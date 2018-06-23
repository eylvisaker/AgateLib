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
	public class HittingTheGroundExample : IPhysicsExample
	{
		private KinematicsSystem system;
		private Size screenArea;
		private int particleCount = 1;
		private ParticleRenderer renderer = new ParticleRenderer();

		public string Name => "Hitting the ground";

		public double PotentialEnergy => 0;

		public KinematicsSystem Initialize(Size area)
		{
			screenArea = area;


			ParticleGenerator generator = new ParticleGenerator();

			system = new KinematicsSystem();

			system.AddParticles(generator.Generate(particleCount, new Vector2(screenArea.Width / 2, 100), Vector2.Zero));

			system.AddForceField(new ConstantGravityField());

			system.AddConstraints(new RectangleBoundaryConstraint(new Rectangle(Point.Zero, area)));

			return system;
		}

		public void Draw()
		{
			renderer.Draw(system);
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

	}
}
