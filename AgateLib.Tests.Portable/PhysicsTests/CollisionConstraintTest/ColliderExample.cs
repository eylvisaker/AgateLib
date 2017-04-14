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
	public class ColliderExample : IPhysicsExample
	{
		private KinematicsSystem system;
		private Size screenArea;
		private int particleCount = 2;
		private ParticleRenderer renderer = new ParticleRenderer();

		public string Name => "Hitting the ground";

		public double PotentialEnergy => 0;

		public KinematicsSystem Initialize(Size area)
		{
			screenArea = area;


			ParticleGenerator generator = new ParticleGenerator();

			system = new KinematicsSystem();

			system.AddParticles(generator.Generate(particleCount,
				i => new Vector2(50 + 200 * i, 200), 
				i => Vector2.Zero));

			system.AddConstraints(new TwoBodyConstraint(new CollisionConstraint(), new AllPairs()));

			system.Particles.First().Velocity.X = 100;

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

			if (particleCount <= 2)
				particleCount = 2;
		}

	}
}
