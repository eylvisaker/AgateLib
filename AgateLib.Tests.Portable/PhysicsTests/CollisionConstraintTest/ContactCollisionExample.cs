using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Builders;
using AgateLib.Physics.TwoDimensions;
using AgateLib.Physics.TwoDimensions.Constraints;

namespace AgateLib.Tests.PhysicsTests.CollisionConstraintTest
{
	public class ContactCollisionExample : IPhysicsExample
	{
		class PlanetGravity : IForce
		{
			private PhysicalParticle source;
			private double gravity;

			public PlanetGravity(PhysicalParticle source, double gravity)
			{
				this.source = source;
				this.gravity = gravity;
			}

			public void AccumulateForce(IEnumerable<PhysicalParticle> particles)
			{
				foreach (var particle in particles)
				{
					if (particle == source)
						continue;

					var dist = source.Position - particle.Position;

					var force = dist / Math.Pow(dist.Magnitude, 3) * gravity * particle.Mass;

					particle.Force += force;
				}
			}
		}

		private ParticleRenderer renderer = new ParticleRenderer();
		private KinematicsSystem system;
		private int particles = 1;

		public string Name => "Contact Collision";

		public double PotentialEnergy => 0;

		public KinematicsSystem Initialize(Size area)
		{
			system = new KinematicsSystem();

			var planet = new PhysicalParticle
			{
				Position = (Vector2) area / 2,
				Polygon = new RegularPolygonBuilder().BuildPolygon(6, 60),
				Mass = 10,
				InertialMoment = 5 * 60 * 60,
			};

			system.AddParticles(planet);
			GenerateParticles(planet);

			system.AddConstraints(new TwoBodyConstraint(new CollisionConstraint(), new AllPairs()));
			system.AddForceField(new PlanetGravity(planet, 4000000));

			return system;
		}

		private void GenerateParticles(PhysicalParticle planet)
		{
			system.AddParticles(new ParticleGenerator().Generate(particles,
				i => planet.Position + Vector2.FromPolar(300, 2 * Math.PI * i / particles),
				i => Vector2.FromPolar(50, 2 * Math.PI * i / particles + Math.PI * 0.5)));
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
			particles++;
		}

		public void RemoveParticle()
		{
			particles = Math.Max(1, particles - 1);
		}
	}
}
