using System.Collections.Generic;
using AgateLib.Geometry;

namespace RigidBodyDynamics
{
	public interface IKinematicsExample
	{
		string Name { get; }
		float PotentialEnergy { get; }

		KinematicsSystem Initialize(Size area);

		void Draw();

		void ComputeExternalForces();
	}
}