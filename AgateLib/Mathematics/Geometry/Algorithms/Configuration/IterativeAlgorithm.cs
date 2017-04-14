using AgateLib.Quality;

namespace AgateLib.Mathematics.Geometry.Algorithms.Configuration
{
	public class IterativeAlgorithm
	{
		private int maxIterations = 50;
		private double tolerance = 1e-6;

		public int MaxIterations
		{
			get { return maxIterations; }
			set
			{
				Require.ArgumentInRange(value > 1, nameof(MaxIterations), "Value must be greater than 1.");
				maxIterations = value;
			}
		}

		public double Tolerance
		{
			get { return tolerance; }
			set
			{
				Require.ArgumentInRange(value > 0, nameof(Tolerance), "Value must be positive.");
				tolerance = value;
			}
		}

	}
}