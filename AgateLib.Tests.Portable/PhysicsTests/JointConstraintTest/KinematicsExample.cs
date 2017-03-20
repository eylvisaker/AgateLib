using AgateLib.Mathematics.Geometry;
using AgateLib.Physics;

namespace AgateLib.Tests.PhysicsTests.JointConstraintTest
{
	public interface IKinematicsExample
	{
		string Name { get; }
		double PotentialEnergy { get; }

		KinematicsSystem Initialize(Size area);

		void Draw();

		void ComputeExternalForces();

		void AddParticle();
		void RemoveParticle();
	}
}