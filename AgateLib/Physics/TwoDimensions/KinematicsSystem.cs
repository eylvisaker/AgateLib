//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

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

		public double LinearKineticEnergy => particles.Sum(p => .5 * p.Mass * p.Velocity.LengthSquared());

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