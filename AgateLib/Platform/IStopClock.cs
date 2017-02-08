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
		TimeSpan Elapsed { get; }

		/// <summary>
		/// The current time shown on the clock face.
		/// </summary>
		TimeSpan CurrentTime { get; }
	}
}