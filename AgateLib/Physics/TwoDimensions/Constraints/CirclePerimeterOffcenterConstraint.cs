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
using AgateLib.Mathematics;
using Microsoft.Xna.Framework;

namespace AgateLib.Physics.TwoDimensions.Constraints
{
	/// <summary>
	/// Constrains a particle to exist on a circle.
	/// </summary>
	public class CirclePerimeterOffcenterConstraint : IPhysicalConstraint
	{
		private Vector2 circlePosition;
		private float circleRadius;
		private PhysicalParticle particle;
		private Vector2 offset;

		public CirclePerimeterOffcenterConstraint(PhysicalParticle particle, Vector2 circlePosition, float circleRadius, Vector2 offset)
		{
			this.particle = particle;
			this.circlePosition = circlePosition;
			this.circleRadius = circleRadius;
			this.offset = offset;
		}

		public float MultiplierMin => float.MinValue;
		public float MultiplierMax => float.MaxValue;

		public ConstraintType ConstraintType => ConstraintType.Equality;

		private Vector2 ConstrainedPointLocalPosition => offset.Rotate(particle.Angle);

		/// <summary>
		/// Returns the position of the constrained point on the particle, relative to the circle's center.
		/// </summary>
		/// <remarks>\f$\vec{r}\f$ from the math.</remarks>
		private Vector2 ConstrainedPointPosition => particle.Position + ConstrainedPointLocalPosition - circlePosition;

		private Vector2 OffsetDerivative => offset.Rotate(particle.Angle + (float)Math.PI * .5f);

		public float Value(IReadOnlyList<PhysicalParticle> particles) => .5f * (ConstrainedPointPosition.LengthSquared() - circleRadius * circleRadius);

		public bool AppliesTo(PhysicalParticle particle)
		{
			return ReferenceEquals(this.particle, particle);
		}

		public ConstraintDerivative Derivative(PhysicalParticle particle)
		{
			return new ConstraintDerivative(
				ConstrainedPointPosition.X,
				ConstrainedPointPosition.Y,
				Vector2.Dot(ConstrainedPointPosition, OffsetDerivative));
		}

		public IEnumerable<IReadOnlyList<PhysicalParticle>> ApplyTo(KinematicsSystem system)
		{
			yield return new List<PhysicalParticle> { particle };
		}

	}
}
