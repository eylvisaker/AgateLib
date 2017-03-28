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

namespace AgateLib.Platform
{
	/// <summary>
	/// Class which represents a master clock. This clock can be advanced at specified intervals
	/// and will track the amount of time passed for each interval.
	/// </summary>
	public class MasterClock : IStopClock
	{
		private readonly IClock root;

		private const double MinDeltaTime = 0.0000001;

		private double currentTimeMs;
		private double lastTime;
		private bool ranOnce;

		private double fpsStart;
		private int frames;
		private double fps;

		/// <summary>
		/// Constructs a MasterClock object.
		/// </summary>
		/// <param name="rootClock">The IClock object which provides a continuously running clock to draw from.</param>
		public MasterClock(IClock rootClock)
		{
			this.root = rootClock;
		}

		/// <summary>
		/// Gets or sets the number of times per second
		/// that the FPS should be updated. Larger values
		/// will update slower but the results will be more
		/// consistent. The default is 5 updates per second.
		/// </summary>
		public double FpsUpdateFrequency { get; set; } = 5;

		/// <summary>
		/// The amount of time elapsed between the last two calls to Advance().
		/// </summary>
		public ClockTimeSpan Elapsed { get; private set; }

		/// <summary>
		/// The current time shown on the clock face.
		/// </summary>
		public ClockTimeSpan CurrentTime => ClockTimeSpan.FromMilliseconds(currentTimeMs);

		/// <summary>
		/// Gets or sets the maximum time span returned by Elapsed. This allows the application
		/// to slow down when the system is too slow rather than having really choppy updates.
		/// </summary>
		public ClockTimeSpan MaxElapsed { get; set; } = ClockTimeSpan.FromMilliseconds(100);

		/// <summary>
		/// The number of frames per second. 
		/// </summary>
		public double FramesPerSecond => fps;

		/// <summary>
		/// Advances the time tracker and computes the amount of time passed since 
		/// </summary>
		public void Advance()
		{
			double now = root.CurrentTime.TotalMilliseconds;
			double timePassedMs = 0;

			frames++;

			if (ranOnce)
			{
				timePassedMs = now - lastTime;
				lastTime = now;

				currentTimeMs += timePassedMs;

				if (now - fpsStart > 1000 / FpsUpdateFrequency)
				{
					double time = (now - fpsStart) * 0.001;

					// average current framerate with that of the last update
					fps = (frames / time) * 0.8 + fps * 0.2;

					fpsStart = now;
					frames = 0;
				}

				// hack to make sure delta time is never zero.
				if (timePassedMs < MinDeltaTime)
				{
					timePassedMs = MinDeltaTime;
				}
			}
			else
			{
				lastTime = now;

				fpsStart = now;
				frames = 0;

				ranOnce = true;
			}

			if (timePassedMs > MaxElapsed.TotalMilliseconds)
				Elapsed = MaxElapsed;
			else
				Elapsed = ClockTimeSpan.FromMilliseconds(timePassedMs);
		}
	}
}
