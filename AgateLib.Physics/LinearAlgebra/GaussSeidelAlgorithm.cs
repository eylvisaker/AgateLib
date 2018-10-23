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
using AgateLib.Quality;
using MathNet.Numerics.LinearAlgebra;

namespace AgateLib.Physics.LinearAlgebra
{
	public class GaussSeidelAlgorithm
	{
		private float tolerance = 1e-6f;
		private int maxIter = 50;
		private float mixing = 0.2f;

		/// <summary>
		/// Zero-tolerance value for small numbers.
		/// </summary>
		public float Tolerance
		{
			get => tolerance;
			set
			{
				Require.ArgumentInRange(value > 0, nameof(Tolerance), $"{nameof(Tolerance)} must be positive.");
				tolerance = value;
			}
		}


		/// <summary>
		/// Maximum number of iterations.
		/// </summary>
		public int MaxIterations
		{
			get => maxIter;
			set
			{
				Require.ArgumentInRange(value > 0, nameof(MaxIterations), $"{nameof(MaxIterations)} must be positive.");
				maxIter = value;
			}
		}

		/// <summary>
		/// The number of iterations the last call to solve took.
		/// </summary>
		public int IterationCount { get; private set; }

		/// <summary>
		/// True if the last calculation converged.
		/// </summary>
		public bool Converged { get; private set; }

		/// <summary>
		/// The amount of error that existed in the last calculation.
		/// </summary>
		public float ConvergenceError { get; private set; }

		/// <summary>
		/// The amout of the previous iteration to mix in.
		/// </summary>
		public float Mixing
		{
			get => mixing;
			private set => mixing = value;
		}

		/// <summary>
		/// Returns true if the matrix A is singular. In this case, resulting parameters are zero
		/// for values associated with rows that have a 0 on the diagonal element.
		/// </summary>
		public bool Singular { get; private set; }

		/// <summary>
		/// Performs a projected GaussSeidel algorithm.
		/// </summary>
		/// <param name="A"></param>
		/// <param name="B"></param>
		/// <returns></returns>
		/// <remarks>
		/// Method is described here:
		/// https://en.wikipedia.org/wiki/Gauss%E2%80%93Seidel_method
		/// </remarks>
		public Matrix<float> SolveProjected(Matrix<float> A, Matrix<float> B, Matrix<float> lowerLimit, Matrix<float> upperLimit)
		{
			Matrix<float> result = Matrix<float>.Build.Dense(A.RowCount, 1);
			int[] pivot = Enumerable.Range(0, result.RowCount).ToArray();
			int singularIndex = result.RowCount;

			// perform pivoting if any diagonal elements are zero.
			//for (int i = 0; i < result.RowCount; i++)
			//{
			//	if (Math.Abs(A[i, i]) < tolerance)
			//	{
			//		int swapIndex = i;
			//		float maxValue = tolerance;

			//		for (int j = i + 1; j < A.RowCount; j++)
			//		{
			//			if (Math.Abs(A[j, i]) > maxValue)
			//			{
			//				maxValue = Math.Abs(A[j, i]);
			//				swapIndex = j;
			//			}
			//		}

			//		if (swapIndex == i)
			//		{
			//			// we have a singular matrix, so just move this row to the bottom.
			//			singularIndex--;
			//			swapIndex = singularIndex;
			//			Singular = true;
			//		}

			//		pivot[i] = swapIndex;
			//		pivot[swapIndex] = i;
			//	}
			//}

			float error;
			int iter = 0;

			do
			{
				error = 0;
				iter++;

				for (int i = 0; i < singularIndex; i++)
				{
					var oldElement = result[pivot[i], 0];
					var element = B[pivot[i], 0];

					if (Math.Abs(A[pivot[i], pivot[i]]) < tolerance)
					{
						element = 0;
					}
					else
					{
						for (int j = 0; j < i; j++)
						{
							element -= A[pivot[i], pivot[j]] * result[pivot[j], 0];
						}

						for (int j = i + 1; j < result.RowCount; j++)
						{
							element -= A[pivot[i], pivot[j]] * result[pivot[j], 0];
						}

						element /= A[pivot[i], pivot[i]];

						element = Math.Max(element, lowerLimit[pivot[i], 0]);
						element = Math.Min(element, upperLimit[pivot[i], 0]);
					}

					var newElement = element * (1 - mixing) + oldElement * mixing;
					error += Math.Abs(element - oldElement);
					result[pivot[i], 0] = newElement;
				}

			} while (error > tolerance && iter < maxIter);

			IterationCount = iter;
			Converged = error <= tolerance;
			ConvergenceError = error;

			return result;
		}
	}
}
