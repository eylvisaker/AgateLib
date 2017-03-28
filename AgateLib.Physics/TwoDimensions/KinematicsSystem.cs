using System.Collections.Generic;
using System.Linq;

namespace AgateLib.Physics.TwoDimensions
{
	public class KinematicsSystem
	{
		private readonly List<PhysicalParticle> particles = new List<PhysicalParticle>();
		private readonly List<IPhysicalConstraint> constraints = new List<IPhysicalConstraint>();
		private readonly List<IForce> forces = new List<IForce>();

		public IReadOnlyList<PhysicalParticle> Particles => particles;

		public IReadOnlyList<IPhysicalConstraint> Constraints => constraints;

		public IReadOnlyList<IForce> Forces => forces;

		public double LinearKineticEnergy => particles.Sum(p => .5 * p.Mass * p.Velocity.MagnitudeSquared);

		public double AngularKineticEnergy
			=> particles.Sum(p => .5 * p.InertialMoment * p.AngularVelocity * p.AngularVelocity);

		public void AddParticles(IEnumerable<PhysicalParticle> items)
		{
			foreach (var item in items)
			{
				particles.Add(item);
			}
		}

		public void AddParticles(params PhysicalParticle[] items)
		{
			foreach (var item in items)
			{
				particles.Add(item);
			}
		}

		public void AddConstraints(params IPhysicalConstraint[] physicalConstraints)
		{
			constraints.AddRange(physicalConstraints);
		}

		public void AddForceField(IForce force)
		{
			forces.Add(force);
		}

		/// <summary>
		/// Removes all the particles from the system.
		/// </summary>
		public void ClearParticles()
		{
			particles.Clear();
		}
	}
}