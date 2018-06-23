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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Mathematics
{
	/// <summary>
	/// Provides single precision math operations.
	/// </summary>
	public static class MathF
	{
		public const float PI = 3.14159265359f;
		public const float TwoPI = 6.28318530718f;


		/// <summary>
		/// Single precision sine.
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static float Sin(float angle)
		{
			return (float)Math.Sin(angle);
		}

		/// <summary>
		/// Single precision cosine.
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static float Cos(float angle)
		{
			return (float)Math.Cos(angle);
		}

        /// <summary>
        /// Single precision square root.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Sqrt(float value)
        {
            return (float)Math.Sqrt(value);
        }

        /// <summary>
        /// Floating precision exponential function.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static float Pow(float x, float y)
		{
			return (float)Math.Pow(x, y);
		}
	}
}
