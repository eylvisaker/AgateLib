using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Physics.TwoDimensions;
using AgateLib.Physics.TwoDimensions.Constraints;
using AgateLib.Physics.TwoDimensions.Forces;

namespace AgateLib.Tests.PhysicsTests.CollisionConstraintTest
{
	class BoxStackingExample : IPhysicsExample
	{
		private KinematicsSystem system;
		private int rows = 2;

		private ParticleRenderer renderer = new ParticleRenderer();

		public string Name => "Box Stacking";

		public double PotentialEnergy => 0;

		public KinematicsSystem Initialize(Size area)
		{
			system = new KinematicsSystem();
			const int boxSize = 20;

			for (int i = 0; i < rows; i++)
			{
				double y = area.Height - boxSize * (i + 0.5);

				for (int j = 0; j < rows - i; j++)
				{
					int x = (area.Width - boxSize * 2 * rows) / 2 + boxSize * 2 * j + boxSize * i;

					var particle = new PhysicalParticle
					{
						Position = new Vector2(x, y),
						Polygon = new Rectangle(-boxSize, -boxSize / 2, boxSize * 2, boxSize).ToPolygon(),
						InertialMoment = boxSize * boxSize
					};

					system.AddParticles(particle);
				}
			}

			system.AddForceField(new ConstantGravityField());

			system.AddConstraints(new TwoBodyConstraint(new CollisionConstraint(), new AllPairs()));
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
			rows++;
		}

		public void RemoveParticle()
		{
			rows = Math.Max(2, rows - 1);
		}
	}
}
