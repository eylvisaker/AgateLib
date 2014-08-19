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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Mathematics
{
	/// <summary>
	/// Extra math functions which are useful but not found in System.Math.
	/// </summary>
	public static class MathFunctions
	{
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
	}
}
