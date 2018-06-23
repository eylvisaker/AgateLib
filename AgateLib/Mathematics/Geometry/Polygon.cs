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
using System.Linq;
using AgateLib.Mathematics.Geometry.Algorithms;
using AgateLib.Mathematics.Geometry.Algorithms.ConvexDecomposition;
using AgateLib.Quality;
using Microsoft.Xna.Framework;
using YamlDotNet.Serialization;

namespace AgateLib.Mathematics.Geometry
{
    /// <summary>
    /// Class which represents a two-dimensional polygon in a plane.
    /// </summary>
    public class Polygon : IVector2List, IReadOnlyPolygon
    {
        private static IPolygonConvexDecompositionAlgorithm _convexDecompositionAlgorithm = new BayazitDecomposition();
        private static IPolygonSimpleDetectionAlgorithm _simplePolygonDetectionAlgorithm = new PolygonSweepLineAlgorithm();

        /// <summary>
        /// Set this to true to skip all computations of polygon simplicity.
        /// These calculations are fairly expensive so performance may be improved if this is set to true.
        /// This of course means that if you actually have complex polygons, they will not be detected
        /// as complex and algorithms will not work properly.
        /// </summary>
        public static bool AssumeAllPolygonsAreSimple { get; set; } = true;

        private Vector2List points = new Vector2List();

        private bool isSimple;
        private bool isConvex;
        private PolygonWinding winding;
        private List<Polygon> convexDecomposition = new List<Polygon>();
        private List<PolygonEdge> edges = new List<PolygonEdge>();
        private RectangleF boundingRect;

        private Vector2 centroid;

        /// <summary>
        /// Constructs an empty polygon object.
        /// </summary>
        public Polygon()
        {
        }

        /// <summary>
        /// Constructs a polygon from the given points.
        /// </summary>
        /// <param name="points"></param>
        public Polygon(IEnumerable<Vector2> points) : this()
        {
            this.points = points.ToVector2List();
            this.points.Dirty = true;

            ComputeAllProperties();
        }

        /// <summary>
        /// Constructs a polygon from the given points.
        /// </summary>
        /// <param name="points"></param>
        public Polygon(params Vector2[] points)
        {
            this.points = points.ToVector2List();
            this.points.Dirty = true;

            ComputeAllProperties();
        }

        /// <summary>
        /// Constructs a polygon from the given points.
        /// </summary>
        /// <param name="points"></param>
        public Polygon(IVector2List points)
        {
            this.points = points.ToVector2List();
            this.points.Dirty = true;

            ComputeAllProperties();
        }

        /// <summary>
        /// Gets or sets the list of points for the polygon.
        /// </summary>
        [YamlIgnore]
        public Vector2List Points
        {
            get => points;
            set
            {
                Require.ArgumentNotNull(value, nameof(Points));
                points = value;
                points.Dirty = true;
            }
        }

        IReadOnlyList<Vector2> IReadOnlyPolygon.Points => Points;

        [YamlIgnore]
        public IEnumerable<PolygonEdge> Edges
        {
            get
            {
                if (points.Dirty)
                    ComputeAllProperties();

                return edges;
            }
        }
        bool ICollection<Vector2>.IsReadOnly => false;

        /// <summary>
        /// Gets the axis-aligned bounding rect.
        /// </summary>
        [YamlIgnore]
        public RectangleF BoundingRect
        {
            get
            {
                if (points.Count == 0)
                    throw new InvalidOperationException();

                if (points.Dirty)
                    ComputeAllProperties();

                return boundingRect;
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
                    ComputeAllProperties();

                return isConvex;
            }
            private set => isConvex = value;
        }

