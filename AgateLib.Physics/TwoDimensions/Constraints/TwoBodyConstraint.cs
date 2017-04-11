using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Physics.TwoDimensions.Constraints
{
	public class TwoBodyConstraint : IPhysicalConstraint
	{
		private IPairConstraint constraint;
		private IParticlePairSelector pairSelector;
		private List<Tuple<PhysicalParticle, PhysicalParticle>> pairs;

		public TwoBodyConstraint(IPairConstraint constraint, IParticlePairSelector pairSelector)
		{
			this.constraint = constraint;
			this.pairSelector = pairSelector;
		}

		public double MultiplierMin => constraint.MultiplierMin;
		public double MultiplierMax => constraint.MultiplierMax;
		public ConstraintType ConstraintType => constraint.ConstraintType;

		public double Value(IReadOnlyList<PhysicalParticle> particles)
		{
			return constraint.Value(new Tuple<PhysicalParticle, PhysicalParticle>(particles.First(), particles.Last()));
		}

		public bool AppliesTo(PhysicalParticle particle)
		{
			throw new NotImplementedException();
		}

		public ConstraintDerivative Derivative(PhysicalParticle particle)
		{
			ConstraintDerivative result = new ConstraintDerivative();

			foreach (var pair in pairs)
			{
				if (pair.Item1 == particle || pair.Item2 == particle)
				{
					result += constraint.Derivative(particle, pair);
				}
			}

			return result;
		}

		public IEnumerable<IReadOnlyList<PhysicalParticle>> ApplyTo(KinematicsSystem system)
		{
			pairs = pairSelector.SelectPairs(system).ToList();
			return pairs.Select(x => new[] { x.Item1, x.Item2 });
		}
	}
}
