using System.Text;

namespace AgateLib.Physics.TwoDimensions
{
	public interface IConstraintSolver
	{
		/// <summary>
		/// Computes the constraint forces from the current state of the system.
		/// </summary>
		void ComputeConstraintForces(double dt);

		void ApplyConstraintForces();

		void DebugInfo(StringBuilder b, int debugPage, PhysicalParticle particle);

		void IntegrateKinematicVariables(double dt);
	}
}