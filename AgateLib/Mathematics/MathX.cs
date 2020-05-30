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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Mathematics
{
	/// <summary>
	/// Extra math functions which are useful but not found in System.Math.
	/// </summary>
	public static class MathX
	{
		/// <summary>
		/// Single precision value of pi.
		/// </summary>
		public const float PI = (float)Math.PI;
		
		/// <summary>
		/// Multiplication factor to convert a value in degrees to an equivalent number of radians.
		/// </summary>
		public const float DegreesToRadians = PI / 180.0f;

		/// <summary>
		/// Multiplication factor to convert a value in radians to an equivalent number of degrees.
		/// </summary>
		public const float RadiansToDegrees = 1 / DegreesToRadians;

		/// <summary>
		/// Computes the error function erf(x).
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static double Erf(double x)
		{
			// Approximate form given on
			// http://en.wikipedia.org/wiki/Error_function
			// constants
			const double a1 =  0.254829592;
			const double a2 = -0.284496736;
			const double a3 =  1.421413741;
			const double a4 = -1.453152027;
			const double a5 =  1.061405429;
			const double p  =  0.3275911;

			//# Save the sign of x
			int sign = Math.Sign(x);
			x = sign * x;

			// A & S 7.1.26
			double t = 1.0/(1.0 + p*x);
			double y = 1.0 - (((((a5*t + a4)*t) + a3)*t + a2)*t + a1)*t*Math.Exp(-x*x);

			return sign * y;
		}

		public static double Erfc(double x)
		{
			return 1 - Erf(x);
		}

		public static double IntegratedGaussian(double x)
		{
			return 0.5 + 0.5 * Erf(x);
		}

        /// <summary>
        /// Calculates the distance between a point and a rectangle.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        /// <remarks>
        /// This algorithm was adapted from work by Philip Peterson: http://wiki.unity3d.com/index.php/Distance_from_a_point_to_a_rectangle
        /// </remarks>
        public static float DistancePointToRectangle(Vector2 point, Rectangle rect)
        {
            //  Calculate a distance between a point and a rectangle.
            //  The area around/in the rectangle is defined in terms of
            //  several regions:
            //
            //  O--x
            //  |
            //  y
            //
            //
            //        I   |    II    |  III
            //      ======+==========+======   --yMin
            //       VIII |  IX (in) |  IV
            //      ======+==========+======   --yMax
            //       VII  |    VI    |   V
            //
            //
            //  Note that the +y direction is down because of Unity's GUI coordinates.

            if (point.X < rect.Left)
            { 
                // Region I, VIII, or VII
                if (point.Y < rect.Top)
                { 
                    // I
                    Vector2 diff = point - new Vector2(rect.Left, rect.Top);
                    return diff.Length();
                }
                else if (point.Y > rect.Bottom)
                { 
                    // VII
                    Vector2 diff = point - new Vector2(rect.Left, rect.Bottom);
                    return diff.Length();
                }
                else
                { 
                    // VIII
                    return rect.Left - point.X;
                }
            }
            else if (point.X > rect.Right)
            { 
                // Region III, IV, or V
                if (point.Y < rect.Top)
                { 
                    // III
                    Vector2 diff = point - new Vector2(rect.Right, rect.Top);
                    return diff.Length();
                }
                else if (point.Y > rect.Bottom)
                { 
                    // V
                    Vector2 diff = point - new Vector2(rect.Right, rect.Bottom);
                    return diff.Length();
                }
                else
                { 
                    // IV
                    return point.X - rect.Right;
                }
            }
            else
            { 
                // Region II, IX, or VI
                if (point.Y < rect.Top)
                { 
                    // II
                    return rect.Top - point.Y;
                }
                else if (point.Y > rect.Bottom)
                { 
                    // VI
                    return point.Y - rect.Bottom;
                }
                else
                { 
                    // IX
                    return 0f;
                }
            }
        }
    }
}
