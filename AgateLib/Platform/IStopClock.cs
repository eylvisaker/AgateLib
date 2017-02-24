using System;

namespace AgateLib.Platform
{
	/// <summary>
	/// Provides an interface for a clock which only advances according to an external control.
	/// </summary>
	public interface IStopClock
	{
		/// <summary>
		/// The amount of time which passed since the last time the clock advanced.
		/// </summary>
		ClockTimeSpan Elapsed { get; }

		/// <summary>
		/// The current time shown on the clock face.
		/// </summary>
		ClockTimeSpan CurrentTime { get; }
	}
}