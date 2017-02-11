using System;
using MathNet.Numerics.LinearAlgebra;

namespace RigidBodyDynamics
{
	public class SimpleJacobianDifferentiator
	{
		private const int historySize = 2;
		private ValueHistory<Matrix<float>> history;
		private Matrix<float> derivative;

		public int Rows { get; set; }

		public int Columns { get; set; }

		public Matrix<float> Current => history.Current;

		public void Advance()
		{
			FixHistory();

			history.Cycle();

			var current = history.Current;

			Initialize(ref current);

			history.Current = current;
		}

		public Matrix<float> ComputeDerivative(float dt)
		{
			Initialize(ref derivative);

			var last = history[1];

			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					derivative[i, j] = (Current[i, j] - last[i, j]) / dt;
				}
			}

			return derivative;
		}

		/// <summary>
		/// If any parameters have changed, redo history.
		/// </summary>
		private void FixHistory()
		{
			if (history == null || history.Size != historySize)
			{
				history = new ValueHistory<Matrix<float>> { Size = 2 };

				history.Current = Matrix<float>.Build.Dense(Rows, Columns);
				history.Cycle();

				history.Current = Matrix<float>.Build.Dense(Rows, Columns);
			}
		}

		/// <summary>
		/// Called at the beginning of Derivative only. Initializes the 
		/// Jacobian matrix to be the right size and zeroed out.
		/// </summary>
		/// <returns></returns>
		public void Initialize(ref Matrix<float> jacobian)
		{
			if (jacobian == null ||
				jacobian.RowCount != Rows ||
				jacobian.ColumnCount != Columns)
			{
				jacobian = Matrix<float>.Build.Dense(Rows, Columns);
			}
			else
			{
				for (int i = 0; i < jacobian.RowCount; i++)
				{
					for (int j = 0; j < jacobian.ColumnCount; j++)
					{
						jacobian[i, j] = 0;
					}
				}
			}
		}
	}
}