        /// <summary>
        /// Returns a convex decomposition of this polygon.
        /// If this polygon is convex, this just returns itself.
        /// </summary>
        [YamlIgnore]
        public IEnumerable<IReadOnlyPolygon> ConvexDecomposition
        {
            get
            {
                if (Points.Dirty)
                    ComputeAllProperties();

                if (IsConvex)
                {
                    yield return this;
                }
                else
                {
                    foreach (var convexPoly in convexDecomposition)
                        yield return convexPoly;
                }
            }
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
        /// Returns the winding direction of the points of the polygon.
        /// </summary>
        [YamlIgnore]
        public PolygonWinding Winding

        {
            get
            {
                if (Points.Dirty)
                    ComputeAllProperties();

                return winding;
            }
        }

        /// <summary>
        /// Computes the area of the polygon.
        /// </summary>
        [YamlIgnore]
        public double Area
        {
            get
            {
                double result = 0;
                Vector2 prev = points[points.Count - 1];

                // Algorithm from http://mathopenref.com/coordpolygonarea2.html
                foreach (var point in points)
                {
                    result += (point.X + prev.X) * (point.Y - prev.Y);
                    prev = point;
                }

                // Absolute value function allows this algorithm to work regardless
                // of the winding of the polygon.
                return Math.Abs(result * 0.5);
            }
        }

        /// <summary>
        /// Returns true if this polygon is simple, meaning that none of its line segments intersect each other.
        /// </summary>
        public bool IsSimple
        {
            get
            {
                if (points.Dirty)
                    ComputeAllProperties();

                return isSimple;
            }
        }

        /// <summary>
        /// Returns true if this polygon is complex, meaning that at least one of its line segments intersect each other.
        /// Complex polygons introduce ambiguities, so many algorithms don't work with them.
        /// </summary>
        public bool IsComplex => !IsSimple;

        /// <summary>
        /// Returns the centroid of the polygon. The centroid is the average of all the points in the polygon.
        /// </summary>
        public Vector2 Centroid
        {
            get
            {
                if (points.Dirty)
                    ComputeAllProperties();

                return centroid;
            }
        }

        /// <summary>
        /// Gets or sets a specific point in this polygon/
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [YamlIgnore]
        public Vector2 this[int index]
        {
            get => points[index];
            set => points[index] = value;
        }

        /// <summary>
        /// Returns a debug string describing this polygon.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"N = {Count}, Axis-Aligned Bounds = {BoundingRect}";
        }

        /// <summary>
        /// Copies the points to an array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(Vector2[] array, int arrayIndex)
        {
            points.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns true if the item is one of the polygon's vertices.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(Vector2 item)
        {
            return points.Contains(item);
        }

        /// <summary>
        /// Copies the verticies of this polygon to the target.
        /// </summary>
        /// <param name="target"></param>
        public void CopyTo(Polygon target)
        {
            target.Points.Count = Count;

            for (int i = 0; i < Count; i++)
                target[i] = this[i];
        }

        /// <summary>
        /// Returns the index of a point.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(Vector2 item)
        {
            return points.IndexOf(item);
        }

        /// <summary>
        /// Inserts a point into the polygon.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, Vector2 item)
        {
            points.Insert(index, item);
        }

        /// <summary>
        /// Removes a point from the polygon.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(Vector2 item)
        {
            return points.Remove(item);
        }

        /// <summary>
        /// Removes a point at a specified index.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            points.RemoveAt(index);
        }

