using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Builders;
using AgateLib.Physics.TwoDimensions;

namespace AgateLib.Tests.PhysicsTests.CollisionConstraintTest
{
	class ParticleGenerator
	{
		private Random random;

		public ParticleGenerator() : this(new Random(2))
		{
		}

		public ParticleGenerator(Random rnd)
		{
			random = rnd;
		}

		public IEnumerable<PhysicalParticle> Generate(int particleCount, Vector2 startPoint)
		{
			return Enumerable.Range(0, particleCount).Select(i => GenerateParticle(startPoint));
		}

		public IEnumerable<PhysicalParticle> Generate(int particleCount, Func<int, Vector2> startPoint)
		{
			return Enumerable.Range(0, particleCount).Select(i => GenerateParticle(startPoint(i)));
		}

		private PhysicalParticle GenerateParticle(Vector2 startPoint)
		{
			var size = random.Next(15, 80);
			var innerSize = random.NextDouble() * 0.7 * size + 1;

			var particle = new PhysicalParticle
			{
				Polygon = new StarBuilder().BuildStar(random.Next(3, 8), size, innerSize),
				Position = startPoint,
				Velocity = new Vector2(random.Next(-100, 100), random.Next(-100, 200)),
				AngularVelocity = random.NextDouble() * 4,
			};

			return particle;
		}
	}
}
