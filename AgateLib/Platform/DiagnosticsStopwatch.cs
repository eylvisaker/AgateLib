﻿//     The contents of this file are subject to the Mozilla Public License
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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//

using System;
using Watch = System.Diagnostics.Stopwatch;

namespace AgateLib.Platform
{
	/// <summary>
	/// Class which represents a StopWatch.
	/// A StopWatch can be paused and reset independently of other
	/// StopWatches.
	/// </summary>
	public class DiagnosticsStopwatch : IDisposable, AgateLib.Platform.IStopwatch
	{
		Watch watch = new Watch();
		int mPause = 1;

		/// <summary>
		/// Constructs a timer object, and immediately begins 
		/// keeping track of time.
		/// </summary>
		public DiagnosticsStopwatch()
			: this(true)
		{
			watch.Start();
		}
		/// <summary>
		/// Constructs a timer object.
		/// If the timer starts paused, a call to Resume() must be made
		/// for it to begin keeping track of time.
		/// </summary>
		/// <param name="autostart">Pass true to immediately begin keeping track of time.
		/// False to pause the timer initially.</param>
		public DiagnosticsStopwatch(bool autostart)
		{
			Timing.PauseAllTimersEvent += Pause;
			Timing.ResumeAllTimersEvent += Resume;
			Timing.ForceResumeAllTimersEvent += ForceResume;

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
				Timing.PauseAllTimersEvent -= Pause;
				Timing.ResumeAllTimersEvent -= Resume;
				Timing.ForceResumeAllTimersEvent -= ForceResume;
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
