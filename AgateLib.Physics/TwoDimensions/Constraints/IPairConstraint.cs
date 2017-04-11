using System;

namespace AgateLib.Physics.TwoDimensions.Constraints
{
	public interface IPairConstraint
	{
		double MultiplierMin { get; }
		double MultiplierMax { get; }

		ConstraintType ConstraintType { get; }

		double Value(Tuple<PhysicalParticle, PhysicalParticle> tuple);

		ConstraintDerivative Derivative(PhysicalParticle particle, Tuple<PhysicalParticle, PhysicalParticle> pair);
	}
}