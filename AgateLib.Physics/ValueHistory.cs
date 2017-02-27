using AgateLib.Quality;

namespace AgateLib.Physics
{
	public class ValueHistory<T>
	{
		/// <summary>
		/// Most recent item is placed in history[0], oldest item is in history[history.Length - 1]
		/// </summary>
		private T[] history;
		private int historySize = 2;

		public ValueHistory()
		{
			Size = historySize;
		}

		public int Size
		{
			get { return historySize; }
			set
			{
				Require.ArgumentInRange(value > 0, nameof(Size), $"{nameof(Size)} must be positive.");

				historySize = value;

				var old = history;
				history = new T[Size];

				if (old != null)
				{
					for (int i = 0; i < old.Length && i < history.Length; i++)
					{
						history[i] = old[i];
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public T Current
		{
			get { return history[0]; }
			set { history[0] = value; }
		}

		public T this[int index] => history[index];

		public void Cycle()
		{
			T last = history[history.Length - 1];

			for (int i = history.Length - 1; i > 0; i--)
			{
				history[i] = history[i - 1];
			}

			history[0] = last;
		}
	}
}