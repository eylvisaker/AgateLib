using System;

namespace AgateLib.Configuration
{
	/// <summary>
	/// A clock which returns the application time.
	/// </summary>
	public class AgateAppClock : IClock
	{
		public TimeSpan CurrentTime => AgateApp.AppClockTime();
	}
}