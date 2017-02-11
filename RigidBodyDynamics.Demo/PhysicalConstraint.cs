namespace RigidBodyDynamics
{
	/// <summary>
	/// A constraint equation for the kinematics solver to incorporate in the computation of forces.
	/// </summary>
	public interface IPhysicalConstraint
	{
		/// <summary>
		/// Compute the derivative of your constraint with respect to each coordinate of the physical object.
		/// </summary>
		/// <param name="physicalObject"></param>
		/// <returns></returns>
		ConstraintDerivative Derivative(Physical physicalObject);

		/// <summary>
		/// Returns true if the constraint applies to the specified object.
		/// </summary>
		/// <param name="physicalObject"></param>
		/// <returns></returns>
		bool AppliesTo(Physical physicalObject);
	}
}