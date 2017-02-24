using MathNet.Numerics.LinearAlgebra;

namespace RigidBodyDynamics
{
	/// <summary>
	/// A constraint equation for the kinematics solver to incorporate in the computation of forces.
	/// </summary>
	public interface IPhysicalConstraint
	{
		/// <summary>
		/// Returns the current value of the constraint equation.
		/// </summary>
		double Value { get; }

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

		/// <summary>
		/// Compute the second mixed partial derivative of your constraint with respect to
		/// time and each coordinate.
		/// </summary>
		/// <param name="particle"></param>
		/// <returns></returns>
		ConstraintDerivative MixedPartialDerivative(PhysicalParticle particle);
	}
}