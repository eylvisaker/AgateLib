using AgateLib.Diagnostics;
//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Platform
{
	/// <summary>
	/// Static class which handles timing.  This is often used
	/// to update object positions and animations.
	/// </summary>
	public static class Timing
	{
		private static IStopWatch mAppTimer;

		public delegate void TimerDelegate();

		public static event TimerDelegate PauseAllTimersEvent;
		public static event TimerDelegate ResumeAllTimersEvent;
		public static event TimerDelegate ForceResumeAllTimersEvent;

		static Timing()
		{
			// Display the timer frequency and resolution.
			//if (Watch.IsHighResolution)
			//{
			//	Log.WriteLine("Operations timed using the system's high-resolution performance counter.");
			//}
			//else
			//{
			//	Log.WriteLine("Operations timed using the DateTime class.");
			//}

			//long frequency = Watch.Frequency;
			//Console.WriteLine("  Timer frequency in ticks per second = {0}",
			//	frequency);
			//long nanosecPerTick = (1000L * 1000L * 1000L) / frequency;
			//Console.WriteLine("  Timer is accurate within {0} nanoseconds",
			//	nanosecPerTick);

		}
		/// <summary>
		/// Returns the number of seconds since the application started.
		/// </summary>
		public static double TotalSeconds
		{
			get { return mAppTimer.TotalSeconds; }
		}
		/// <summary>
		/// Returns the number of milliseconds since the application started.
		/// </summary>
		public static double TotalMilliseconds
		{
			get { return mAppTimer.TotalMilliseconds; }
		}

		/// <summary>
		/// Increments the pause counter.
		/// If the counter is greater than zero, the timer won't advance.
		/// </summary>
		public static void Pause()
		{
			mAppTimer.Pause();
		}
		/// <summary>
		/// Decrements the pause counter.
		/// If the pause counter is zero the timer will begin advancing.
		/// </summary>
		public static void Resume()
		{
			mAppTimer.Resume();
		}

		/// <summary>
		/// Sets the pause counter to zero, causing the timer to begin advancing
		/// regardless of how many calls to Pause() are made.
		/// </summary>
		public static void ForceResume()
		{
			mAppTimer.ForceResume();
		}

		/// <summary>
		/// Calls Pause() on all timers, and fires the PauseAllTimersEvent.
		/// </summary>
		public static void PauseAllTimers()
		{
			PauseAllTimersEvent();
		}

		/// <summary>
		/// Calls Resume() on all timers, and fires the ResumeAllTimersEvent.
		/// </summary>
		public static void ResumeAllTimers()
		{
			ResumeAllTimersEvent();
		}

		/// <summary>
		/// Calls ForceResume on all timers, and fires the ResumeAllTimersEvent.
		/// You probably don't want to use this one much.
		/// </summary>
		public static void ForceResumeAllTimers()
		{
			ForceResumeAllTimersEvent();
			ResumeAllTimersEvent();
		}
	}
}