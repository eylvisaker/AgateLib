﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry.Algorithms.CollisionDetection;

namespace AgateLib.Physics.TwoDimensions.Constraints
{
	public class CollisionConstraint : IPairConstraint
	{
		private CollisionDetector collider = new CollisionDetector();

		public double MultiplierMin => 0;
		public double MultiplierMax => double.MaxValue;

		public ConstraintType ConstraintType => ConstraintType.Inequality;

		public double Value(Tuple<PhysicalParticle, PhysicalParticle> pair)
		{
			return 0;
		}

		public bool AppliesTo(PhysicalParticle particle)
		{
			throw new NotImplementedException();
		}

		public ConstraintDerivative Derivative(PhysicalParticle particle, Tuple<PhysicalParticle, PhysicalParticle> pair)
		{
			var contactPoint = collider.FindConvexContactPoint(pair.Item1.TransformedPolygon, pair.Item2.TransformedPolygon);

			if (contactPoint.Contact == false)
				return new ConstraintDerivative();

			var sign = particle == pair.Item1 ? -1 : 1;
			var force = sign * contactPoint.PenetrationDepth;

			var r = particle == pair.Item1 ? contactPoint.FirstPolygonContactPoint : contactPoint.SecondPolygonContactPoint;
			var torque = -r.CrossProduct(force); // minus sign because r points from the center to the contact point instead of the other way.

			return new ConstraintDerivative(force.X, force.Y, torque);
		}

		public IEnumerable<IReadOnlyList<PhysicalParticle>> ApplyTo(KinematicsSystem system)
		{
			foreach (var p1 in system.Particles)
			{
				foreach (var p2 in system.Particles)
				{
					if (p2 == p1)
						continue;

					if (collider.DoPolygonsIntersect(p1.TransformedPolygon, p2.TransformedPolygon) == false)
						continue;

					yield return new List<PhysicalParticle> {p1, p2};
				}
			}
		}
	}
}
