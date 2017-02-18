﻿using System;

namespace AgateLib.Platform
{
	/// <summary>
	/// A clock which can be used to track the passage of time.
	/// </summary>
	public interface IClock
	{
		/// <summary>
		/// Returns the current time on the clock.
		/// </summary>
		TimeSpan CurrentTime { get; }
	}
}