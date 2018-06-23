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

using System.Collections.Generic;
using System.Linq;

namespace AgateLib.Mathematics.Geometry.Algorithms.ConvexDecomposition
{
	/// <summary>
	/// This implements Bayazit's polygon decomposition algorithm.
	/// https://mpen.ca/406/bayazit
	/// </summary>
	/// <remarks>
	/// Convex decomposition algorithm originally created by Mark Bayazit (http://mnbayazit.com/)
	/// For more information about this algorithm, see http://mnbayazit.com/406/bayazit
	/// But modified by Yogesh (http://yogeshkulkarni.com)
	/// Adapted from Mark Penner's repository at https://bitbucket.org/mnpenner/bayazit-yogesh-poly-decomp
	/// to use AgateLib mathematics libraries.
	/// </remarks>
	public class BayazitDecomposition : IPolygonConvexDecompositionAlgorithm
	{
		public int MaxPolygonVertices = 9999;

		/// <summary>
		/// Decomposes a concave polygon into a set of convex polygons.
		/// </summary>
		/// <param name="polygon"></param>
		/// <returns></returns>
		public IReadOnlyList<Polygon> BuildConvexDecomposition(Polygon polygon)
		{
			//We force it to CCW as it is a precondition in this algorithm.
			polygon = ForceCounterClockWise(polygon);

			List<Polygon> list = new List<Polygon>();

			// YOGESH : Convex Partition can not happen if there are less than 3, ie 2,1 vertices
			if (polygon.Count < 3)
				return list;

			double d = 0.0;
			double lowerDist, upperDist;
			Microsoft.Xna.Framework.Vector2 p;
			Microsoft.Xna.Framework.Vector2 lowerInt = new Microsoft.Xna.Framework.Vector2();
			Microsoft.Xna.Framework.Vector2 upperInt = new Microsoft.Xna.Framework.Vector2(); // intersection points
			int lowerIndex = 0, upperIndex = 0;
			Polygon lowerPoly, upperPoly;

			// Go thru all Verices until  we  find  a  reflex  vertex  i.  
			// Extend  the  edges incident at i until they hit an edge
			// Find BEST vertex within the range, that the partitioning chord

			// A polygon can be broken into convex regions by eliminating all reflex vertices
			// Eliminating two reflex vertices with one diagonal is better than eliminating just one
			// A reflex vertex can only be removed if the diagonal connecting to it is within the range given by extending its neighbouring edges; 
			// otherwise, its angle is only reduced
			for (int i = 0; i < polygon.Count; i++)
			{
				Microsoft.Xna.Framework.Vector2 prev = polygon.At(i - 1);
				Microsoft.Xna.Framework.Vector2 on = polygon.At(i);
				Microsoft.Xna.Framework.Vector2 next = polygon.At(i + 1);

				if (IsReflex(prev, on, next))
				{
					lowerDist = double.MaxValue;
					upperDist = double.MaxValue;

					for (int j = 0; j < polygon.Count; j++)
					{
						// YOGESH: if any of j line's endpoints matches with reflex i, skip
						if ((i == j) 
							|| (i == NormalizeIndex(j - 1, polygon.Count)) 
							|| (i == NormalizeIndex(j + 1, polygon.Count)))
						{
							continue; // no self and prev and next, for testing
						}

						// testing incoming edge:
						// if line coming into i vertex (i-1 to i) has j vertex of the test-line on left
						// AND have j-i on right, then they will be intersecting
						Microsoft.Xna.Framework.Vector2 iPrev = polygon.At(i - 1);
						Microsoft.Xna.Framework.Vector2 iSelf = polygon.At(i);
						Microsoft.Xna.Framework.Vector2 jSelf = polygon.At(j);
						Microsoft.Xna.Framework.Vector2 jPrev = polygon.At(j - 1);

						bool leftOK = Left(iPrev, iSelf, jSelf);
						bool rightOK = Right(iPrev, iSelf, jPrev);

						bool leftOnOK = LineAlgorithms.AreCollinear(iPrev, iSelf, jSelf); // YOGESH: cached into variables for better debugging
						bool rightOnOK = LineAlgorithms.AreCollinear(iPrev, iSelf, jPrev); // YOGESH: cached into variables for better debugging

						if (leftOnOK || rightOnOK) // YOGESH: Checked "ON" condition as well, collinearity
						{
							// lines are colinear, they can not be overlapping as polygon is simple
							// find closest point which is not internal to incoming line i , i -1
							d = Microsoft.Xna.Framework.Vector2.DistanceSquared(iSelf, jSelf);

							// this lower* is the point got from incoming edge into the i vertex,
							// lowerInt incoming edge intersection point
							// lowerIndex incoming edge intersection edge
							if (d < lowerDist)
							{
								// keep only the closest intersection
								lowerDist = d;
								lowerInt = jSelf;
								lowerIndex = j - 1;
							}

							d = Microsoft.Xna.Framework.Vector2.DistanceSquared(iSelf, jPrev);

							// this lower* is the point got from incoming edge into the i vertex,
							// lowerInt incoming edge intersection point
							// lowerIndex incoming edge intersection edge
							if (d < lowerDist)
							{
								// keep only the closest intersection
								lowerDist = d;
								lowerInt = jPrev;
								lowerIndex = j;
							}
						}
						else if (leftOK && rightOK) // YOGESH: Intersection in-between. Bayazit had ON condition in built here, which I have taken care above.
						{
							// find the point of intersection
							var intersection = LineAlgorithms.LineSegmentIntersection(
								polygon.At(i - 1), polygon.At(i), polygon.At(j), polygon.At(j - 1))
								?.IntersectionPoint;

							if (intersection != null)
							{
								p = intersection.Value;

								// make sure it's inside the poly, 
								if (Right(polygon.At(i + 1), polygon.At(i), p))
								{
									d = Microsoft.Xna.Framework.Vector2.DistanceSquared(polygon.At(i), p);

									// this lower* is the point got from incoming edge into the i vertex,
									// lowerInt incoming edge intersection point
									// lowerIndex incoming edge intersection edge
									if (d < lowerDist)
									{
										// keep only the closest intersection
										lowerDist = d;
										lowerInt = p;
										lowerIndex = j;
									}
								}
							}
						}

						// testing outgoing edge:
						// if line outgoing from i vertex (i to i+1) has j vertex of the test-line on right
						// AND has j+1 on left, they they will be intersecting
						Microsoft.Xna.Framework.Vector2 iNext = polygon.At(i + 1);
						Microsoft.Xna.Framework.Vector2 jNext = polygon.At(j + 1);

						bool leftOKn = Left(iNext, iSelf, jNext);
						bool rightOKn = Right(iNext, iSelf, jSelf);

						bool leftOnOKn = LineAlgorithms.AreCollinear(iNext, iSelf, jNext); // YOGESH: cached into variables for better debugging
						bool rightOnOKn = LineAlgorithms.AreCollinear(iNext, iSelf, jSelf);

						if (leftOnOKn || rightOnOKn)// YOGESH: Checked "ON" condition as well, collinearity
						{
							// lines are colinear, they can not be overlapping as polygon is simple
							// find closest point which is not internal to incoming line i , i -1
							d = Microsoft.Xna.Framework.Vector2.DistanceSquared(iSelf, jNext);

							// this upper* is the point got from outgoing edge into the i vertex,
							// upperInt outgoing edge intersection point
							// upperIndex outgoing edge intersection edge
							if (d < upperDist)
							{
								// keep only the closest intersection
								upperDist = d;
								upperInt = jNext;
								upperIndex = j + 1;
							}

							d = Microsoft.Xna.Framework.Vector2.DistanceSquared(polygon.At(i), polygon.At(j));

							// this upper* is the point got from outgoing edge into the i vertex,
							// upperInt outgoing edge intersection point
							// upperIndex outgoing edge intersection edge
							if (d < upperDist)
							{
								// keep only the closest intersection
								upperDist = d;
								upperInt = jSelf;
								upperIndex = j;
							}
						}
						else if (leftOKn && rightOKn) // YOGESH: Intersection in-between. Bayazit had ON condition in built here, which I have taken care above.
						{
							var intersection = LineAlgorithms.LineSegmentIntersection(
						 		polygon.At(i + 1), polygon.At(i), polygon.At(j), polygon.At(j + 1))
						 		?.IntersectionPoint;

							if (intersection != null)
							{
								p = intersection.Value;

								if (Left(polygon.At(i - 1), polygon.At(i), p))
								{
									d = Microsoft.Xna.Framework.Vector2.DistanceSquared(polygon.At(i), p);

									// this upper* is the point got from outgoing edge from the i vertex,
									// upperInt outgoing edge intersection point
									// upperIndex outgoing edge intersection edge
									if (d < upperDist)
									{
										upperDist = d;
										upperIndex = j;
										upperInt = p;
									}
								}
							}
						}
					}

					// YOGESH: If no vertices in the range, lets not choose midpoint but closet point of that segment
					//// if there are no vertices to connect to, choose a point in the middle
					if (lowerIndex == (upperIndex + 1) % polygon.Count)
					{
						Microsoft.Xna.Framework.Vector2 sp = ((lowerInt + upperInt) / 2);

						lowerPoly = polygon.CopyRange(i, upperIndex);
						lowerPoly.Add(sp);
						upperPoly = polygon.CopyRange(lowerIndex, i);
						upperPoly.Add(sp);
					}
					else
					{
						//find vertex to connect to
						double highestScore = 0, bestIndex = lowerIndex;
						while (upperIndex < lowerIndex) upperIndex += polygon.Count;

						// go throuh all the vertices between the range of lower and upper
						for (int j = lowerIndex; j <= upperIndex; ++j)
						{
							if (CanSee(polygon, i, j))
							{
								double score = 1 / (Microsoft.Xna.Framework.Vector2.DistanceSquared(polygon.At(i), polygon.At(j)) + 1);

								// if another vertex is Reflex, choosing it has highest score
								Microsoft.Xna.Framework.Vector2 prevj = polygon.At(j - 1);
								Microsoft.Xna.Framework.Vector2 onj = polygon.At(j);
								Microsoft.Xna.Framework.Vector2 nextj = polygon.At(j + 1);

								if (IsReflex(prevj, onj, nextj))
								{
									if (RightOrOn(polygon.At(j - 1), polygon.At(j), polygon.At(i)) &&
										LeftOrOn(polygon.At(j + 1), polygon.At(j), polygon.At(i)))
									{
										score += 3;
									}
									else
									{
										score += 2;
									}
								}
								else
								{
									score += 1;
								}
								if (score > highestScore)
								{
									bestIndex = j;
									highestScore = score;
								}
							}
						}

						// YOGESH : Pending: if there are 2 vertices as 'bestIndex', its better to disregard both and put midpoint (M case)
						lowerPoly = polygon.CopyRange(i, (int)bestIndex);
						upperPoly = polygon.CopyRange((int)bestIndex, i);
					}

					// solve smallest poly first (SAW in Bayazit's C++ code)
					if (lowerPoly.Count < upperPoly.Count)
					{
						list.AddRange(BuildConvexDecomposition(lowerPoly));
						list.AddRange(BuildConvexDecomposition(upperPoly));
					}
					else
					{
						list.AddRange(BuildConvexDecomposition(upperPoly));
						list.AddRange(BuildConvexDecomposition(lowerPoly));

					}

					return list;
				}
			}

			// polygon is already convex
			if (polygon.Count > MaxPolygonVertices)
			{
				lowerPoly = polygon.CopyRange(0, polygon.Count / 2);
				upperPoly = polygon.CopyRange(polygon.Count / 2, 0);
				list.AddRange(BuildConvexDecomposition(lowerPoly));
				list.AddRange(BuildConvexDecomposition(upperPoly));
			}
			else
			{
				list.Add(polygon);
			}

			// The polygons are not guaranteed to be without collinear points. We remove
			// them to be sure.
			for (int i = 0; i < list.Count; i++)
			{
				list[i] = RemoveCollinearPoints(list[i]);
			}

			return list;
		}

