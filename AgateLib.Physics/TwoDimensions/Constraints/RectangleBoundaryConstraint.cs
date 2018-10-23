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
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Algorithms;
using Microsoft.Xna.Framework;

namespace AgateLib.Physics.TwoDimensions.Constraints
{
	public class RectangleBoundaryConstraint : IPhysicalConstraint
	{
		public RectangleBoundaryConstraint(Rectangle bounds)
		{
			Bounds = bounds;
			InnerBounds = bounds.Contract(bounds.Width / 8);
		}

		public float MultiplierMin => float.MinValue;

		public float MultiplierMax => 0;

		public ConstraintType ConstraintType => ConstraintType.Inequality;

		public Rectangle InnerBounds { get; private set; }

		public Rectangle Bounds { get; private set; }

		public float Magnitude { get; set; } = 100;

		public float Value(IReadOnlyList<PhysicalParticle> particles)
		{
			//double x = double.MinValue;
			//double y = double.MinValue;

			//foreach (var point in particles.First().TransformedPolygon)
			//{
			//	x = Math.Max(x, Bounds.Left - point.X);
			//	x = Math.Max(x, point.X - Bounds.Right);

			//	y = Math.Max(y, Bounds.Top - point.Y);
			//	y = Math.Max(y, point.Y - Bounds.Bottom);
			//}

			//if (x > 0 && y > 0)
			//	return x + y;
			//else
			//	return Math.Max(x, y);

			return particles.First().TransformedPolygon
					   .Max(pt => pt.Y) - Bounds.Bottom;
		}

		public bool AppliesTo(PhysicalParticle particle)
		{
			return true;
		}

		public ConstraintDerivative Derivative(PhysicalParticle particle)
		{
			float dx = 0;
			float  dy = 0;
			float dphi = 0;

			int pointCount = 0;

			foreach (var point in particle.TransformedPolygon)
			{
				var magnitude = (point - particle.Position).Length();
				var angle = (point - particle.Position).Angle();

				//if (rotatedPoint.X < Bounds.Left)
				//{
				//	dx -= 1;
				//	dphi += -Math.Sin(particle.Angle + angle);
				//}
				//if (rotatedPoint.X > Bounds.Right)
				//{
				//	dx += 1;
				//	dphi += -Math.Sin(particle.Angle + angle);
				//}

				//if (rotatedPoint.Y < Bounds.Top)
				//{
				//	dy -= 1;
				//	dphi += Math.Cos(particle.Angle + angle);
				//}
				if (point.Y > Bounds.Bottom)
				{
					pointCount++;

					dy += 1;
					dphi -= magnitude * (float)Math.Cos(angle);
				}
			}

			return new ConstraintDerivative(dx, dy, dphi);
		}

		public IEnumerable<IReadOnlyList<PhysicalParticle>> ApplyTo(KinematicsSystem system)
		{
			List<PhysicalParticle> result = new List<PhysicalParticle>();

			result.Add(null);

			foreach (var particle in system.Particles)
			{
				result[0] = particle;
				yield return result;
			}
		}

	}
}
