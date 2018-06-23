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
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Algorithms.Configuration;
using AgateLib.Quality;
using Microsoft.Xna.Framework;

namespace AgateLib.Mathematics.Geometry.Algorithms.CollisionDetection
{
	/// <summary>
	/// Class which can be used to detect collision between two polygons.
	/// </summary>
	public class CollisionDetector
	{
		private IterativeAlgorithm iterationControl;
		private GilbertJohnsonKeerthiAlgorithm gjk;

		public CollisionDetector(IterativeAlgorithm iterationControl = null)
		{
			this.iterationControl = new IterativeAlgorithm(50, 1e-2f);

			gjk = new GilbertJohnsonKeerthiAlgorithm(iterationControl);
		}

		public void Initialize()
		{
			gjk.Initialize();
		}

		public IterativeAlgorithm IterationControl
		{
			get => iterationControl;
			set
			{
				Require.ArgumentNotNull(value, nameof(IterationControl));

				iterationControl = value;
			}
		}

		/// <summary>
		/// Gets information about the point of contact of two intersecting polygons.
		/// </summary>
		/// <remarks>Contact points are defined as the point on each polygon that is closest
		/// to the center of the other polygon. This algorithm only works correctly for convex polygons.</remarks>
		/// <param name="polyA">The first polygon.</param>
		/// <param name="polyB">The second polygon.</param>
		/// <param name="farDistance">The distance at which the polygons will be considered far away. 
		/// This short-circuits the algorithm by checking if the bounding boxes are further than this distance.</param>
		/// <returns></returns>
		public ContactPoint FindConvexContactPoint(Polygon polyA, Polygon polyB, float farDistance = 2)
		{
			var boundingA = polyA.BoundingRect;
			var boundingB = polyB.BoundingRect;

			if (boundingA.Right < boundingB.Left - farDistance)
				return new ContactPoint { Contact = false, DistanceToContact = farDistance };
			if (boundingA.Bottom < boundingB.Top - farDistance)
				return new ContactPoint { Contact = false, DistanceToContact = farDistance };
			if (boundingB.Right < boundingA.Left - farDistance)
				return new ContactPoint { Contact = false, DistanceToContact = farDistance };
			if (boundingB.Bottom < boundingA.Top - farDistance)
				return new ContactPoint { Contact = false, DistanceToContact = farDistance };

			gjk.Initialize();

			var simplex = gjk.FindMinkowskiSimplex(polyA, polyB);

			if (simplex.ContainsOrigin == false)
			{
				return new ContactPoint { Contact = false, DistanceToContact = (simplex.ClosestA - simplex.ClosestB).Length()};
			}

			var closestA = simplex.ClosestA - polyA.Centroid;
			var closestB = simplex.ClosestB - polyB.Centroid;

			var epa = new ExpandingPolytopeAlgorithm(iterationControl);

			var pv = epa.PenetrationDepth(
				v => GilbertJohnsonKeerthiAlgorithm.PolygonSupport(polyA, v),
				v => GilbertJohnsonKeerthiAlgorithm.PolygonSupport(polyB, v),
				simplex);

			if (pv == null)
				return new ContactPoint { Contact = false };

			var edgeA = FindClosestEdge(simplex.ClosestA, polyA);
			var edgeB = FindClosestEdge(simplex.ClosestB, polyB);


			return new ContactPoint
			{
				Contact = true,

				FirstPolygon = polyA,
				SecondPolygon = polyB,

				PenetrationDepth = pv.Value,

				FirstPolygonContactPoint = closestA,
				FirstPolygonNormal = edgeA.Normal,

				SecondPolygonContactPoint = closestB,
				SecondPolygonNormal = edgeB.Normal,
			};
		}

		private PolygonEdge FindClosestEdge(Microsoft.Xna.Framework.Vector2 point, Polygon poly)
		{
			double distance = double.MaxValue;
			PolygonEdge result = null;

			foreach (var edge in poly.Edges)
			{
				var dist = Math.Abs(LineAlgorithms.SideOf(edge.LineSegment.Start, edge.LineSegment.End, point));

				if (dist < distance)
				{
					result = edge;
					distance = dist;

					if (dist <= IterationControl.Tolerance)
						break;
				}
			}

			return result;
		}

		/// <summary>
		/// Checks if two polygons intersect.
		/// </summary>
		/// <param name="polyA"></param>
		/// <param name="polyB"></param>
		/// <returns></returns>
		public bool DoPolygonsIntersect(IReadOnlyPolygon polyA, IReadOnlyPolygon polyB)
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
			IReadOnlyPolygon polyA, Vector2 offsetA,
			IReadOnlyPolygon polyB, Vector2 offsetB)
		{
			if (polyA.IsConcave || polyB.IsConcave)
			{
				// hasn't been implemented yet, bu
				// the separating axis theorem works
				// well for the "mostly" convex shapes
				// being used.

				//throw new NotImplementedException();
			}

			// first do the axis aligned bounding box test.
			if (BoundingBoxesSeparated(polyA, offsetA, polyB, offsetB))
			{
				return false;
			}

			// do the separating axis test for each edge in each polygon.
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

		private bool BoundingBoxesSeparated(IReadOnlyPolygon polyA, Vector2 offsetA, IReadOnlyPolygon polyB, Vector2 offsetB)
		{
			RectangleF a = polyA.BoundingRect;
			RectangleF b = polyB.BoundingRect;

			a.Location += offsetA;
			b.Location += offsetB;

			if (a.Left > b.Right) return true;
			if (a.Top > b.Bottom) return true;
			if (a.Bottom < b.Top) return true;
			if (a.Right < b.Left) return true;

			return false;
		}

		/// <summary>
		/// Checks to see if any of the lines in the first set of vectors groups
		/// all the vector2s in the second set of vectors entirely into one side.
		/// This algorithm can be used to determine if two convex polygons intersect.
		/// </summary>
		/// <param name="va"></param>
		/// <param name="vb"></param>
		/// <param name="offsetA"></param>
		/// <param name="offsetB"></param>
		/// <returns></returns>
		private static bool FindSeparatingAxis(
			IReadOnlyList<Microsoft.Xna.Framework.Vector2> va, Microsoft.Xna.Framework.Vector2 offsetA,
			IReadOnlyList<Microsoft.Xna.Framework.Vector2> vb, Microsoft.Xna.Framework.Vector2 offsetB)
		{
			for (int i = 0; i < va.Count; i++)
			{
				int next = i + 1;
				if (next == va.Count) next = 0;

				int nextnext = next + 1;
				if (nextnext == va.Count) nextnext = 0;

				Microsoft.Xna.Framework.Vector2 edge = va[next] - va[i];

				bool separating = true;

				// first check to see which side of the axis the vector2s in 
				// va are on, stored in the inSide variable.
				Microsoft.Xna.Framework.Vector2 indiff = va[nextnext] - va[i];

				var indot = Vector2.Dot(indiff, edge);
				int inSide = Math.Sign(indot);
				int lastSide = 0;

				for (int j = 0; j < vb.Count; j++)
				{
					Microsoft.Xna.Framework.Vector2 diff = vb[j] - va[i];
					diff += offsetB - offsetA;

					var dot = Vector2.Dot(diff, edge);
					var side = Math.Sign(dot);

					// this means points in vb are on the same side 
					// of the edge as vectors in va. Thus, it is not 
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
