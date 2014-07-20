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
using Watch = System.Diagnostics.Stopwatch;

namespace AgateLib
{
	/// <summary>
	/// Static class which handles timing.  This is often used
	/// to update object positions and animations.
	/// </summary>
	public static class Timing
	{
		private static StopWatch mAppTimer = new StopWatch();

		private delegate void TimerDelegate();

		private static event TimerDelegate PauseAllTimersEvent;
		private static event TimerDelegate ResumeAllTimersEvent;
		private static event TimerDelegate ForceResumeAllTimersEvent;

		static Timing()
		{
			// Display the timer frequency and resolution.
			if (Watch.IsHighResolution)
			{
				Console.WriteLine("Operations timed using the system's high-resolution performance counter.");
			}
			else
			{
				Console.WriteLine("Operations timed using the DateTime class.");
			}

			long frequency = Watch.Frequency;
			Console.WriteLine("  Timer frequency in ticks per second = {0}",
				frequency);
			long nanosecPerTick = (1000L * 1000L * 1000L) / frequency;
			Console.WriteLine("  Timer is accurate within {0} nanoseconds",
				nanosecPerTick);

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

		/// <summary>
		/// Class which represents a StopWatch.
		/// A StopWatch can be paused and reset independently of other
		/// StopWatches.
		/// </summary>
		public class StopWatch : IDisposable
		{
			Watch watch = new Watch();
			int mPause = 1;

			/// <summary>
			/// Constructs a timer object, and immediately begins 
			/// keeping track of time.
			/// </summary>
			public StopWatch()
				: this(true)
			{
			}
			/// <summary>
			/// Constructs a timer object.
			/// If the timer starts paused, a call to Resume() must be made
			/// for it to begin keeping track of time.
			/// </summary>
			/// <param name="autostart">Pass true to immediately begin keeping track of time.
			/// False to pause the timer initially.</param>
			public StopWatch(bool autostart)
			{
				PauseAllTimersEvent += Pause;
				ResumeAllTimersEvent += Resume;
				ForceResumeAllTimersEvent += ForceResume;

				if (autostart)
				{
					watch.Start();
					mPause = 0;
				}
			}

			/// <summary>
			/// Destroys this timer.
			/// </summary>
			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}
			/// <summary>
			/// Removes this timer from events.
			/// </summary>
			/// <param name="manual"></param>
			protected virtual void Dispose(bool manual)
			{
				if (manual)
				{
					PauseAllTimersEvent -= Pause;
					ResumeAllTimersEvent -= Resume;
					ForceResumeAllTimersEvent -= ForceResume;
				}
			}

			/// <summary>
			/// Returns the number of seconds since the timer started.
			/// </summary>
			public double TotalSeconds
			{
				get { return (double)watch.ElapsedTicks / (double)Watch.Frequency; }
			}

			/// <summary>
			/// Returns the number of ticks (milliseconds) since the timer started.
			/// </summary>
			public double TotalMilliseconds
			{
				get { return 1000.0 * TotalSeconds; }
			}

			/// <summary>
			/// Resets the timer to zero.
			/// </summary>
			public void Reset()
			{
				watch = new System.Diagnostics.Stopwatch();
				if (mPause <= 0)
					watch.Start();
			}

			/// <summary>
			/// Increments the pause counter.
			/// If the counter is greater than zero, the timer won't advance.
			/// </summary>
			public void Pause()
			{
				watch.Stop();
				mPause += 1;
			}
			/// <summary>
			/// Decrements the pause counter.
			/// If the pause counter is zero the timer will begin advancing.
			/// </summary>
			public void Resume()
			{
				mPause -= 1;

				if (mPause < 0)
					mPause = 0;

				if (mPause == 0)
					watch.Start();
			}


			/// <summary>
			/// Sets the pause counter to zero, causing the timer to begin advancing
			/// regardless of how many calls to Pause() are made.
			/// </summary>
			public void ForceResume()
			{
				if (mPause <= 0)
					return;

				mPause = 0;
				Resume();
			}

			/// <summary>
			/// Gets whether or not this StopWatch is paused.
			/// </summary>
			public bool IsPaused
			{
				get { return mPause > 0; }
			}
		}
	}
}