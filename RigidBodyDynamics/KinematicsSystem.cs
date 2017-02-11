using System.Collections.Generic;

namespace RigidBodyDynamics
{
	public class KinematicsSystem
	{
		public List<PhysicalParticle> Particles = new List<PhysicalParticle>();
		public List<IPhysicalConstraint> Constraints = new List<IPhysicalConstraint>();


		public void AddObjects(params PhysicalParticle[] items)
		{
			Particles.AddRange(items);
		}

		public void AddConstraints(List<IPhysicalConstraint> constraints)
		{
			Constraints.AddRange(constraints);
		}
	}
}