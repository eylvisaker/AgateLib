//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AgateLib.Mathematics.Geometry
{
	[DataContract]
	public class Polygon
	{
		[DataMember]
		List<Vector2f> mPoints = new List<Vector2f>();

		public List<Vector2f> Points
		{
			get { return mPoints; }
		}

		public Polygon Clone()
		{
			var result = new Polygon();

			result.mPoints.AddRange(mPoints);

			return result;
		}

		public static Polygon FromRect(Rectangle rect)
		{
			Polygon result = new Polygon();

			result.AddPoint(rect.X, rect.Y);
			result.AddPoint(rect.Right, rect.Y);
			result.AddPoint(rect.Right, rect.Bottom);
			result.AddPoint(rect.X, rect.Bottom);

			return result;
		}

		private void AddPoint(int x, int y)
		{
			mPoints.Add(new Vector2f(x, y));
		}

		public Rectangle BoundingRect
		{
			get
			{
				if (Points.Count == 0)
					throw new InvalidOperationException();

				int left = int.MaxValue;
				int right = int.MinValue;
				int top = int.MaxValue;
				int bottom = int.MinValue;

				foreach (var pt in Points)
				{
					if (pt.X < left) left = (int)pt.X;
					if (pt.X > right) right = (int)pt.X;
					if (pt.Y < top) top = (int)pt.Y;
					if (pt.Y > bottom) bottom = (int)pt.Y;
				}

				return Rectangle.FromLTRB(left, top, right, bottom);
			}
		}


		#region --- Collision Checking ---

		public static bool PolysIntersect(Polygon polyA, Polygon polyB)
		{
			return PolysIntersect(polyA, Vector2f.Zero, polyB, Vector2f.Zero);
		}
		public static bool PolysIntersect(
			Polygon polyA, Vector2f offsetA,
			Polygon polyB, Vector2f offsetB)
		{
			// do the separating axis test for each edge in each square.
			if (FindSeparatingAxis(
				polyA.mPoints, offsetA,
				polyB.mPoints, offsetB))
				return false;

			if (FindSeparatingAxis(
				polyB.mPoints, offsetB,
				polyA.mPoints, offsetA))
				return false;

			return true;
		}

		/// <summary>
		/// Checks to see if any of the lines in the first set of vectors groups
		/// all the points in the second set of vectors entirely into one side.
		/// This algorithm can be used to determine if two convex polygons intersect.
		/// </summary>
		/// <param name="va"></param>
		/// <param name="vb"></param>
		/// <returns></returns>
		private static bool FindSeparatingAxis(
			List<Vector2f> va, Vector2f offsetA,
			List<Vector2f> vb, Vector2f offsetB)
		{
			for (int i = 0; i < va.Count; i++)
			{
				int next = i + 1;
				if (next == va.Count) next = 0;

				int nextnext = next + 1;
				if (nextnext == va.Count) nextnext = 0;

				Vector2f edge = va[next] - va[i];

				bool separating = true;

				// first check to see which side of the axis the points in 
				// va are on, stored in the inSide variable.
				Vector2f indiff = va[nextnext] - va[i];

				var indot = indiff.DotProduct(edge);
				int inSide = Math.Sign(indot);
				int lastSide = 0;

				for (int j = 0; j < vb.Count; j++)
				{
					Vector2f diff = vb[j] - va[i];
					diff += offsetB - offsetA;

					var dot = diff.DotProduct(edge);
					var side = Math.Sign(dot);

					// this means points in vb are on the same side 
					// of the edge as points in va. Thus, it is not 
					// a separating axis.
					if (side == inSide)
					{
						separating = false;
						break;
					}

					if (lastSide == 0)
						lastSide = side;
					else if (lastSide != side)
					{
						// if we fail here, it means the axis goes right through
						// the polygon defined in vb, so this is not a separating
						// axis.
						separating = false;
						break;
					}
				}

				if (separating)
					return true;
			}

			return false;
		}

		#endregion


		public bool IsAlignedRect
		{
			get
			{
				if (mPoints.Count != 4)
					return false;

				// TODO: actually implement this method.
				return true;
			}
		}
	}
}
