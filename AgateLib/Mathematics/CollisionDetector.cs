using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Mathematics
{
	/// <summary>
	/// Class which can be used to detect collision between two polygons.
	/// </summary>
	public class CollisionDetector
	{
		/// <summary>
		/// Checks if two polygons intersect.
		/// </summary>
		/// <param name="polyA"></param>
		/// <param name="polyB"></param>
		/// <returns></returns>
		public bool DoPolygonsIntersect(Polygon polyA, Polygon polyB)
		{
			return DoPolygonsIntersect(polyA, Vector2.Zero, polyB, Vector2.Zero);
		}

		/// <summary>
		/// Checks if two polygons intersect.
		/// </summary>
		/// <param name="polyA"></param>
		/// <param name="offsetA"></param>
		/// <param name="polyB"></param>
		/// <param name="offsetB"></param>
		/// <returns></returns>
		public bool DoPolygonsIntersect(
			Polygon polyA, Vector2 offsetA,
			Polygon polyB, Vector2 offsetB)
		{
			if (polyA.IsConcave || polyB.IsConcave)
			{
				throw new NotImplementedException();
			}

			// do the separating axis test for each edge in each square.
			if (FindSeparatingAxis(
				polyA.Points, offsetA,
				polyB.Points, offsetB))
				return false;

			if (FindSeparatingAxis(
				polyB.Points, offsetB,
				polyA.Points, offsetA))
				return false;

			return true;
		}

		/// <summary>
		/// Checks to see if any of the lines in the first set of vectors groups
		/// all the vector2s in the second set of vectors entirely into one side.
		/// This algorithm can be used to determine if two convex polygons intersect.
		/// </summary>
		/// <param name="va"></param>
		/// <param name="vb"></param>
		/// <returns></returns>
		private static bool FindSeparatingAxis(
			IList<Vector2> va, Vector2 offsetA,
			IList<Vector2> vb, Vector2 offsetB)
		{
			for (int i = 0; i < va.Count; i++)
			{
				int next = i + 1;
				if (next == va.Count) next = 0;

				int nextnext = next + 1;
				if (nextnext == va.Count) nextnext = 0;

				Vector2 edge = va[next] - va[i];

				bool separating = true;

				// first check to see which side of the axis the vector2s in 
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

					// this means vector2s in vb are on the same side 
					// of the edge as vector2s in va. Thus, it is not 
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
	}
}
