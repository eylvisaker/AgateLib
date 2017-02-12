using System.Collections.Generic;
using System.Linq;

namespace RigidBodyDynamics
{
	public class KinematicsSystem
	{
		private List<PhysicalParticle> particles = new List<PhysicalParticle>();
		private List<IPhysicalConstraint> constraints = new List<IPhysicalConstraint>();

		public IReadOnlyList<PhysicalParticle> Particles => particles;

		public IReadOnlyList<IPhysicalConstraint> Constraints => constraints;

		public float LinearKineticEnergy => particles.Sum(p => .5f * p.Mass * p.Velocity.MagnitudeSquared);

		public float AngularKineticEnergy
			=> particles.Sum(p => .5f * p.InertialMoment * p.AngularVelocity * p.AngularVelocity);

		public void AddParticles(params PhysicalParticle[] items)
		{
			foreach (var item in items)
			{
				if (item.Mass == 0)
					item.Mass = 1;

				if (item.InertialMoment == 0)
					item.InertialMoment = 1;

				particles.Add(item);
			}
		}

		public void AddConstraints(params IPhysicalConstraint[] constraints)
		{
			this.constraints.AddRange(constraints);
		}
	}
}