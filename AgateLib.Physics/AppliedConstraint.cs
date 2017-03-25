using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Physics
{
	class AppliedConstraint
	{
		public IPhysicalConstraint Constraint { get; set; }
		public IReadOnlyList<PhysicalParticle> Particles { get; set; }

		public double Value => Constraint.Value(Particles);

		public bool AppliesTo(PhysicalParticle physicalParticle)
		{
			return Particles.Contains(physicalParticle);
		}

		public ConstraintDerivative Derivative(PhysicalParticle physicalParticle)
		{
			return Constraint.Derivative(physicalParticle);
		}
	}
}
