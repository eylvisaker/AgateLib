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

namespace AgateLib.Platform
{
	/// <summary>
	/// Structure which represents a time span on an IStopClock object.
	/// </summary>
	public struct ClockTimeSpan
	{
		/// <summary>
		/// Constructs a new ClockTimeSpan object representing the amount of seconds.
		/// </summary>
		/// <param name="timePassedSeconds"></param>
		/// <returns></returns>
		public static ClockTimeSpan FromSeconds(double timePassedSeconds)
		{
			return new ClockTimeSpan(timePassedSeconds);
		}

		/// <summary>
		/// Constructs a new ClockTimeSpan object representing the amount of milliseconds.
		/// </summary>
		/// <param name="timePassedMs"></param>
		/// <returns></returns>
		public static ClockTimeSpan FromMilliseconds(double timePassedMs)
		{
			return new ClockTimeSpan(timePassedMs * 0.001);
		}

		/// <summary>
		/// Returns an empty ClockTimeSpan object.
		/// </summary>
		public static ClockTimeSpan Zero => new ClockTimeSpan();

		private double timeSpanSeconds;

		private ClockTimeSpan(double timeSpanSeconds)
		{
			this.timeSpanSeconds = timeSpanSeconds;
		}

		/// <summary>
		/// Gets the total amount of time in milliseconds.
		/// </summary>
		public double TotalMilliseconds => timeSpanSeconds * 1000;

		/// <summary>
		/// Gets the total amount of time in seconds.
		/// </summary>
		public double TotalSeconds => timeSpanSeconds;

		/// <summary>
		/// Converts a ClockTimeSpan to a TimeSpan object.
		/// </summary>
		/// <param name="source"></param>
		public static explicit operator TimeSpan(ClockTimeSpan source)
		{
			return TimeSpan.FromMilliseconds(source.TotalMilliseconds);
		}
	}
}