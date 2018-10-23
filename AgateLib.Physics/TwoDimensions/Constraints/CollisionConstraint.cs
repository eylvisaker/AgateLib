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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry.Algorithms.CollisionDetection;
using Microsoft.Xna.Framework;

namespace AgateLib.Physics.TwoDimensions.Constraints
{
	public class CollisionConstraint : IPairConstraint
	{
		private CollisionDetector collider = new CollisionDetector();

		public float MultiplierMin => 0;
		public float MultiplierMax => float.MaxValue;
		
		public ConstraintType ConstraintType => ConstraintType.Inequality;

		public float Value(Tuple<PhysicalParticle, PhysicalParticle> pair)
		{
			var contactPoint = collider.FindConvexContactPoint(pair.Item1.TransformedPolygon, pair.Item2.TransformedPolygon);

			if (contactPoint.Contact == false)
				return 0;

			var springConstant = 30;

			var a = ((contactPoint.SecondPolygon.Centroid + contactPoint.SecondPolygonContactPoint) -
			         (contactPoint.FirstPolygon.Centroid + contactPoint.FirstPolygonContactPoint));
			var b = contactPoint.FirstPolygonNormal;

			return springConstant * Vector2.Dot(a, b);
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

			var firstParticle = particle == pair.Item1;

			var sign = firstParticle ? -1 : 1;
			var force = sign * contactPoint.FirstPolygonNormal;

			var r = firstParticle ? contactPoint.FirstPolygonContactPoint : contactPoint.SecondPolygonContactPoint;
			var torque = -Vector2X.Cross(r, force); // minus sign because r points from the center to the contact point instead of the other way.

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

					yield return new List<PhysicalParticle> { p1, p2 };
				}
			}
		}
	}
}
