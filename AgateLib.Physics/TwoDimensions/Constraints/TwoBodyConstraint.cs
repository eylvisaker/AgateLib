//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

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

		public float MultiplierMin => constraint.MultiplierMin;
		public float MultiplierMax => constraint.MultiplierMax;
		public ConstraintType ConstraintType => constraint.ConstraintType;

		public float Value(IReadOnlyList<PhysicalParticle> particles)
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
