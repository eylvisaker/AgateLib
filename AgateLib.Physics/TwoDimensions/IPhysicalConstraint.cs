using System.Collections.Generic;

namespace AgateLib.Physics.TwoDimensions
{
	/// <summary>
	/// A constraint equation for the kinematics solver to incorporate in the computation of forces.
	/// </summary>
	public interface IPhysicalConstraint
	{
		double MultiplierMin { get; }
		double MultiplierMax { get; }

		/// <summary>
		/// Return true to have the constraint applied on ly if its value is positive.
		/// </summary>
		ConstraintType ConstraintType { get; }

		/// <summary>
		/// Returns the current value of the constraint equation.
		/// </summary>
		double Value(IReadOnlyList<PhysicalParticle> particles);

		/// <summary>
		/// Returns true if the constraint applies to the specified object.
		/// </summary>
		/// <param name="particle"></param>
		/// <returns></returns>
		bool AppliesTo(PhysicalParticle particle);

		/// <summary>
		/// Compute the derivative of your constraint with respect to each coordinate of the particle object.
		/// </summary>
		/// <param name="particle"></param>
		/// <returns></returns>
		ConstraintDerivative Derivative(PhysicalParticle particle);

		IEnumerable<IReadOnlyList<PhysicalParticle>> ApplyTo(KinematicsSystem system);

	}

	public enum ConstraintType
	{
		Equality,
		Inequality,
	}
}