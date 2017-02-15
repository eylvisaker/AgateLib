using System;

namespace AgateLib.Platform
{
	/// <summary>
	/// Structure which represents a time span on an IStopClock object.
	/// </summary>
	public struct ClockTimeSpan
	{
		/// <summary>
		/// Constructs a new ClockTimeSpan object representing the amount of seconds.
		/// </summary>
		/// <param name="timePassedSeconds"></param>
		/// <returns></returns>
		public static ClockTimeSpan FromSeconds(double timePassedSeconds)
		{
			return new ClockTimeSpan(timePassedSeconds);
		}

		/// <summary>
		/// Constructs a new ClockTimeSpan object representing the amount of milliseconds.
		/// </summary>
		/// <param name="timePassedMs"></param>
		/// <returns></returns>
		public static ClockTimeSpan FromMilliseconds(double timePassedMs)
		{
			return new ClockTimeSpan(timePassedMs * 0.001);
		}

		/// <summary>
		/// Returns an empty ClockTimeSpan object.
		/// </summary>
		public static ClockTimeSpan Zero => new ClockTimeSpan();

		private double timeSpanSeconds;

		private ClockTimeSpan(double timeSpanSeconds)
		{
			this.timeSpanSeconds = timeSpanSeconds;
		}

		/// <summary>
		/// Gets the total amount of time in milliseconds.
		/// </summary>
		public double TotalMilliseconds => timeSpanSeconds * 1000;

		/// <summary>
		/// Gets the total amount of time in seconds.
		/// </summary>
		public double TotalSeconds => timeSpanSeconds;

		/// <summary>
		/// Converts a ClockTimeSpan to a TimeSpan object.
		/// </summary>
		/// <param name="source"></param>
		public static explicit operator TimeSpan(ClockTimeSpan source)
		{
			return TimeSpan.FromMilliseconds(source.TotalMilliseconds);
		}
	}
}