using AgateLib.Platform;

namespace AgateLib
{
	/// <summary>
	/// Event arguments for scene updates.
	/// </summary>
	public class UpdateEventArgs
	{
		/// <summary>
		/// Constructs an UpdateEventArgs object.
		/// </summary>
		/// <param name="elapsed"></param>
		public UpdateEventArgs(ClockTimeSpan elapsed)
		{
			this.Elapsed = elapsed;
		}

		/// <summary>
		/// Elapsed time since the last time update was called.
		/// </summary>
		public ClockTimeSpan Elapsed { get; internal set; }
	}
}