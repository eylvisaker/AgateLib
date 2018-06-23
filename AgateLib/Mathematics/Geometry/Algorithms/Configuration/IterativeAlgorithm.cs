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

using AgateLib.Quality;

namespace AgateLib.Mathematics.Geometry.Algorithms.Configuration
{
	public class IterativeAlgorithm
	{
		private int maxIterations = 50;
		private float tolerance = 1e-4f;

		public IterativeAlgorithm(int maxIterations = 50, float tolerance = 1e-4f)
		{
			this.maxIterations = maxIterations;
			this.tolerance = tolerance;
		}

		public int MaxIterations
		{
			get => maxIterations;
			set
			{
				Require.ArgumentInRange(value > 1, nameof(MaxIterations), "Value must be greater than 1.");
				maxIterations = value;
			}
		}

		public float Tolerance
		{
			get => tolerance;
			set
			{
				Require.ArgumentInRange(value > 0, nameof(Tolerance), "Value must be positive.");
				tolerance = value;
			}
		}

	}
}