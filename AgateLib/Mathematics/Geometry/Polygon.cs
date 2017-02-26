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
using System.Linq;
using System.Runtime.Serialization;
using AgateLib.Quality;
using YamlDotNet.Serialization;

namespace AgateLib.Mathematics.Geometry
{
	/// <summary>
	/// Class which represents a two-dimensional polygon in a plane.
	/// </summary>
	public class Polygon
	{
		public static Polygon FromRect(Rectangle rect)
		{
			Polygon retval = new Polygon(new[]
			{
				new Vector2(rect.Left, rect.Top),
				new Vector2(rect.Right, rect.Top),
				new Vector2(rect.Right, rect.Bottom),
				new Vector2(rect.Left, rect.Bottom)
			});

			return retval;
		}

		private Vector2List points = new Vector2List();
		private bool isConvex;
		
		/// <summary>
		/// Constructs an empty polygon object.
		/// </summary>
		public Polygon()
		{ }

		/// <summary>
		/// Constructs a polygon from the given points.
		/// </summary>
		/// <param name="points"></param>
		public Polygon(IEnumerable<Vector2> points)
		{
			this.points = points.ToVector2List();

			ComputeProperties();
		}

		/// <summary>
		/// Constructs a polygon from the given points.
		/// </summary>
		/// <param name="points"></param>
		public Polygon(IVector2List points)
		{
			this.points = points.ToVector2List();
		}

		/// <summary>
		/// Gets or sets the list of points for the polygon.
		/// </summary>
		/// <param name="points"></param>
		public Vector2List Points
		{
			get { return points; }
			set
			{
				Require.ArgumentNotNull(value, nameof(Points));
				points = value;
				points.Dirty = true;
			}
		}

		/// <summary>
		/// Gets the axis-aligned bounding rect.
		/// </summary>
		[YamlIgnore]
		public Rectangle BoundingRect
		{
			get
			{
				if (points.Count == 0)
					throw new InvalidOperationException();

				int left = int.MaxValue;
				int right = int.MinValue;
				int top = int.MaxValue;
				int bottom = int.MinValue;

				foreach (var pt in points)
				{
					if (pt.X < left) left = (int)pt.X;
					if (pt.X > right) right = (int)pt.X;
					if (pt.Y < top) top = (int)pt.Y;
					if (pt.Y > bottom) bottom = (int)pt.Y;
				}

				return Rectangle.FromLTRB(left, top, right, bottom);
			}
		}

		/// <summary>
		/// Gets whether this polygon is convex.
		/// </summary>
		[YamlIgnore]
		public bool IsConvex
		{
			get
			{
				if (Points.Dirty)
					ComputeProperties();

				return isConvex;
			}
			private set { isConvex = value; }
		}

		/// <summary>
		/// Gets whether this polygon is concave.
		/// </summary>
		[YamlIgnore]
		public bool IsConcave => !IsConvex;

		/// <summary>
		/// Gets the count of points that make up the polygon.
		/// </summary>
		[YamlIgnore]
		public int Count => points.Count;

		/// <summary>
		/// Gets or sets a specific point in this polygon/
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		[YamlIgnore]
		public Vector2 this[int index]
		{
			get { return points[index]; }
			set
			{
				points[index] = value;
			}
		}

		/// <summary>
		/// Adds a point to the polygon.
		/// </summary>
		/// <param name="point"></param>
		public void Add(Vector2 point)
		{
			points.Add(point);
		}

		/// <summary>
		/// Adds a point to the polygon.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void Add(double x, double y)
		{
			points.Add(new Vector2(x, y));
		}

		/// <summary>
		/// Clones this polygon.
		/// </summary>
		/// <returns></returns>
		public Polygon Clone() => new Polygon(points);

		/// <summary>
		/// Gets whether a point exists within the closed area of this polygon.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public bool Contains(Vector2 point)
		{
			// This is a ray tracing algorithm. It computes whether a point is within a polygon by
			// counting the number of times a ray passing through the test point crosses an 
			// edge of the polygon. If it's odd, the point is inside the polygon and if it's even
			// the point is outside.

			// Algorithm from:
			// http://stackoverflow.com/a/15599478/3856907

			int nvert = points.Count;
			bool contains = false;

			for (int i = 0, j = nvert - 1; i < nvert; j = i++)
			{
				if (points[i].Y >= point.Y != points[j].Y >= point.Y &&
					point.X <= (points[j].X - points[i].X) * (point.Y - points[i].Y) / (points[j].Y - points[i].Y) + points[i].X
				)
				{
					contains = !contains;
				}
			}

			return contains;
		}

		/// <summary>
		/// Tests whether a convex polygon contains a point. 
		/// TODO: This should be profiled against the general Contains method. Unless it performs significantly better
		/// it should be removed.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		private bool ConvexPolygonContains(Vector2 point)
		{
			Require.True<InvalidOperationException>(IsConvex,
				"ConvexPolygonContains algorithm only works for convex polygons.");

			// Magic here:
			// We're computing (y-y0)(x1-x0) - (x-x0)(y1-y0) for three vector2s:
			//    test point: (x,y)
			//    two consecutive vector2s (x0,y0) and (x1,y1) that form an edge of the polygon.
			// If the coefficients are always on the same side, then the point is inside the polygon.
			var coef = points.Skip(1).Select((p, i) =>
				(point.Y - points[i].Y) * (p.X - points[i].X)
				- (point.X - points[i].X) * (p.Y - points[i].Y));

			double? last = null;

			foreach (var c in coef)
			{
				// C == 0 indicates the point is right on the edge, and thus inside the polygon.
				if (c == 0)
					return true;

				if (last != null && last.Value * c < 0)
					return false;

				last = c;
			}

			return true;
		}

		private void ComputeProperties()
		{
			ComputeConvexity();

			Points.Dirty = false;
		}

		private void ComputeConvexity()
		{
			const double TOLERANCE = 1e-10;

			// algorithm taken from here:
			// http://www.sunshine2k.de/coding/java/Polygon/Convex/polygon.htm
			double sign = 0;

			for (int i = 0; i < points.Count; i++)
			{
				var p = i > 1 ? points[i - 2] : points[points.Count - 2 + i];
				var t = i > 0 ? points[i - 1] : points.Last();
				var u = points[i];
				var v = t - p;

				var newSign = u.X * v.Y - u.Y * v.X + v.X * p.Y - p.X * v.Y;

				if (Math.Abs(newSign) < TOLERANCE)
					continue;

				if (sign == 0)
					sign = newSign;
				else if (newSign * sign < 0)
				{
					IsConvex = false;
					return;
				}
			}

			IsConvex = true;
		}
	}
}
