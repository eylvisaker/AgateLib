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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace AgateLib.Mathematics.Geometry.Algorithms.CollisionDetection
{
	public class MinkowskiSimplex : IEnumerable<Microsoft.Xna.Framework.Vector2>
	{
		private bool dirty;
		private Microsoft.Xna.Framework.Vector2 closestA;
		private Microsoft.Xna.Framework.Vector2 closestB;

		public Polygon Simplex = new Polygon();
		public double DistanceFromOrigin;

		public List<Microsoft.Xna.Framework.Vector2> ShapeA = new List<Microsoft.Xna.Framework.Vector2>();
		public List<Microsoft.Xna.Framework.Vector2> ShapeB = new List<Microsoft.Xna.Framework.Vector2>();

		public MinkowskiSimplex()
		{
			Initialize();
		}

		internal void Initialize()
		{
			Simplex.Points.Clear();
			ShapeA.Clear();
			ShapeB.Clear();
			DistanceFromOrigin = 0;
		}

		/// <summary>
		/// True indicates that the two polygons are overlapping.
		/// </summary>
		public bool ContainsOrigin => Simplex.AreaContains(Microsoft.Xna.Framework.Vector2.Zero);

		public Microsoft.Xna.Framework.Vector2 ClosestA
		{
			get
			{
				if (dirty)
					ComputeClosestPoints();

				return closestA;
			}
		}

		public Microsoft.Xna.Framework.Vector2 ClosestB
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

		public IEnumerator<Microsoft.Xna.Framework.Vector2> GetEnumerator()
		{
			return Simplex.GetEnumerator();
		}

		internal static SupportData Support(Func<Microsoft.Xna.Framework.Vector2, Microsoft.Xna.Framework.Vector2> supportA, Func<Microsoft.Xna.Framework.Vector2, Microsoft.Xna.Framework.Vector2> supportB, Microsoft.Xna.Framework.Vector2 v)
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

		private void AddSupportPoints(Microsoft.Xna.Framework.Vector2 supportA, Microsoft.Xna.Framework.Vector2 supportB)
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

			var lambda2 = -Vector2.Dot(L, A) / L.LengthSquared();
			var lambda1 = 1 - lambda2;

			closestA = lambda1 * ShapeA[Simplex.Count - 2] + lambda2 * ShapeA[Simplex.Count - 1];
			closestB = lambda1 * ShapeB[Simplex.Count - 2] + lambda2 * ShapeB[Simplex.Count - 1];
		}
	}
}