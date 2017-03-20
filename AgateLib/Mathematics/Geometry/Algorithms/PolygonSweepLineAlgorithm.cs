
using AgateLib.Collections.Generic;

namespace AgateLib.Mathematics.Geometry.Algorithms
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using Quality;
	using Point = Vector2;

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
		const int LEFT = 0;
		const int RIGHT = 1;

		
		// xyorder(): determines the xy lexicographical order of two points
		//      returns: (+1) if p1 > p2; (-1) if p1 < p2; and  0 if equal 
		static int xyorder(Point p1, Point p2)
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
		static double isLeft(Point P0, Point P1, Point P2)
		{
			return (P1.X - P0.X) * (P2.Y - P0.Y) - (P2.X - P0.X) * (P1.Y - P0.Y);
		}
		//===================================================================



		// EventQueue Class

		// Event element data struct
		class Event
		{
			public int edge;          // polygon edge i is V[i] to V[i+1]
			public int type;          // event type: LEFT or RIGHT vertex
			public Point eV;            // event vertex
		};

		static int E_compare(Event v1, Event v2) // qsort compare two events
		{
			return xyorder(v1.eV, v2.eV);
		}

		// the EventQueue is a presorted array (no insertions needed)
		class EventQueue
		{
			int ne;                // total number of events in array
			int ix;                // index of next event on queue
			List<Event> Edata = new List<Event>();             // array of all events
			List<Event> Eq = new List<Event>();                // sorted list of event pointers

			public EventQueue(Polygon P)
			{

				ix = 0;
				ne = 2 * P.Count;           // 2 vertex events for each edge
				Edata.AddRange(Enumerable.Range(0, ne).Select(i => new Event()));
				Eq.AddRange(Edata);

				// Initialize event queue with edge segment endpoints
				for (int i = 0; i < P.Count; i++)
				{
					// init data for edge i
					Eq[2 * i].edge = i;
					Eq[2 * i + 1].edge = i;
					Eq[2 * i].eV = P.At(i);
					Eq[2 * i + 1].eV = P.At(i + 1);

					if (xyorder(P.At(i), P.At(i + 1)) < 0)
					{
						// determine type
						Eq[2 * i].type = LEFT;
						Eq[2 * i + 1].type = RIGHT;
					}
					else
					{
						Eq[2 * i].type = RIGHT;
						Eq[2 * i + 1].type = LEFT;
					}
				}

				// Sort Eq[] by increasing x and y
				Eq.Sort(E_compare);
			}

			public Event next()
			{
				if (ix >= ne)
					return null;
				else
					return Eq[ix++];
			}
		}

		// SweepLine Class

		// SweepLine segment data struct
		class SLseg
		{
			public int edge;          // polygon edge i is V[i] to V[i+1]
			public Point lP;            // leftmost vertex point
			public Point rP;            // rightmost vertex point
			public SLseg above;         // segment above this one
			public SLseg below;         // segment below this one

			public override string ToString()
			{
				return $"Edge: {edge}, Left: {lP}, Right: {rP}";
			}
		};

		static int SLsegComparison(SLseg a, SLseg b)
		{
			return a.edge.CompareTo(b.edge);
		}

		// the Sweep Line itself
		class SweepLine
		{
			int nv => Pn.Count;            // number of vertices in polygon
			Polygon Pn;            // initial Polygon
			BinaryTree<SLseg> Tree = new BinaryTree<SLseg>(SLsegComparison);          // balanced binary tree

			public SweepLine(Polygon P)            // constructor
			{ Pn = P; }

			public SLseg add(Event E)
			{
				// fill in SLseg element data
				SLseg s = new SLseg();
				s.edge = E.edge;

				// if it is being added, then it must be a LEFT edge event
				// but need to determine which endpoint is the left one 
				Point v1 = Pn.At(s.edge);
				Point v2 = Pn.At(s.edge + 1);

				// determine which is leftmost
				if (xyorder(v1, v2) < 0)
				{ 
					s.lP = v1;
					s.rP = v2;
				}
				else
				{
					s.lP = v2;
					s.rP = v1;
				}
				s.above = null;
				s.below = null;

				// add a node to the balanced binary tree
				BinaryTreeNode<SLseg> nd = Tree.Add(s);
				BinaryTreeNode<SLseg> nx = Tree.Next(nd);
				BinaryTreeNode<SLseg> np = Tree.Previous(nd);
				if (nx != null)
				{
					s.above = nx.Value;
					s.above.below = s;
				}
				if (np != null)
				{
					s.below = np.Value;
					s.below.above = s;
				}
				return s;
			}

			public SLseg find(Event E)
			{
				// need a segment to find it in the tree
				SLseg s = new SLseg();
				s.edge = E.edge;
				s.above = null;
				s.below = null;

				BinaryTreeNode<SLseg> nd = Tree.Find(s);

				return nd?.Value;
			}

			// test intersect of 2 segments and return: 0=none, 1=intersect
			public bool intersect(SLseg s1, SLseg s2)
			{
				if (s1 == null || s2 == null)
					return false;       // no intersect if either segment doesn't exist

				// check for consecutive edges in polygon
				int e1 = s1.edge;
				int e2 = s2.edge;
				if (((e1 + 1) % nv == e2) || (e1 == (e2 + 1) % nv))
					return false;       // no non-simple intersect since consecutive

				// test for existence of an intersect point
				double lsign, rsign;
				lsign = isLeft(s1.lP, s1.rP, s2.lP);    //  s2 left point sign
				rsign = isLeft(s1.lP, s1.rP, s2.rP);    //  s2 right point sign
				if (lsign * rsign > 0) // s2 endpoints have same sign  relative to s1
					return false;       // => on same side => no intersect is possible
				lsign = isLeft(s2.lP, s2.rP, s1.lP);    //  s1 left point sign
				rsign = isLeft(s2.lP, s2.rP, s1.rP);    //  s1 right point sign
				if (lsign * rsign > 0) // s1 endpoints have same sign  relative to s2
					return false;       // => on same side => no intersect is possible
										// the segments s1 and s2 straddle each other
				return true;            // => an intersect exists
			}

			public void remove(SLseg s)
			{
				// remove the node from the balanced binary tree
				BinaryTreeNode<SLseg> nd = Tree.Find(s);
				if (nd == null)
					return;       // not there

				// get the above and below segments pointing to each other
				BinaryTreeNode<SLseg> nx = Tree.Next(nd);
				if (nx != null)
				{
					SLseg sx = nx.Value;
					sx.below = s.below;
				}
				BinaryTreeNode<SLseg> np = Tree.Previous(nd);
				if (np != null)
				{
					SLseg sp = np.Value;
					sp.above = s.above;
				}
				Tree.Remove(nd);       // now  can safely remove it
			}
		}

		
		/// <summary>
		///  simple_Polygon(): test if a Polygon is simple or not
		///     Input:  Pn = a polygon with n vertices V[]
		///     Return: false(0) = is NOT simple
		///             true(1)  = IS simple
		/// </summary>
		/// <param name="Pn"></param>
		/// <returns></returns>
		bool simple_Polygon(Polygon Pn)
		{
			EventQueue Eq = new EventQueue(Pn);
			SweepLine SL = new SweepLine(Pn);
			Event e;                  // the current event
			SLseg s;                  // the current SL segment

			// This loop processes all events in the sorted queue
			// Events are only left or right vertices since
			// No new events will be added (an intersect => Done)
			while ((e = Eq.next()) != null)
			{
				// while there are events
				if (e.type == LEFT)
				{      // process a left vertex
					s = SL.add(e);          // add it to the sweep line
					if (SL.intersect(s, s.above))
						return false;      // Pn is NOT simple
					if (SL.intersect(s, s.below))
						return false;      // Pn is NOT simple
				}
				else
				{                      // processs a right vertex
					s = SL.find(e);
					if (SL.intersect(s.above, s.below))
						return false;      // Pn is NOT simple
					SL.remove(s);           // remove it from the sweep line
				}
			}
			return true;      // Pn IS simple
		}

		public bool IsSimple(Polygon polygon)
		{
			return simple_Polygon(polygon);
		}

		//===================================================================

	}
}