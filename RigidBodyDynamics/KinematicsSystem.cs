using System.Collections.Generic;

namespace RigidBodyDynamics
{
	public class KinematicsSystem
	{
		public List<PhysicalParticle> Particles = new List<PhysicalParticle>();
		public List<IPhysicalConstraint> Constraints = new List<IPhysicalConstraint>();


		public void AddObjects(params PhysicalParticle[] items)
		{
			foreach (var item in items)
			{
				if (item.Mass == 0)
					item.Mass = 1;

				if (item.InertialMoment == 0)
					item.InertialMoment = 1;

				Particles.Add(item);
			}
		}

		public void AddConstraints(List<IPhysicalConstraint> constraints)
		{
			Constraints.AddRange(constraints);
		}
	}
}