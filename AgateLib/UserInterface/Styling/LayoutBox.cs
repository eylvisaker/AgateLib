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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface
{
    public struct LayoutBox
    {
        public LayoutBox(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public int Bottom { get; set; }

        public int Right { get; set; }

        public int Left { get; set; }

        public int Top { get; set; }

        /// <summary>
        /// Left + Right values.
        /// </summary>
        public int Width => Left + Right;

        /// <summary>
        /// Top + Bottom values.
        /// </summary>
        public int Height => Top + Bottom;

        /// <summary>
        /// Gets a Point representing the top left of the LayoutBox.
        /// </summary>
        public Point TopLeft => new Point(Left, Top);

        /// <summary>
        /// Expands a rectangle by the layout box.
        /// </summary>
        /// <param name="targetRect"></param>
        /// <returns></returns>
        public Rectangle Expand(Rectangle targetRect)
        {
            return new Rectangle(
                targetRect.X - Left,
                targetRect.Y - Top,
                targetRect.Width + Width,
                targetRect.Height + Height);
        }

        /// <summary>
        /// Expands a rectangle by the layout box.
        /// </summary>
        /// <param name="targetRect"></param>
        /// <returns></returns>
        public Size Expand(Size targetRect)
        {
            return new Size(
                targetRect.Width + Width,
                targetRect.Height + Height);
        }

        /// <summary>
        /// Contracts a rectangle by the layout box.
        /// </summary>
        /// <param name="targetRect"></param>
        /// <returns></returns>
        public Rectangle Contract(Rectangle targetRect)
        {
            return new Rectangle(
                targetRect.X + Left,
                targetRect.Y + Top,
                targetRect.Width - Width,
                targetRect.Height - Height);
        }

        /// <summary>
        /// Contracts a size by the layout box.
        /// </summary>
        /// <param name="targetRect"></param>
        /// <returns></returns>
        public Size Contract(Size targetRect)
        {
            return new Size(
                targetRect.Width - Width,
                targetRect.Height - Height);
        }

        /// <summary>
        /// Gets a Point representing the bottom right of the LayoutBox.
        /// </summary>
        public Point BottomRight { get => new Point(Right, Bottom); }

        public override string ToString()
        {
            return $"{Left} {Top} {Right} {Bottom}";
        }

        /// <summary>
        /// Returns a LayoutBox object with all its values equal to the same number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LayoutBox SameAllAround(int value)
        {
            var result = new LayoutBox();

            result.Left = result.Right = result.Top = result.Bottom = value;

            return result;
        }

        /// <summary>
        /// Returns a LayoutBox object with left/right values equal to one another, and top/bottom values equal to one another.
        /// </summary>
        /// <param name="leftRight">The value for left and right</param>
        /// <param name="topBottom">The value for top and bottom.</param>
        /// <returns></returns>
        public static LayoutBox Rectangular(int leftRight, int topBottom)
        {
            return new LayoutBox
            {
                Left = leftRight,
                Right = leftRight,
                Top = topBottom,
                Bottom = topBottom,
            };
        }
    }
}
