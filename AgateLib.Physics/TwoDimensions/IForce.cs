using System.Collections.Generic;

namespace AgateLib.Physics.TwoDimensions
{
	/// <summary>
	/// Interface for a force.
	/// </summary>
	public interface IForce
	{
		/// <summary>
		/// Computes and adds the amount of force for each particle in the list.
		/// </summary>
		/// <param name="particles"></param>
		void AccumulateForce(IEnumerable<PhysicalParticle> particles);
	}
}