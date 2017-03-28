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
		private static IStopwatch mAppTimer;

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

			mAppTimer = CreateStopWatch();
			mAppTimer.Resume();
		}

		public static IStopwatch CreateStopWatch()
		{
			return AgateApp.State.Factory.PlatformFactory.CreateStopwatch();
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