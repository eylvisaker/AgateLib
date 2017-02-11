namespace RigidBodyDynamics
{
	/// <summary>
	/// Represents a Jacobian matrix.
	/// </summary>
	public class Jacobian
	{
		private readonly float[,] data;

		public Jacobian(int rows, int columns)
		{
			Rows = rows;
			Columns = columns;

			data = new float[rows,columns];
		}

		public int Rows { get; }

		public int Columns { get; }

		public float this[int row, int column]
		{
			get { return data[row, column]; }
			set { data[row, column] = value; }
		}
	}
}