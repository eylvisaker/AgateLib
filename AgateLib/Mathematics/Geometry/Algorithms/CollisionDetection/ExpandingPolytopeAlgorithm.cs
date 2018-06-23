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
using AgateLib.Mathematics.Geometry.Algorithms.Configuration;
using Microsoft.Xna.Framework;

namespace AgateLib.Mathematics.Geometry.Algorithms.CollisionDetection
{
	public class ExpandingPolytopeAlgorithm
	{
		class Edge
		{
			public Microsoft.Xna.Framework.Vector2 Normal { get; set; }
			public double Distance { get; set; }
			public int Index { get; set; }
		}

		private int iterations;

		public ExpandingPolytopeAlgorithm(IterativeAlgorithm iterationControl = null)
		{
			IterationControl = iterationControl ?? new IterativeAlgorithm();
		}

		private double Tolerance => IterationControl.Tolerance;
		private int MaxIterations => IterationControl.MaxIterations;

		public int Iterations => iterations;

		public IterativeAlgorithm IterationControl { get; }

		public Microsoft.Xna.Framework.Vector2? PenetrationDepth(Func<Microsoft.Xna.Framework.Vector2, Microsoft.Xna.Framework.Vector2> supportA, Func<Microsoft.Xna.Framework.Vector2, Microsoft.Xna.Framework.Vector2> supportB, MinkowskiSimplex simplex)
		{
			iterations = 0;

			while (iterations < MaxIterations)
			{
				iterations++;

				Edge e = FindClosestEdge(simplex.Simplex);
				var support = MinkowskiSimplex.Support(supportA, supportB, e.Normal);
				Vector2 p = support.Difference;
				float d = Vector2.Dot(p, e.Normal);

				if (d - e.Distance < Tolerance)
				{
					return d * e.Normal;
				}

				simplex.Insert(e.Index, support);
			}

			return null;
		}

		private static Microsoft.Xna.Framework.Vector2 Support(Func<Microsoft.Xna.Framework.Vector2, Microsoft.Xna.Framework.Vector2> supportA, Func<Microsoft.Xna.Framework.Vector2, Microsoft.Xna.Framework.Vector2> supportB, Microsoft.Xna.Framework.Vector2 v)
		{
			var sa = supportA(v);
			var sb = supportB(-v);

			var w = sa - sb;
			return w;
		}

		/// <summary>
		/// Finds the edge of the simplex that is closest to the origin.
		/// </summary>
		/// <param name="simplex"></param>
		/// <returns></returns>
		private Edge FindClosestEdge(Polygon simplex)
		{
			Edge closest = new Edge();

			closest.Distance = double.MaxValue;

			for (int i = 0; i < simplex.Count; i++)
			{
				int j = (i + 1) % simplex.Count;

				Vector2 a = simplex[i];
				Vector2 b = simplex[j];

				Vector2 e = b - a;

				Vector2 n = TripleProduct(e, a, e);

				var normalized = n;
				normalized.Normalize();

				float d = Vector2.Dot(normalized, a);
				var zeroNormal = n.LengthSquared() < IterationControl.Tolerance;

				if (zeroNormal)
				{
					closest.Distance = 0;
					closest.Normal = Vector2.Zero;
					closest.Index = j;
					break;
				}

				if (d < closest.Distance)
				{
					closest.Distance = d;
					closest.Normal = normalized;
					closest.Index = j;
				}
			}

			return closest;
		}

		private Vector2 TripleProduct(Vector2 a, Vector2 b, Vector2 c)
		{
			double adotc = Vector2.Dot(a, c);
			double adotb = Vector2.Dot(b, c);

			double x = b.X * adotc - c.X * adotb;
			double y = b.Y * adotc - c.Y * adotb;
			
			return new Vector2((float)x, (float)y);
		}
	}
}
