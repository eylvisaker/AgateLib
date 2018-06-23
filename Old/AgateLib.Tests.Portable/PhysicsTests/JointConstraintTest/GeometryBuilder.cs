using AgateLib.Mathematics;
using AgateLib.Physics;
using AgateLib.Physics.TwoDimensions;
using AgateLib.Physics.TwoDimensions.Constraints;

namespace AgateLib.Tests.PhysicsTests.JointConstraintTest
{
	static class GeometryBuilder
	{
		public static void CreateChain(KinematicsSystem system, int boxCount, float boxSize, Vector2 particlePosition)
		{
			for (int i = 0; i < boxCount; i++)
			{
				particlePosition += new Vector2(boxSize, 0);

				system.AddParticles(
					new PhysicalParticle
					{
						Position = particlePosition,
						InertialMoment = boxSize * boxSize / 12f,
					}
				);

				if (i > 0)
				{
					PhysicalParticle last = system.Particles[i - 1];
					PhysicalParticle current = system.Particles[i];

					// Use the sign to alternate which corner is attached. This allows the chain to spread out diagonally.
					int sign = 2 * (i % 2) - 1;

					system.AddConstraints(new JointConstraint(
						last, new Vector2(boxSize * .5f, sign * boxSize * .5f),
						current, new Vector2(-boxSize * .5f, sign * boxSize * .5f)));
				}
			}
		}

	}
}
