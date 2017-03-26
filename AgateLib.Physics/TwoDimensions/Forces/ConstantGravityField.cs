using System;
using System.Collections.Generic;
using AgateLib.Mathematics;

namespace AgateLib.Physics.TwoDimensions.Forces
{
	/// <summary>
	/// Implements a "planet surface" gravity force, that is, a constant force field that points in a single direction.
	/// </summary>
	public class ConstantGravityField : IForce
	{
		private Vector2 direction = Vector2.UnitY;

		/// <summary>
		/// The amount of acceleration due to gravity.
		/// </summary>
		public double GravityValue { get; set; } = 1000;

		/// <summary>
		/// Defaults to the +Y direction, which is down in AgateLib's usual coordinate system.
		/// </summary>
		public Vector2 Direction
		{
			get { return direction; }
			set
			{
				if (direction.Magnitude < 1e-8)
					throw new DivideByZeroException();

				direction = value.Normalize();
			}
		}

		/// <summary>
		/// Adds the force to the accumulated value.
		/// </summary>
		/// <param name="particles"></param>
		public void AccumulateForce(IEnumerable<PhysicalParticle> particles)
		{
			foreach (var particle in particles)
			{
				particle.Force += direction * GravityValue * particle.Mass;
			}
		}
	}
}
