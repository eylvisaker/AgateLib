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

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

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
		public float GravityValue { get; set; } = 1000;

		/// <summary>
		/// Defaults to the +Y direction, which is down in AgateLib's usual coordinate system.
		/// </summary>
		public Vector2 Direction
		{
			get => direction;
			set
			{
				if (direction.LengthSquared() < 1e-8f)
					throw new DivideByZeroException();

				value.Normalize();
				direction = value;
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
