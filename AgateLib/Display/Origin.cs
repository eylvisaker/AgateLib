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

using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using System;

namespace AgateLib.Display
{
    /// <summary>
    /// Static class which performs necessary calculates based on OriginAlignment
    /// values.
    /// </summary>
    public static class Origin
    {
        /// <summary>
        /// Return the point on the rectangle that corresponds to the specified alignemtn.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Point OnRect(this OriginAlignment origin, Rectangle rect)
        {
            var calcPoint = Calc(origin, rect.Size);

            return new Point(calcPoint.X + rect.X,
                             calcPoint.Y + rect.Y);
        }

        /// <summary>
        /// Returns a point which should be subtracted from the interpreted
        /// point to get the top-left position.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Point Calc(this OriginAlignment origin, Size size)
        {
            Point result = new Point();
            int tb = ((int)origin) & 0xf0;
            int lr = ((int)origin) & 0x0f;
            int width = Math.Abs(size.Width);
            int height = Math.Abs(size.Height);

            if (lr == 0x01)
                result.X = 0;
            else if (lr == 0x02)
                result.X = width / 2;
            else if (lr == 0x03)
            {
                result.X = width;

            }

            if (tb == 0x10)
                result.Y = 0;
            else if (tb == 0x20)
                result.Y = height / 2;
            else if (tb == 0x30)
            {
                result.Y = height;
            }

            return result;
        }

        /// <summary>
        /// Returns a point which should be subtracted from the interpreted
        /// point to get the top-left position.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Vector2 CalcF(this OriginAlignment origin, Size size)
        {
            Point result = Calc(origin, size);

            return new Vector2(result.X, result.Y);
        }

        /// <summary>
        /// Returns a point which should be subtracted from the interpreted
        /// point to get the top-left position.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Vector2 CalcF(this OriginAlignment origin, SizeF size)
        {
            Vector2 result = new Vector2();
            int tb = ((int)origin) & 0xf0;
            int lr = ((int)origin) & 0x0f;

            if (lr == 0x01)
                result.X = 0;
            else if (lr == 0x02)
                result.X = size.Width / 2;
            else if (lr == 0x03)
                result.X = size.Width;

            if (tb == 0x10)
                result.Y = 0;
            else if (tb == 0x20)
                result.Y = size.Height / 2;
            else if (tb == 0x30)
                result.Y = size.Height;

            return result;
        }

        /// <summary>
        /// Returns a point which should be subtracted from the interpreted
        /// point to get the top-left position.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="size"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Calc(this OriginAlignment origin, Size size, out int x, out int y)
        {
            Point ret = Calc(origin, size);

            x = ret.X;
            y = ret.Y;

        }

        /// <summary>
        /// Modifies the rectangle by taking its Location and converting it
        /// using Calc so that the rectangle outlines the actual destination.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Rectangle CalcRect(this OriginAlignment origin, Rectangle rect)
        {
            return CalcRect(origin, rect, rect.Size);
        }

        /// <summary>
        /// Modifies the rectangle by taking its Location and converting it
        /// using Calc so that the rectangle outlines the actual destination.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="rect"></param>
        /// <param name="effectiveSize"></param>
        /// <returns></returns>
        public static Rectangle CalcRect(this OriginAlignment origin, Rectangle rect, Size effectiveSize)
        {
            Point pt = Origin.Calc(origin, effectiveSize);
            Rectangle result = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);

            result.X -= pt.X;
            result.Y -= pt.Y;

            return result;
        }
    };

}
