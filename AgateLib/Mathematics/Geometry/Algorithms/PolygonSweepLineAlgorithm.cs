//
//    Copyright (c) 2006-2017 Erik Ylvisaker
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

using AgateLib.Collections.Generic;

namespace AgateLib.Mathematics.Geometry.Algorithms
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using Quality;

	/// <summary>
	/// Implementation of the Shamos-Hoey Algorithm found at http://geomalgorithms.com/a09-_intersect-3.html#simple_Polygon()
	/// </summary>
	public class PolygonSweepLineAlgorithm : IPolygonSimpleDetectionAlgorithm
	{
		// Copyright 2001 softSurfer, 2012 Dan Sunday
		// This code may be freely used and modified for any purpose
		// providing that this copyright notice is included with it.
		// SoftSurfer makes no warranty for this code, and cannot be held
		// liable for any real or imagined damage resulting from its use.
		// Users of this code must verify correctness for their application.
		// Cleaned up by Erik Ylvisaker

		// Assume that classes are already given for the objects:
		//    Point with 2D coordinates {float x, y;}
		//    Polygon with n vertices {int n; Point *V;} with V[n]=V[0]
		//    Tnode is a node element structure for a BBT
		//    BBT is a class for a Balanced Binary Tree
		//        such as an AVL, a 2-3, or a  red-black tree
		//        with methods given by the  placeholder code:
		enum EventType
		{
			Left,
			Right
		}

		// xyorder(): determines the xy lexicographical order of two points
		//      returns: (+1) if p1 > p2; (-1) if p1 < p2; and  0 if equal 
		static int XYOrder(Vector2 p1, Vector2 p2)
		{
			// test the x-coord first
			if (p1.X > p2.X) return 1;
			if (p1.X < p2.X) return (-1);

			// and test the y-coord second
			if (p1.Y > p2.Y) return 1;
			if (p1.Y < p2.Y) return (-1);

			// when you exclude all other possibilities, what remains  is...
			return 0;  // they are the same point 
		}

		// isLeft(): tests if point P2 is Left|On|Right of the line P0 to P1.
		//      returns: >0 for left, 0 for on, and <0 for  right of the line.
		//      (see Algorithm 1 on Area of Triangles)
		static double IsLeft(Vector2 P0, Vector2 P1, Vector2 P2)
		{
			return (P1.X - P0.X) * (P2.Y - P0.Y) - (P2.X - P0.X) * (P1.Y - P0.Y);
		}
		//===================================================================



		// EventQueue Class

		// Event element data struct
		class Event
		{
			public int Edge;          // polygon edge i is V[i] to V[i+1]
			public EventType EventType;   // event type: LEFT or RIGHT vertex
			public Vector2 Vertex;            // event vertex
		};

		static int EventComparison(Event v1, Event v2) // qsort compare two events
		{
			return XYOrder(v1.Vertex, v2.Vertex);
		}

		// the EventQueue is a presorted array (no insertions needed)
		class EventQueue
		{
			int eventCount;                // total number of events in array
			int nextIndex;                // index of next event on queue
			List<Event> events = new List<Event>();             // array of all events
			List<Event> sortedEvents = new List<Event>();                // sorted list of event pointers

			public EventQueue(Polygon polygon)
			{
				nextIndex = 0;
				eventCount = 2 * polygon.Count;           // 2 vertex events for each edge
				events.AddRange(Enumerable.Range(0, eventCount).Select(i => new Event()));
				sortedEvents.AddRange(events);

				// Initialize event queue with edge segment endpoints
				for (int i = 0; i < polygon.Count; i++)
				{
					// Initialize data for edge i
					// The original C code had either an issue or an undocumented assumption here: 
					// they used polygon[i + 1] which could result in an out of memory error, unless
					// the next point was automatically returned.
					sortedEvents[2 * i].Edge = i;
					sortedEvents[2 * i + 1].Edge = i;
					sortedEvents[2 * i].Vertex = polygon.At(i);
					sortedEvents[2 * i + 1].Vertex = polygon.At(i + 1);

					// determine type
					if (XYOrder(polygon.At(i), polygon.At(i + 1)) <= 0)
					{
						sortedEvents[2 * i].EventType = EventType.Left;
						sortedEvents[2 * i + 1].EventType = EventType.Right;
					}
					else
					{
						sortedEvents[2 * i].EventType = EventType.Right;
						sortedEvents[2 * i + 1].EventType = EventType.Left;
					}
				}

				// Sort Eq[] by increasing x and y
				sortedEvents.Sort(EventComparison);
			}

			public Event Next()
			{
				if (nextIndex >= eventCount)
					return null;

				return sortedEvents[nextIndex++];
			}
		}

		// SweepLine Class

		// SweepLine segment data struct
		class SweepLineSegment
		{
			public int Edge;          // polygon edge i is V[i] to V[i+1]
			public Vector2 LeftVertex;            // leftmost vertex point
			public Vector2 RightVertex;            // rightmost vertex point
			public SweepLineSegment Above;         // segment above this one
			public SweepLineSegment Below;         // segment below this one

			public override string ToString()
			{
				return $"Edge: {Edge}, Left: {LeftVertex}, Right: {RightVertex}";
			}
		};

		static int SLsegComparison(SweepLineSegment a, SweepLineSegment b)
		{
			return a.Edge.CompareTo(b.Edge);
		}

		// the Sweep Line itself
		class SweepLine
		{
			/// <summary>
			/// Initial Polygon
			/// </summary>
			private readonly Polygon polygon;

			/// <summary>
			/// Balanced binary tree
			/// </summary>
			private readonly BinaryTree<SweepLineSegment> tree = new BinaryTree<SweepLineSegment>(SLsegComparison)
			{
				AutoBalance = true
			};

			public SweepLine(Polygon polygon) // constructor
			{
				this.polygon = polygon;
			}

			public SweepLineSegment Add(Event E)
			{
				SweepLineSegment segToAdd = new SweepLineSegment { Edge = E.Edge };

				// if it is being added, then it must be a LEFT edge event
				// but need to determine which endpoint is the left one 
				Vector2 v1 = polygon.At(segToAdd.Edge);
				Vector2 v2 = polygon.At(segToAdd.Edge + 1);

				// determine which is leftmost
				if (XYOrder(v1, v2) < 0)
				{
					segToAdd.LeftVertex = v1;
					segToAdd.RightVertex = v2;
				}
				else
				{
					segToAdd.LeftVertex = v2;
					segToAdd.RightVertex = v1;
				}
				segToAdd.Above = null;
				segToAdd.Below = null;

				// add a node to the balanced binary tree
				BinaryTreeNode<SweepLineSegment> nd = tree.Add(segToAdd);
				BinaryTreeNode<SweepLineSegment> nx = nd.Next();
				BinaryTreeNode<SweepLineSegment> np = nd.Previous();

				if (nx != null)
				{
					segToAdd.Above = nx.Value;
					segToAdd.Above.Below = segToAdd;
				}
				if (np != null)
				{
					segToAdd.Below = np.Value;
					segToAdd.Below.Above = segToAdd;
				}

				return segToAdd;
			}

			public SweepLineSegment Find(Event E)
			{
				// need a segment to find it in the tree
				SweepLineSegment s = new SweepLineSegment();
				s.Edge = E.Edge;
				s.Above = null;
				s.Below = null;

				BinaryTreeNode<SweepLineSegment> nd = tree.Find(s);

				return nd?.Value;
			}

			// test intersect of 2 segments and return: 0=none, 1=intersect
			public bool Intersect(SweepLineSegment s1, SweepLineSegment s2)
			{
				// no intersect if either segment doesn't exist
				if (s1 == null || s2 == null)
					return false;

				// check for consecutive edges in polygon
				int e1 = s1.Edge;
				int e2 = s2.Edge;
				if (((e1 + 1) % polygon.Count == e2) || (e1 == (e2 + 1) % polygon.Count))
					return false;       // no non-simple intersect since consecutive

				// test for existence of an intersect point
				double lsign, rsign;
				lsign = IsLeft(s1.LeftVertex, s1.RightVertex, s2.LeftVertex);    //  s2 left point sign
				rsign = IsLeft(s1.LeftVertex, s1.RightVertex, s2.RightVertex);    //  s2 right point sign

				if (lsign * rsign > 0) // s2 endpoints have same sign  relative to s1
					return false;       // => on same side => no intersect is possible

				lsign = IsLeft(s2.LeftVertex, s2.RightVertex, s1.LeftVertex);    //  s1 left point sign
				rsign = IsLeft(s2.LeftVertex, s2.RightVertex, s1.RightVertex);    //  s1 right point sign

				if (lsign * rsign > 0) // s1 endpoints have same sign  relative to s2
					return false;       // => on same side => no intersect is possible

				// the segments s1 and s2 straddle each other
				return true;            // => an intersect exists
			}

			public void Remove(SweepLineSegment s)
			{
				// remove the node from the balanced binary tree
				BinaryTreeNode<SweepLineSegment> nd = tree.Find(s);
				if (nd == null)
					return;       // not there

				// get the above and below segments pointing to each other
				BinaryTreeNode<SweepLineSegment> nx = nd.Next();
				if (nx != null)
				{
					SweepLineSegment sx = nx.Value;
					sx.Below = s.Below;
				}
				BinaryTreeNode<SweepLineSegment> np = nd.Previous();
				if (np != null)
				{
					SweepLineSegment sp = np.Value;
					sp.Above = s.Above;
				}
				tree.Remove(nd);       // now  can safely remove it
			}
		}


		/// <summary>
		///  simple_Polygon(): test if a Polygon is simple or not
		///     Input:  Pn = a polygon with n vertices V[]
		///     Return: false(0) = is NOT simple
		///             true(1)  = IS simple
		/// </summary>
		/// <param name="polygon"></param>
		/// <returns></returns>
		public bool IsSimple(Polygon polygon)
		{
			Require.ArgumentNotNull(polygon, nameof(polygon));

			EventQueue eventQueue = new EventQueue(polygon);
			SweepLine sweepLine = new SweepLine(polygon);
			Event currentEvent;                  // the current event

			// This loop processes all events in the sorted queue
			// Events are only left or right vertices since
			// No new events will be added (an intersect => Done)
			while ((currentEvent = eventQueue.Next()) != null)
			{
				// while there are events
				if (currentEvent.EventType == EventType.Left)
				{      // process a left vertex
					var currentSegment = sweepLine.Add(currentEvent);          // add it to the sweep line
					if (sweepLine.Intersect(currentSegment, currentSegment.Above))
						return false;      // Pn is NOT simple
					if (sweepLine.Intersect(currentSegment, currentSegment.Below))
						return false;      // Pn is NOT simple
				}
				else
				{                      // processs a right vertex
					var currentSegment = sweepLine.Find(currentEvent);
					if (sweepLine.Intersect(currentSegment.Above, currentSegment.Below))
						return false;      // Pn is NOT simple
					sweepLine.Remove(currentSegment);           // remove it from the sweep line
				}
			}
			return true;      // Pn IS simple
		}
	}
}