using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace AgateLib.Physics.TwoDimensions
{
	class AppliedConstraint
	{
		public IPhysicalConstraint Constraint { get; set; }
		public IReadOnlyList<PhysicalParticle> Particles { get; set; }

		public double Value => Constraint.Value(Particles);
		public double MultiplierMin => Constraint.MultiplierMin;
		public double MultiplierMax => Constraint.MultiplierMax;

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
