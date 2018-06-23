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

using Microsoft.Xna.Framework;

namespace AgateLib.Mathematics.Geometry
{
    public struct LineSegment
    {
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }

        /// <summary>
        /// Returns the displacement of this vector, equivalent to End - Start.
        /// </summary>
        public Vector2 Displacement => End - Start;

        /// <summary>
        /// The point in the center of the line segment.
        /// </summary>
        public Vector2 Center => (End + Start) / 2;

        /// <summary>
        /// Gets a vector which is normal to this line.
        /// </summary>
        public Vector2 Normal
        {
            get
            {
                var result = new Vector2(Displacement.Y, -Displacement.X);
                result.Normalize();
                return result;
            }
        }

        /// <summary>
        /// Returns the value of Y one the line at X.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public float Y(float x)
		{
			var slope = (End.Y - Start.Y) / (End.X - Start.X);
			var run = x - Start.X;
			var rise = Start.Y + run * slope;

			return rise;
		}
	}
}
