using System;

namespace AgateLib.Configuration
{
	public class AgateAppClock : IClock
	{
		public TimeSpan CurrentTime => AgateApp.AppClockTime();
	}
}