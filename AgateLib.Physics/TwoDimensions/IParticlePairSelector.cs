using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Physics.TwoDimensions
{
	public interface IParticlePairSelector
	{
		IEnumerable<Tuple<PhysicalParticle, PhysicalParticle>> SelectPairs(KinematicsSystem system);
	}

	public class AllPairs : IParticlePairSelector
	{
		public IEnumerable<Tuple<PhysicalParticle, PhysicalParticle>> SelectPairs(KinematicsSystem system)
		{
			for (int i = 0; i < system.Particles.Count; i++)
			{
				for (int j = i + 1; j < system.Particles.Count; j++)
				{
					yield return new Tuple<PhysicalParticle, PhysicalParticle>(system.Particles[i], system.Particles[j]);
				}
			}
		}
	}
}
