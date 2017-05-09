using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry.Algorithms.Configuration;

namespace AgateLib.Mathematics.Geometry.Algorithms.CollisionDetection
{
	public class ExpandingPolytopeAlgorithm
	{
		class Edge
		{
			public Vector2 Normal { get; set; }
			public double Distance { get; set; }
			public int Index { get; set; }
		}


		private int iterations;

		private double Tolerance => IterationControl.Tolerance;
		private int MaxIterations => IterationControl.MaxIterations;

		public int Iterations => iterations;

		public IterativeAlgorithm IterationControl { get; } = new IterativeAlgorithm();

		public Vector2? PenetrationDepth(Func<Vector2, Vector2> supportA, Func<Vector2, Vector2> supportB, MinkowskiSimplex simplex)
		{
			iterations = 0;

			while (iterations < MaxIterations)
			{
				iterations++;

				Edge e = FindClosestEdge(simplex.Simplex);
				var support = MinkowskiSimplex.Support(supportA, supportB, e.Normal);
				Vector2 p = support.Difference;
				double d = p.DotProduct(e.Normal);

				if (d - e.Distance < Tolerance)
				{
					return d * e.Normal;
				}

				simplex.Insert(e.Index, support);
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

				Vector2 oa = a;

				Vector2 n = Vector2.TripleProduct(e, oa, e);

				var normalized = n.Normalize();

				double d = normalized.DotProduct(a);

				if (n.IsZero || d < closest.Distance)
				{
					if (n.IsZero)
					{
						d = 0;
						normalized = n;
					}

					closest.Distance = d;
					closest.Normal = normalized;
					closest.Index = j;
				}
			}

			return closest;
		}
	}
}