		/// <summary>
		/// Forces the vertices to be counter clock wise order.
		/// </summary>
		private Polygon ForceCounterClockWise(Polygon p)
		{
			if (p.Count < 3)
				return p;

			if (!p.IsCounterClockwise())
				return new Polygon(p.Reverse());

			return p;
		}

		/// <summary>
		/// Performs wrap-around on an index to get it in range.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		private int NormalizeIndex(int index, int size)
		{
			int result = index < 0 ? size - (-index % size) : index % size;
			return result;
		}
		
		/// <summary>
		/// Returns true if vertex j can be seen from vertex i without any obstructions.
		/// </summary>
		/// <param name="polygon"></param>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <returns></returns>
		private bool CanSee(Polygon polygon, int i, int j)
		{
			Microsoft.Xna.Framework.Vector2 prev = polygon.At(i - 1);
			Microsoft.Xna.Framework.Vector2 on = polygon.At(i);
			Microsoft.Xna.Framework.Vector2 next = polygon.At(i + 1);

			if (IsReflex(prev, on, next))
			{
				if (LeftOrOn(polygon.At(i), polygon.At(i - 1), polygon.At(j)) &&
					RightOrOn(polygon.At(i), polygon.At(i + 1), polygon.At(j))) return false;
			}
			else
			{
				if (RightOrOn(polygon.At(i), polygon.At(i + 1), polygon.At(j)) ||
					LeftOrOn(polygon.At(i), polygon.At(i - 1), polygon.At(j))) return false;
			}

			Microsoft.Xna.Framework.Vector2 prevj = polygon.At(j - 1);
			Microsoft.Xna.Framework.Vector2 onj = polygon.At(j);
			Microsoft.Xna.Framework.Vector2 nextj = polygon.At(j + 1);

			if (IsReflex(prevj, onj, nextj))
			{
				if (LeftOrOn(polygon.At(j), polygon.At(j - 1), polygon.At(i)) &&
					RightOrOn(polygon.At(j), polygon.At(j + 1), polygon.At(i))) return false;
			}
			else
			{
				if (RightOrOn(polygon.At(j), polygon.At(j + 1), polygon.At(i)) ||
					LeftOrOn(polygon.At(j), polygon.At(j - 1), polygon.At(i))) return false;
			}

			for (int k = 0; k < polygon.Count; ++k)
			{
				// YOGESH : changed from Line-Line intersection to Segment-Segment Intersection
				Microsoft.Xna.Framework.Vector2 p1 = polygon.At(i);
				Microsoft.Xna.Framework.Vector2 p2 = polygon.At(j);
				Microsoft.Xna.Framework.Vector2 q1 = polygon.At(k);
				Microsoft.Xna.Framework.Vector2 q2 = polygon.At(k + 1);

				// ignore incident edges
				if (p1.Equals(q1) || p1.Equals(q2) || p2.Equals(q1) || p2.Equals(q2))
					continue;

				var intersection = LineAlgorithms.LineSegmentIntersection(p1, p2, q1, q2);

				if (intersection != null
					&& intersection.WithinFirstSegment
					&& intersection.WithinSecondSegment)
				{
					Microsoft.Xna.Framework.Vector2 intPoint = intersection.IntersectionPoint;

					// intPoint is not any of the j line then false, else continue. Intersection has to be interior to qualify s 'false' from here
					if ((!intPoint.Equals(polygon.At(k))) || (!intPoint.Equals(polygon.At(k + 1))))
						return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Returns true if on is a reflex vertex. The polygon must be ordered in CCW for this to work properly.
		/// </summary>
		/// <param name="prev"></param>
		/// <param name="on"></param>
		/// <param name="next"></param>
		/// <returns></returns>
		private bool IsReflex(Microsoft.Xna.Framework.Vector2 prev, Microsoft.Xna.Framework.Vector2 on, Microsoft.Xna.Framework.Vector2 next)
		{
			// YOGESH: Added following condition of collinearity
			if (LineAlgorithms.AreCollinear(prev, on, next))
				return false;

			return Right(prev, on, next);
		}

		/// <summary>
		/// Returns true if c is left of the line connecting a and b.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		private bool Left(Microsoft.Xna.Framework.Vector2 a, Microsoft.Xna.Framework.Vector2 b, Microsoft.Xna.Framework.Vector2 c)
		{
			return Winding(a, b, c) > 0;
		}

		/// <summary>
		/// Returns true if c is left of or collinear with the line connecting a and b.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		private bool LeftOrOn(Microsoft.Xna.Framework.Vector2 a, Microsoft.Xna.Framework.Vector2 b, Microsoft.Xna.Framework.Vector2 c)
		{
			return Winding(a, b, c) >= 0;
		}

		/// <summary>
		/// Returns true if c is right of the line connecting a and b.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		private bool Right(Microsoft.Xna.Framework.Vector2 a, Microsoft.Xna.Framework.Vector2 b, Microsoft.Xna.Framework.Vector2 c)
		{
			return Winding(a, b, c) < 0;
		}

		/// <summary>
		/// Returns true if c is right of or collinear with the line connecting a and b.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		private bool RightOrOn(Microsoft.Xna.Framework.Vector2 a, Microsoft.Xna.Framework.Vector2 b, Microsoft.Xna.Framework.Vector2 c)
		{
			return Winding(a, b, c) <= 0;
		}
		
		/// <summary>
		/// Removes any points in the polygon which are collinear with its nearest neighbors.
		/// </summary>
		/// <param name="polygon">The polygon to search for collinear points</param>
		private Polygon RemoveCollinearPoints(Polygon polygon)
		{
			if (polygon.Count <= 3)
				return polygon;

			Polygon result = new Polygon();

			for (int i = 0; i < polygon.Count; i++)
			{
				Microsoft.Xna.Framework.Vector2 prev = polygon.At(i - 1);
				Microsoft.Xna.Framework.Vector2 current = polygon[i];
				Microsoft.Xna.Framework.Vector2 next = polygon.At(i + 1);

				// Skip adding this point to the result if it is collinear with its neighbors.
				if (LineAlgorithms.AreCollinear(prev, current, next))
					continue;

				result.Add(current);
			}

			return result;
		}

		/// <summary>
		/// Returns a positive number if c is to the left of the line going from a to b.
		/// </summary>
		/// <returns>Positive number if point is left, negative if point is right, 
		/// and 0 if points are collinear.</returns>
		private double Winding(Microsoft.Xna.Framework.Vector2 a, Microsoft.Xna.Framework.Vector2 b, Microsoft.Xna.Framework.Vector2 c)
		{
			return a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y);
		}
	}
}
