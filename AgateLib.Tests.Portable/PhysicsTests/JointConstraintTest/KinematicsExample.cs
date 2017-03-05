﻿using System.Collections.Generic;
using AgateLib.Mathematics.Geometry;
using AgateLib.Physics;

namespace RigidBodyDynamics.Demo
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