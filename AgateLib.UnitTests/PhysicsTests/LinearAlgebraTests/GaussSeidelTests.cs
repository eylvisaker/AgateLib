using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Physics.LinearAlgebra;
using FluentAssertions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;
using Xunit;

namespace AgateLib.Tests.PhysicsTests.LinearAlgebraTests
{
    public class GaussSeidelTests
    {
        private GaussSeidelAlgorithm gaussSeidel;

        public GaussSeidelTests()
        {
            gaussSeidel = new GaussSeidelAlgorithm();
        }

        private Matrix<float> DefaultMax(int size)
        {
            return DenseMatrix.Build.DenseOfRowArrays(
                Enumerable.Range(0, size).Select(i => new[] { float.MaxValue }));
        }

        private Matrix<float> DefaultMin(int size)
        {
            return DenseMatrix.Build.DenseOfRowArrays(
                Enumerable.Range(0, size).Select(i => new[] { float.MinValue }));
        }

        [Fact]
        public void GS_TwoByTwoTest()
        {
            Matrix<float> A = DenseMatrix.OfArray(new float[,]
            {
                {2, 1},
                {1, 4}
            });

            Matrix<float> B = DenseMatrix.OfArray(new float[,]
            {
                {7},
                {14},
            });

            var result = gaussSeidel.SolveProjected(A, B, DefaultMin(2), DefaultMax(2));

            gaussSeidel.Converged.Should().BeTrue("Iterations failed to converge.");
            result.RowCount.Should().Be(2);
            result.ColumnCount.Should().Be(1);
            result[0, 0].Should().BeApproximately(2, gaussSeidel.Tolerance);
            result[1, 0].Should().BeApproximately(3, gaussSeidel.Tolerance);
        }

        [Fact]
        public void GS_ThreeByThreeSingularTest()
        {
            Matrix<float> A = DenseMatrix.OfArray(new float[,]
            {
                { 2, 1, 0 },
                { 1, 4, 0 },
                { 0, 0, 0 }
            });

            Matrix<float> B = DenseMatrix.OfArray(new float[,]
            {
                { 7 },
                { 14 },
                { 5 },
            });

            var result = gaussSeidel.SolveProjected(A, B, DefaultMin(3), DefaultMax(3));

            gaussSeidel.Converged.Should().BeTrue("Iterations failed to converge.");
            result.RowCount.Should().Be(3);
            result.ColumnCount.Should().Be(1);
            result[0, 0].Should().BeApproximately(2, gaussSeidel.Tolerance);
            result[1, 0].Should().BeApproximately(3, gaussSeidel.Tolerance);
            result[2, 0].Should().BeApproximately(0, gaussSeidel.Tolerance);
        }

        [Fact]
        public void GS_ThreeByThreeTopSingularTest()
        {
            Matrix<float> A = DenseMatrix.OfArray(new float[,]
            {
                { 0, 0, 0 },
                { 0, 2, 1 },
                { 0, 1, 4 }
            });

            Matrix<float> B = DenseMatrix.OfArray(new float[,]
            {
                { 5 },
                { 7 },
                { 14 },
            });

            var result = gaussSeidel.SolveProjected(A, B, DefaultMin(3), DefaultMax(3));

            gaussSeidel.Converged.Should().BeTrue("Iterations failed to converge.");
            result.RowCount.Should().Be(3);
            result.ColumnCount.Should().Be(1);
            result[0, 0].Should().BeApproximately(0, gaussSeidel.Tolerance);
            result[1, 0].Should().BeApproximately(2, gaussSeidel.Tolerance);
            result[2, 0].Should().BeApproximately(3, gaussSeidel.Tolerance);
        }

        [Fact]
        public void GS_ThreeByThreeCenterSingularTest()
        {
            Matrix<float> A = DenseMatrix.OfArray(new float[,]
            {
                { 2, 0, 1 },
                { 0, 0, 0 },
                { 1, 0, 4 }
            });

            Matrix<float> B = DenseMatrix.OfArray(new float[,]
            {
                { 7 },
                { 5 },
                { 14 },
            });

            var result = gaussSeidel.SolveProjected(A, B, DefaultMin(3), DefaultMax(3));

            gaussSeidel.Converged.Should().BeTrue("Iterations failed to converge.");
            result.RowCount.Should().Be(3);
            result.ColumnCount.Should().Be(1);
            result[0, 0].Should().BeApproximately(2, gaussSeidel.Tolerance);
            result[1, 0].Should().BeApproximately(0, gaussSeidel.Tolerance);
            result[2, 0].Should().BeApproximately(3, gaussSeidel.Tolerance);
        }
    }
}
