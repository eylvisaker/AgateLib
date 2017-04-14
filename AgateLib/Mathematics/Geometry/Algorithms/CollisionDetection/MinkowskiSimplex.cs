using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace AgateLib.Mathematics.Geometry.Algorithms.CollisionDetection
{
	public class MinkowskiSimplex : IEnumerable<Vector2>
	{
		private bool dirty;
		private Vector2 closestA;
		private Vector2 closestB;

		public Polygon Simplex = new Polygon();
		public double DistanceFromOrigin;

		public List<Vector2> ShapeA = new List<Vector2>();
		public List<Vector2> ShapeB = new List<Vector2>();

		/// <summary>
		/// True indicates that the two polygons are overlapping.
		/// </summary>
		public bool ContainsOrigin => Simplex.AreaContains(Vector2.Zero);

		public Vector2 ClosestA
		{
			get
			{
				if (dirty)
					ComputeClosestPoints();

				return closestA;
			}
		}

		public Vector2 ClosestB
		{
			get
			{
				if (dirty)
					ComputeClosestPoints();

				return closestB;
			}
		}


		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<Vector2> GetEnumerator()
		{
			return Simplex.GetEnumerator();
		}

		internal static SupportData Support(Func<Vector2, Vector2> supportA, Func<Vector2, Vector2> supportB, Vector2 v)
		{
			var sa = supportA(v);
			var sb = supportB(-v);

			var result = new SupportData
			{
				SupportA = sa,
				SupportB = sb,
			};

			return result;
		}

		internal void Add(SupportData support)
		{
			dirty = true;

			Simplex.Add(support.Difference);
			Debug.Assert(Simplex.Count <= 3);

			AddSupportPoints(support.SupportA, support.SupportB);
		}

		/// <summary>
		/// Used by the GJK algorithm to gradually walk a triangle into the final position.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="supportData"></param>
		internal void StaggerInsert(int index, SupportData supportData)
		{
			dirty = true;

			if (index > 0 && Simplex.Count > 0)
			{
				ShapeA[0] = ShapeA[index];
				ShapeB[0] = ShapeB[index];
				Simplex[0] = Simplex[index];
			}

			ShapeA[index] = supportData.SupportA;
			ShapeB[index] = supportData.SupportB;
			Simplex[index] = supportData.Difference;
		}

		internal void Insert(int index, SupportData supportData)
		{
			dirty = true;

			ShapeA.Insert(index, supportData.SupportA);
			ShapeB.Insert(index, supportData.SupportB);
			Simplex.Insert(index, supportData.Difference);
		}

		private void AddSupportPoints(Vector2 supportA, Vector2 supportB)
		{
			if (ShapeA.Count == Simplex.Count && Simplex.Count > 0)
			{
				ShapeA.RemoveAt(0);
				ShapeB.RemoveAt(0);
			}

			ShapeA.Add(supportA);
			ShapeB.Add(supportB);
		}

		private void ComputeClosestPoints()
		{
			var A = Simplex[Simplex.Count - 2];
			var B = Simplex[Simplex.Count - 1];
			var L = B - A;

			var lambda2 = -L.DotProduct(A) / L.MagnitudeSquared;
			var lambda1 = 1 - lambda2;

			closestA = lambda1 * ShapeA[Simplex.Count - 2] + lambda2 * ShapeA[Simplex.Count - 1];
			closestB = lambda1 * ShapeB[Simplex.Count - 2] + lambda2 * ShapeB[Simplex.Count - 1];
		}
	}
}