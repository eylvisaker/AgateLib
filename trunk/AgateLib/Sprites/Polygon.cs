using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.Sprites
{
	public class Polygon
	{
		List<Vector2> mPoints = new List<Vector2>();

		public List<Vector2> Points
		{
			get { return mPoints; }
		}

		public Polygon Clone()
		{
			var retval = new Polygon();

			retval.mPoints.AddRange(mPoints);

			return retval;
		}

		public static Polygon FromRect(Rectangle rect)
		{
			Polygon retval = new Polygon();

			retval.AddPoint(rect.X, rect.Y);
			retval.AddPoint(rect.Right, rect.Y);
			retval.AddPoint(rect.Right, rect.Bottom);
			retval.AddPoint(rect.X, rect.Bottom);

			return retval;
		}

		private void AddPoint(int x, int y)
		{
			mPoints.Add(new Vector2(x, y));
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
			return PolysIntersect(polyA, Vector2.Empty, polyB, Vector2.Empty);
		}
		public static bool PolysIntersect(
			Polygon polyA, Vector2 offsetA,
			Polygon polyB, Vector2 offsetB)
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
			List<Vector2> va, Vector2 offsetA,
			List<Vector2> vb, Vector2 offsetB)
		{
			for (int i = 0; i < va.Count; i++)
			{
				int next = i + 1;
				if (next == va.Count) next = 0;

				int nextnext = next + 1;
				if (nextnext == va.Count) nextnext = 0;

				Vector2 edge = va[next] - va[i];

				bool separating = true;

				// first check to see which side of the axis the points in 
				// va are on, stored in the inSide variable.
				Vector2 indiff = va[nextnext] - va[i];

				var indot = indiff.DotProduct(edge);
				int inSide = Math.Sign(indot);
				int lastSide = 0;

				for (int j = 0; j < vb.Count; j++)
				{
					Vector2 diff = vb[j] - va[i];
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
