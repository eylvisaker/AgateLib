using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Mathematics.Geometry.Algorithms.CollisionDetection
{
	public class ExpandingPolytopeAlgorithm
	{
		private int iterations;

		private double Tolerance => IterationControl.Tolerance;
		private int MaxIterations => IterationControl.MaxIterations;

		public int Iterations => iterations;

		public IterativeAlgorithm IterationControl { get; } = new IterativeAlgorithm();

		class Edge
		{
			public Vector2 Normal { get; set; }
			public double Distance { get; set; }
			public int Index { get; set; }
		}

		public Vector2? PenetrationDepth(Func<Vector2, Vector2> supportA, Func<Vector2, Vector2> supportB, Polygon simplex)
		{
			iterations = 0;

			while (iterations < MaxIterations)
			{
				iterations++;

				Edge e = FindClosestEdge(simplex);
				Vector2 p = Support(supportA, supportB, e.Normal);
				double d = p.DotProduct(e.Normal);

				if (d - e.Distance < Tolerance)
				{
					return d * e.Normal;
				}

				simplex.Insert(e.Index, p);
			}

			return null;
		}

		private static Vector2 Support(Func<Vector2, Vector2> supportA, Func<Vector2, Vector2> supportB, Vector2 v)
		{
			var sa = supportA(v);
			var sb = supportB(-v);

			var w = sa - sb;
			return w;
		}

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

				Vector2 oa = a;

				Vector2 n = Vector2.TripleProduct(e, oa, e);

				n = n.Normalize();

				double d = n.DotProduct(a);

				if (d < closest.Distance)
				{
					closest.Distance = d;
					closest.Normal = n;
					closest.Index = j;
				}
			}

			return closest;
		}
	}
}
