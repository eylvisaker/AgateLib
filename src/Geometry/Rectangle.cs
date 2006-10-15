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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Geometry
{
    /// <summary>
    /// Replacement for System.Drawing.Rectangle structure.
    /// </summary>
    [Serializable]
    public struct Rectangle
    {
        Point pt;
        Size sz;

        /// <summary>
        /// Constructs a rectangle.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Rectangle(int x, int y, int width, int height)
        {
            this.pt = new Point(x, y);
            this.sz = new Size(width, height);
        }
        /// <summary>
        /// Construts a rectangle.
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="sz"></param>
        public Rectangle(Point pt, Size sz)
        {
            this.pt = pt;
            this.sz = sz;
        }
        /// <summary>
        /// Constructs a rectangle.
        /// </summary>
        /// <param name="rect"></param>
        public Rectangle(System.Drawing.Rectangle rect)
        {
            this.pt = new Point(rect.Location);
            this.sz = new Size(rect.Size);
        }

        /// <summary>
        /// Static method which returns a rectangle with specified left, top, right and bottom.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        public static Rectangle FromLTRB(int left, int top, int right, int bottom)
        {
            return new Rectangle(left, top, right - left, bottom - top);
        }
        /// <summary>
        /// X value
        /// </summary>
        public int X
        {
            get { return pt.X; }
            set { pt.X = value; }
        }
        /// <summary>
        /// Y value
        /// </summary>
        public int Y
        {
            get { return pt.Y; }
            set { pt.Y = value; }
        }
        /// <summary>
        /// Width
        /// </summary>
        public int Width
        {
            get { return sz.Width; }
            set { sz.Width = value; }
        }
        /// <summary>
        /// Height
        /// </summary>
        public int Height
        {
            get { return sz.Height; }
            set { sz.Height = value; }
        }
        /// <summary>
        /// Gets bottom.
        /// </summary>
        public int Bottom
        {
            get { return pt.Y + sz.Height; }
        }
        /// <summary>
        /// Gets left.
        /// </summary>
        public int Left
        {
            get { return pt.X; }
        }
        /// <summary>
        /// Gets top.
        /// </summary>
        public int Top
        {
            get { return pt.Y; }
        }
        /// <summary>
        /// Gets right.
        /// </summary>
        public int Right
        {
            get { return pt.X + sz.Width; }
        }
        /// <summary>
        /// Gets or sets top-left point.
        /// </summary>
        public Point Location
        {
            get { return pt; }
            set { pt = value; }
        }
        /// <summary>
        /// Gets or sets size.
        /// </summary>
        public Size Size
        {
            get { return sz; }
            set { sz = value; }
        }
        /// <summary>
        /// Returns true if the rectangle contains the specified point.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Contains(int x, int y)
        {
            return Contains(new Point(x, y));
        }
        /// <summary>
        /// Returns true if the rectangle contains the specified point.
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool Contains(Point pt)
        {
            if (pt.X < Left)
                return false;
            if (pt.Y < Top)
                return false;
            if (pt.X > Right)
                return false;
            if (pt.Y > Bottom)
                return false;

            return true;
        }
        /// <summary>
        /// Returns true if the rectangle entirely contains the specified rectangle.
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool Contains(Rectangle rect)
        {
            if (Contains(rect.Location) && Contains(Point.Add(rect.Location, rect.Size)))
                return true;
            else
                return false;
        }
        
        /// <summary>
        /// Returns true if this intersects another rectangle.
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool IntersectsWith(Rectangle rect)
        {
            if (this.Left > rect.Right) return false;
            if (rect.Left > this.Right) return false;
            if (this.Top > rect.Bottom) return false;
            if (rect.Top > this.Bottom) return false;

            return true;
        }
        /// <summary>
        /// True if this is (0,0,0,0).
        /// </summary>
        public bool IsEmpty
        {
            get { return pt.IsEmpty && sz.IsEmpty; }
        }


        #region --- Operator Overloads ---

        /// <summary>
        /// Equality comparison test.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Rectangle a, Rectangle b)
        {
            return a.Equals(b);
        }
        /// <summary>
        /// Inequality comparison test.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Rectangle a, Rectangle b)
        {
            return !a.Equals(b);
        }

        #endregion
        #region --- Object Overrides ---

        /// <summary>
        /// Gets a hash code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (Left + Top + Bottom + Right);
        }
        
        /// <summary>
        /// Creates a string representing this rectangle.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}X={1},Y={2},Width={3},Height={4}", "{", X, Y, Width, Height, "}");
        }
        /// <summary>
        /// Equality test.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Rectangle )
                return Equals((Rectangle)obj);
            else
                return base.Equals(obj);
        }
        /// <summary>
        /// Equality test.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(Rectangle obj)
        {
            if (pt == obj.pt && sz == obj.sz)
                return true;
            else
                return false;
        }

        #endregion

        /// <summary>
        /// Empty rectangle
        /// </summary>
        public static readonly Rectangle Empty = new Rectangle(0, 0, 0, 0);
        /// <summary>
        /// Static method returning the intersection of two rectangles.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Rectangle Intersect(Rectangle a, Rectangle b)
        {
            if (a.IntersectsWith(b))
            {
                return Rectangle.FromLTRB(
                    Math.Max(a.Left, b.Left),
                    Math.Max(a.Top, b.Top),
                    Math.Min(a.Right, b.Right),
                    Math.Min(a.Bottom, b.Bottom));
            }
            else
                return Empty;
        }
        /// <summary>
        /// For inter-op with System.Drawing.
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static explicit operator System.Drawing.Rectangle(Rectangle rect)
        {
            return new System.Drawing.Rectangle(
                rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}