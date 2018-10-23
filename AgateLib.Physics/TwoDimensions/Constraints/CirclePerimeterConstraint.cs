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
using Microsoft.Xna.Framework;

namespace AgateLib.Physics.TwoDimensions.Constraints
{
	/// <summary>
	/// Constrains a particle to exist on a circle.
	/// </summary>
	public class CirclePerimeterConstraint : IPhysicalConstraint
	{
		private Vector2 circlePosition;
		private float circleRadius;
		private PhysicalParticle particle;

		public CirclePerimeterConstraint(PhysicalParticle particle, Vector2 circlePosition, float circleRadius)
		{
			this.particle = particle;
			this.circlePosition = circlePosition;
			this.circleRadius = circleRadius;
		}

		public ConstraintType ConstraintType => ConstraintType.Equality;

		public float Value(IReadOnlyList<PhysicalParticle> particles) => 
			.5f * ((particle.Position - circlePosition).LengthSquared() - circleRadius * circleRadius);

		public IEnumerable<IReadOnlyList<PhysicalParticle>> ApplyTo(KinematicsSystem system)
		{
			yield return new List<PhysicalParticle> { particle };
		}

		public bool AppliesTo(PhysicalParticle particle)
		{
			return ReferenceEquals(this.particle, particle);
		}

		public ConstraintDerivative Derivative(PhysicalParticle particle)
		{
			return new ConstraintDerivative(
				particle.Position.X - circlePosition.X,
				particle.Position.Y - circlePosition.Y,
				0);
		}

		public float MultiplierMin => float.MinValue;
		public float MultiplierMax => float.MaxValue;
	}
}
