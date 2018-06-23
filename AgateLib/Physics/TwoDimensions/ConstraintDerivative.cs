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

namespace AgateLib.Physics.TwoDimensions
{
	/// <summary>
	/// Compute the derivative of your constraint, with respect to each of the particle's values.
	/// </summary>
	public struct ConstraintDerivative
	{
		/// <summary>
		/// Compute the derivative of your constraint, with respect to each of the particle's values.
		/// </summary>
		/// <param name="respectToX">The derivative of your constraint with respect to the particle's 
		/// Position.X variable. Evaluate the derivative numerically at the particle's current position.</param>
		/// <param name="respectToY">The derivative of your constraint with respect to the particle's 
		/// Position.Y variable. Evaluate the derivative numerically at the particle's current position.</param>
		/// <param name="respectToAngle">The derivative of your constraint with respect to the particle's 
		/// angle variable. Evaluate the derivative numerically at the particle's current position.</param>
		public ConstraintDerivative(float respectToX, float respectToY, float respectToAngle)
		{
			RespectToX = respectToX;
			RespectToY = respectToY;
			RespectToAngle = respectToAngle;
		}

		/// <summary>
		/// The derivative of your constraint with respect to the particle's 
		/// Position.X variable. Evaluate the derivative numerically at the particle's current position.
		/// </summary>
		public float RespectToX;

		/// <summary>
		/// The derivative of your constraint with respect to the particle's 
		/// Position.Y variable. Evaluate the derivative numerically at the particle's current position.
		/// </summary>
		public float RespectToY;

		/// <summary>
		/// The derivative of your constraint with respect to the particle's 
		/// angle variable. Evaluate the derivative numerically at the particle's current position.
		/// </summary>
		public float RespectToAngle;

		/// <summary>
		/// Scales the values of the constraint.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static ConstraintDerivative operator *(float a, ConstraintDerivative b)
		{
			return new ConstraintDerivative(a * b.RespectToX, a * b.RespectToY, a * b.RespectToAngle);
		}

		/// <summary>
		/// Adds the value of two derivatives together.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static ConstraintDerivative operator +(ConstraintDerivative a, ConstraintDerivative b)
		{
			return new ConstraintDerivative(a.RespectToX + b.RespectToX, a.RespectToY + b.RespectToY, a.RespectToAngle + b.RespectToAngle);
		}
	}
}