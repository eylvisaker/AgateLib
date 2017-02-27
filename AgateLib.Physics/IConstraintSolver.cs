using System.Text;

namespace AgateLib.Physics
{
	public interface IConstraintSolver
	{
		/// <summary>
		/// Computes the constraint forces from the current state of the system.
		/// </summary>
		void ComputeConstraintForces(float dt);

		void ApplyConstraintForces();

		void DebugInfo(StringBuilder b, int debugPage, PhysicalParticle particle);
	}
}