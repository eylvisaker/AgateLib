using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Physics.TwoDimensions.Constraints
{
	public class CollisionConstraint : IPhysicalConstraint
	{
		public double MultiplierMin { get; }
		public double MultiplierMax { get; }

		public ConstraintType ConstraintType { get; }

		public double Value(IReadOnlyList<PhysicalParticle> particles)
		{
			return 0;
		}

		public bool AppliesTo(PhysicalParticle particle)
		{
			throw new NotImplementedException();
		}

		public ConstraintDerivative Derivative(PhysicalParticle particle)
		{
			return new ConstraintDerivative();
		}

		public IEnumerable<IReadOnlyList<PhysicalParticle>> ApplyTo(KinematicsSystem system)
		{
			foreach (var p1 in system.Particles)
			{
				foreach (var p2 in system.Particles)
				{
					if (p2 == p1)
						continue;

					yield return new List<PhysicalParticle> {p1, p2};
				}
			}
		}
	}
}
