﻿//
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

using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface
{
    /// <summary>
    /// Class which describes how a border for a render element should be drawn.
    /// </summary>
    public class BorderStyle
    {
        /// <summary>
        /// Returns a BorderStyle object which has a solid color on all sides.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static BorderStyle Solid(Color color, int width)
        {
            return new BorderStyle
            {
                Top = new BorderSideStyle { Color = color, Width = width },
                Left = new BorderSideStyle { Color = color, Width = width },
                Right = new BorderSideStyle { Color = color, Width = width },
                Bottom = new BorderSideStyle { Color = color, Width = width },
            };
        }

        /// <summary>
        /// The Id value for a theme border. If this is specified, a theme border is used
        /// and the rest of the properties in this object are ignored.
        /// </summary>
        public string Id { get; set; }

        public ImageSource Image { get; set; }

        public LayoutBox ImageSlice { get; set; }

        public BorderSideStyle Left { get; set; } = new BorderSideStyle();

        public BorderSideStyle Right { get; set; } = new BorderSideStyle();

        public BorderSideStyle Top { get; set; } = new BorderSideStyle();

        public BorderSideStyle Bottom { get; set; } = new BorderSideStyle();

        public ImageScale ImageScale { get; set; }

        /// <summary>
        /// Gets the sum of the left + right border sizes.
        /// </summary>
        public int Width => Left.Width + Right.Width;

        /// <summary>
        /// Gets the sum of the top + bottom border sizes.
        /// </summary>
        public int Height => Top.Width + Bottom.Width;

        public static bool Equals(BorderStyle a, BorderStyle b)
        {
            if (a == null && b == null)
            {
                return true;
            }

            if (a == null || b == null)
            {
                return false;
            }

            if (!ImageSource.Equals(a.Image, b.Image))
            {
                return false;
            }

            if (!LayoutBox.Equals(a.ImageSlice, b.ImageSlice))
            {
                return false;
            }

            if (!BorderSideStyle.Equals(a.Left, b.Left))
            {
                return false;
            }

            if (!BorderSideStyle.Equals(a.Right, b.Right))
            {
                return false;
            }

            if (!BorderSideStyle.Equals(a.Top, b.Top))
            {
                return false;
            }

            if (!BorderSideStyle.Equals(a.Bottom, b.Bottom))
            {
                return false;
            }

            if (a.ImageScale != b.ImageScale)
            {
                return false;
            }

            return true;
        }
    }
}
