using System;

using AgateLib.Algorithms;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Algorithms;
using AgateLib.Quality;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.Algorithms
{
	[TestClass]
	public class MathFunctionsTest
	{
		[TestMethod]
		public void ErfTest()
		{
			Assert.AreEqual(0, MathFunctions.Erf(0));
			Assert.AreEqual(1, MathFunctions.Erf(100), 0.000001);
			Assert.AreEqual(0.27632639016, MathFunctions.Erf(0.25), 0.000001);
			Assert.AreEqual(0.52049987781, MathFunctions.Erf(0.5), 0.000001);
			Assert.AreEqual(0.71115563365, MathFunctions.Erf(0.75), 0.000001);
		}
		[TestMethod]
		public void ErfcTest()
		{
			Assert.AreEqual(1, MathFunctions.Erfc(0));
			Assert.AreEqual(0, MathFunctions.Erfc(100), 0.000001);
			Assert.AreEqual(0.72367360984, MathFunctions.Erfc(0.25), 0.000001);
			Assert.AreEqual(0.47950012219, MathFunctions.Erfc(0.5), 0.000001);
			Assert.AreEqual(0.28884436635, MathFunctions.Erfc(0.75), 0.000001);
		}

		[TestMethod]
		public void IntegratedGaussianTest()
		{
			Assert.AreEqual(0.5, MathFunctions.IntegratedGaussian(0));
			Assert.AreEqual(1, MathFunctions.IntegratedGaussian(100), 0.000001);
			Assert.AreEqual(0.63816319508, MathFunctions.IntegratedGaussian(0.25), 0.000001);
		}

		[TestMethod]
		public void IterateInvertMaxIterationsTest()
		{
			AssertX.Throws<Exception>(() => Inverting.IterateInvert(x => x, 4, itermax: 1));
		}
		[TestMethod]
		public void InterateInvertTest()
		{
			Assert.AreEqual(0, Inverting.IterateInvert(x => 2 * x, 0), 0.000001);
			Assert.AreEqual(0, Inverting.IterateInvert(x => 2 * x, 0, 5), 0.000001);
			Assert.AreEqual(0, Inverting.IterateInvert(x => 2 * x, 0, -5), 0.000001);
			Assert.AreEqual(0, Inverting.IterateInvert(x => 2 * x, 0, 0), 0.000001);

			AssertX.Throws<Exception>(() => Inverting.IterateInvert(x => 1 + x * x, 0, 5));
		}

		[TestMethod]
		public void ErfOddTest()
		{
			for (double x = -4; x < 4; x += 0.2)
			{
				double left = MathFunctions.Erf(x);
				double right = MathFunctions.Erf(-x);

				Assert.AreEqual(-left, right, 0.0000001);
			}
		}
		[TestMethod]
		public void IterateInvertTest()
		{
			Assert.AreEqual(0, Inverting.IterateInvert(x => x, 0, 8), 0.000001);
			Assert.AreEqual(0, Inverting.IterateInvert(x => MathFunctions.Erf(x), 0, 8), 0.000001);
		}
	}
}
