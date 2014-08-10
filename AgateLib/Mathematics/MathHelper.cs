using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Mathematics
{
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
