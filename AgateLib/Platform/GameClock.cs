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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Configuration;
using AgateLib.Quality;

namespace AgateLib.Platform
{
	/// <summary>
	/// Class which represents a clock that can be used to track time passage for a game.
	/// This clock can be sped up and slowed down.
	/// </summary>
	public class GameClock : IStopClock
	{
		private readonly IStopClock parent;

		private double clockSpeed = 1;
		private double lastClockSpeedChangeTimeSeconds;
		private double timeOriginSeconds;

		/// <summary>
		/// Constructs a GameClock object.
		/// </summary>
		/// <param name="parent">The parent IStopClock object which will provide the clock times.</param>
		public GameClock(IStopClock parent)
		{
			this.parent = parent;

			lastClockSpeedChangeTimeSeconds = parent.CurrentTime.TotalSeconds;
		}

		/// <summary>
		/// The amount of time that passed on this clock since the last time the parent clock advanced.
		/// </summary>
		public ClockTimeSpan Elapsed => ClockTimeSpan.FromSeconds(
			parent.Elapsed.TotalSeconds * ClockSpeed);

		/// <summary>
		/// Gets or sets the relative speed this clock should operate at. A value of 0.5 will cause this clock
		/// to advance at half the rate of its parent.
		/// </summary>
		public double ClockSpeed
		{
			get { return clockSpeed; }
			set
			{
				Require.ArgumentInRange(value >= 0, nameof(ClockSpeed), "ClockSpeed must not be negative.");

				timeOriginSeconds = CurrentTime.TotalSeconds;

				clockSpeed = value;
				lastClockSpeedChangeTimeSeconds = parent.CurrentTime.TotalSeconds;
			}
		}

		/// <summary>
		/// The current time shown on the clock face.
		/// </summary>
		public ClockTimeSpan CurrentTime
		{
			get
			{
				double delta = parent.CurrentTime.TotalSeconds - lastClockSpeedChangeTimeSeconds;

				return ClockTimeSpan.FromSeconds(ClockSpeed * delta + timeOriginSeconds);
			}
		}
	}
}