        /// <summary>
        /// Clears the polygon of points.
        /// </summary>
        public void Clear()
        {
            points.Clear();
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
        public void Add(float x, float y)
        {
            points.Add(new Vector2(x, y));
        }

        /// <summary>
        /// Clones this polygon.
        /// </summary>
        /// <returns></returns>
        public Polygon Clone()
        {
            var result = new Polygon(points);

            result.isSimple = isSimple;
            result.isConvex = isConvex;
            result.winding = winding;
            result.boundingRect = boundingRect;

            result.ComputeEdges();

            foreach (var polygon in convexDecomposition)
            {
                result.convexDecomposition.Add(polygon.Clone());
            }

            result.Points.Dirty = Points.Dirty;

            return result;
        }


        /// <summary>
        /// Returns a translated polygon by adding the passed vector to each point in this polygon.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Polygon Translate(Vector2 amount)
        {
            return new Polygon(points.Select(x => amount + x));
        }

        /// <summary>
        /// Translates the points in this polygon by adding the passed vector to each point in this polygon.
        /// </summary>
        /// <param name="amount"></param>
        public void TranslateSelf(Vector2 amount)
        {
            var dirty = Points.Dirty;

            for (int i = 0; i < Count; i++)
                this[i] += amount;

            boundingRect.X += amount.X;
            boundingRect.Y += amount.Y;

            Points.Dirty = dirty;
        }

        /// <summary>
        /// Gets whether a point exists within the closed area of this polygon.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool AreaContains(Vector2 point)
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
                if (point == points[i])
                    return true;

                // first check to see if it's on the edge.
                var distance1 = (point - points[i]).Length();
                var distance2 = (point - points[j]).Length();
                var segmentLength = (points[i] - points[j]).Length();

                if (Math.Abs(distance1 + distance2 - segmentLength) < 1e-6)
                    return true;

                // Now do ray tracing with this edge.
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
        /// Enumerates the points.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Enumerates the points.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Vector2> GetEnumerator()
        {
            return points.GetEnumerator();
        }

        /// <summary>
        /// Returns a new polygon which is this one rotated counter-clockwise by an angle in degrees.
        /// </summary>
        /// <param name="angleInDegrees">The angle in degrees to rotate the polygon by. Pass negative values
        /// to rotate clockwise.</param>
        /// <param name="rotationCenter">The center of rotation. If null is passed, the origin is used
        /// as the center of rotation</param>
        /// <returns></returns>
        public Polygon RotateDegrees(float angleInDegrees, Vector2? rotationCenter = null)
        {
            return Rotate((float)Math.PI / 180.0f * angleInDegrees, rotationCenter);
        }

        /// <summary>
        /// Returns a new polygon which is identical to this one rotated counter-clockwise by an angle.
        /// </summary>
        /// <param name="angle">The angle in degrees to rotate the polygon by. Pass negative values
        /// to rotate clockwise.</param>
        /// <param name="rotationCenter">The center of rotation. If null is passed, the origin is used
        /// as the center of rotation</param>
        /// <returns></returns>
        public Polygon Rotate(float angle, Vector2? rotationCenter = null)
        {
            var result = Clone();

            result.RotateSelf(angle, rotationCenter);

            return result;
        }

        /// <summary>
        /// Returns a new polygon which is identical to this one rotated counter-clockwise by an angle.
        /// </summary>
        /// <param name="angle">The angle in degrees to rotate the polygon by. Pass negative values
        /// to rotate clockwise.</param>
        /// <param name="rotationCenter">The center of rotation. If null is passed, the origin is used
        /// as the center of rotation</param>
        /// <param name="tolerance">If the angle is smaller than the tolerance value, the rotation calculation
        /// will be skipped.</param>
        /// <returns></returns>
        public void RotateSelf(float angle, Vector2? rotationCenter = null, float tolerance = 1e-4f)
        {
            if (angle < tolerance && angle > -tolerance)
                return;

            var dirty = points.Dirty;
            var center = rotationCenter ?? Vector2.Zero;

            for (int i = 0; i < Count; i++)
            {
                Vector2 delta = this[i] - center;
                Vector2 newPoint = delta.Rotate(angle);
                this[i] = newPoint + center;
            }

            points.Dirty = dirty;

            // In rotating, the only property that needs to be computed is the bounding rect.
            ComputeBoundingRect();
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
            Require.That<InvalidOperationException>(IsConvex,
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

        private void ComputeAllProperties()
        {
            if (!Points.Dirty)
                return;

            Points.Dirty = false;

            winding = this.IsCounterClockwise() ? PolygonWinding.CounterClockwise : PolygonWinding.Clockwise;

            ComputeCentroid();
            ComputeEdges();
            ComputeConvexity();
            ComputeSimplicity();
            ComputeConvexDecomposition();
            ComputeBoundingRect();
        }

        private void ComputeBoundingRect()
        {
            float left = float.MaxValue;
            float right = float.MinValue;
            float top = float.MaxValue;
            float bottom = float.MinValue;

            foreach (var pt in points)
            {
                if (pt.X < left) left = pt.X;
                if (pt.X > right) right = pt.X;
                if (pt.Y < top) top = pt.Y;
                if (pt.Y > bottom) bottom = pt.Y;
            }

            boundingRect = RectangleF.FromLTRB(left, top, right, bottom);
        }

        private void ComputeEdges()
        {
            lock (edges)
            {
                while (edges.Count > Points.Count)
                    edges.RemoveAt(edges.Count - 1);
                while (edges.Count < Points.Count)
                    edges.Add(new PolygonEdge(new LineSegment(), new Vector2()));

                for (int i = 0, j = Points.Count - 1; i < Points.Count; j = i++)
                {
                    var segment = new LineSegment { Start = Points[j], End = Points[i] };
                    var normal = new Vector2(segment.Displacement.Y, -segment.Displacement.X);
                    normal.Normalize();

                    if (winding == PolygonWinding.Clockwise)
                        normal *= -1;

                    edges[i].SetValue(segment, normal);
                }
            }
        }

        private void ComputeCentroid()
        {
            centroid = Points.Sum() / Points.Count;
        }

        private void ComputeSimplicity()
        {
            if (AssumeAllPolygonsAreSimple)
            {
                isSimple = true;
            }
            else
            {
                isSimple = _simplePolygonDetectionAlgorithm.IsSimple(this);
            }
        }

        private void ComputeConvexity(double tolerance = 1e-10)
        {
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

                if (Math.Abs(newSign) < tolerance)
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

        private void ComputeConvexDecomposition()
        {
            if (IsConvex)
            {
                convexDecomposition.Clear();

                return;
            }

            convexDecomposition = _convexDecompositionAlgorithm.BuildConvexDecomposition(this).ToList();
        }
    }

    /// <summary>
    /// Read-only interface to a polygon.
    /// </summary>
    public interface IReadOnlyPolygon
    {
        /// <summary>
        /// Gets the points that make up the polygon.
        /// </summary>
        IReadOnlyList<Vector2> Points { get; }

        /// <summary>
        /// Gets whether the polygon is convex.
        /// </summary>
        bool IsConvex { get; }

        /// <summary>
        /// Gets whether the polygon is concave.
        /// </summary>
        bool IsConcave { get; }

        /// <summary>
        /// Gets the bounding rectangle for the polygon.
        /// </summary>
        RectangleF BoundingRect { get; }
    }
}
