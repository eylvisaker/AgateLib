﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;

namespace AgateLib.Physics.Constraints
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

		public double Value(IReadOnlyList<PhysicalParticle> particles) => 
			.5 * ((particle.Position - circlePosition).MagnitudeSquared - circleRadius * circleRadius);

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
	}
}
