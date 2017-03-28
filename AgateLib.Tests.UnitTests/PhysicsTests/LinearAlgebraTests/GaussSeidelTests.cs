using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using AgateLib.Physics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.PhysicsTests.LinearAlgebraTests
{
	[TestClass]
	public class GaussSeidelTests
	{
		private GaussSeidelAlgorithm gaussSeidel;

		[TestInitialize]
		public void Initialize()
		{
			gaussSeidel = new GaussSeidelAlgorithm();
		}

		private Matrix<double> DefaultMax(int size)
		{
			return DenseMatrix.Build.DenseOfRowArrays(
				Enumerable.Range(0, size).Select(i => new[] {double.MaxValue}));
		} 

		private Matrix<double> DefaultMin(int size)
		{
			return DenseMatrix.Build.DenseOfRowArrays(
				Enumerable.Range(0, size).Select(i => new[] { double.MinValue }));
		}

		[TestMethod]
		public void GS_TwoByTwoTest()
		{
			Matrix<double> A = DenseMatrix.OfArray(new double[,]
			{
				{2, 1},
				{1, 4}
			});

			Matrix<double> B = DenseMatrix.OfArray(new double[,]
			{
				{7},
				{14},
			});

			var result = gaussSeidel.SolveProjected(A, B, DefaultMin(2), DefaultMax(2));

			Assert.IsTrue(gaussSeidel.Converged, "Iterations failed to converge.");
			Assert.AreEqual(2, result.RowCount);
			Assert.AreEqual(1, result.ColumnCount);
			Assert.AreEqual(2, result[0, 0], gaussSeidel.Tolerance);
			Assert.AreEqual(3, result[1, 0], gaussSeidel.Tolerance);
		}
		
		[TestMethod]
		public void GS_ThreeByThreeSingularTest()
		{
			Matrix<double> A = DenseMatrix.OfArray(new double[,]
			{
				{ 2, 1, 0 },
				{ 1, 4, 0 },
				{ 0, 0, 0 }
			});

			Matrix<double> B = DenseMatrix.OfArray(new double[,]
			{
				{ 7 },
				{ 14 },
				{ 5 },
			});

			var result = gaussSeidel.SolveProjected(A, B, DefaultMin(3), DefaultMax(3));

			Assert.IsTrue(gaussSeidel.Converged, "Iterations failed to converge.");
			Assert.AreEqual(3, result.RowCount);
			Assert.AreEqual(1, result.ColumnCount);
			Assert.AreEqual(2, result[0, 0], gaussSeidel.Tolerance);
			Assert.AreEqual(3, result[1, 0], gaussSeidel.Tolerance);
			Assert.AreEqual(0, result[2, 0], gaussSeidel.Tolerance);
		}

		[TestMethod]
		public void GS_ThreeByThreeTopSingularTest()
		{
			Matrix<double> A = DenseMatrix.OfArray(new double[,]
			{
				{ 0, 0, 0 },
				{ 0, 2, 1 },
				{ 0, 1, 4 }
			});

			Matrix<double> B = DenseMatrix.OfArray(new double[,]
			{
				{ 5 },
				{ 7 },
				{ 14 },
			});

			var result = gaussSeidel.SolveProjected(A, B, DefaultMin(3), DefaultMax(3));

			Assert.IsTrue(gaussSeidel.Converged, "Iterations failed to converge.");
			Assert.AreEqual(3, result.RowCount);
			Assert.AreEqual(1, result.ColumnCount);
			Assert.AreEqual(0, result[0, 0], gaussSeidel.Tolerance);
			Assert.AreEqual(2, result[1, 0], gaussSeidel.Tolerance);
			Assert.AreEqual(3, result[2, 0], gaussSeidel.Tolerance);
		}

		[TestMethod]
		public void GS_ThreeByThreeCenterSingularTest()
		{
			Matrix<double> A = DenseMatrix.OfArray(new double[,]
			{
				{ 2, 0, 1 },
				{ 0, 0, 0 },
				{ 1, 0, 4 }
			});

			Matrix<double> B = DenseMatrix.OfArray(new double[,]
			{
				{ 7 },
				{ 5 },
				{ 14 },
			});

			var result = gaussSeidel.SolveProjected(A, B, DefaultMin(3), DefaultMax(3));

			Assert.IsTrue(gaussSeidel.Converged, "Iterations failed to converge.");
			Assert.AreEqual(3, result.RowCount);
			Assert.AreEqual(1, result.ColumnCount);
			Assert.AreEqual(2, result[0, 0], gaussSeidel.Tolerance);
			Assert.AreEqual(0, result[1, 0], gaussSeidel.Tolerance);
			Assert.AreEqual(3, result[2, 0], gaussSeidel.Tolerance);
		}
	}
}
