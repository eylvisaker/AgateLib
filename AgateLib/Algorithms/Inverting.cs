//
//    Copyright (c) 2006-2017 Erik Ylvisaker
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

namespace AgateLib.Mathematics.Algorithms
{
	/// <summary>
	/// Contains algorithms for inverting a function.
	/// </summary>
	public class Inverting
	{
		/// <summary>
		/// Uses binary search to find x such that func(x) == targetVal
		/// </summary>
		/// <param name="func"></param>
		/// <param name="targetVal"></param>
		/// <param name="initialPt"></param>
		/// <param name="itermax"></param>
		/// <returns></returns>
		public static double IterateInvert(Func<double, double> func, double targetVal, double initialPt = 0, int itermax = 500)
		{
			bool hasLower = false;
			bool hasUpper = false;
			int iter = 0;

			var p1 = new Pair<double, double>(initialPt, func(initialPt));
			Pair<double, double> lowerVal, upperVal;

			int sign;

			if (p1.Second < targetVal)
			{
				hasLower = true;
				upperVal = lowerVal = p1;

				sign = 1;
			}
			else if (p1.Second > targetVal)
			{
				hasUpper = true;
				lowerVal = upperVal = p1;

				sign = -1;
			}
			else
				return initialPt;

			int step = 1 * sign;

			while ((hasUpper && hasLower) == false)
			{
				p1.First += step;
				p1.Second = func(p1.First);

				if (p1.Second < targetVal)
				{
					lowerVal = p1;
					hasLower = true;
				}
				else if (p1.Second > targetVal)
				{
					upperVal = p1;
					hasUpper = true;
				}
				else
					return p1.First;

				iter++;
				step *= 2;

				if (iter > itermax)
					throw new InvalidOperationException("No solution found.");

			}

			while (Math.Abs(p1.Second - targetVal) > 1e-7)
			{
				if (iter % 3 == 0)
				{
					p1.First = (upperVal.First + lowerVal.First) / 2;
					p1.Second = func(p1.First);
				}
				else
				{
					double invslope = (upperVal.First - lowerVal.First) /
									  (upperVal.Second - lowerVal.Second);

					p1.First = lowerVal.First + invslope * (targetVal - lowerVal.Second);
					p1.Second = func(p1.First);
				}

				if (p1.Second < targetVal)
					lowerVal = p1;
				else if (p1.Second > targetVal)
					upperVal = p1;
				else
					return p1.First;

				iter++;
				if (iter > itermax)
					throw new InvalidOperationException("No solution found.");
			}

			return p1.First;
		}
	}
}
