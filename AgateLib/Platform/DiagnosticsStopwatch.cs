//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
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
