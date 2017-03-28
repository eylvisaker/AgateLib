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
using AgateLib.Platform;

namespace AgateLib.Configuration
{
	class TimeTracker
	{
		private const double MinDeltaTime = 0.0000001;

		private double lastTime;
		private bool ranOnce;

		private double fpsStart;
		private int frames;
		private double fps;
		private readonly IClock clock;

		public TimeTracker(IClock clock)
		{
			this.clock = clock;
		}

		/// <summary>
		/// Gets or sets the number of times per second
		/// that the FPS should be updated. Larger values
		/// will update slower but the results will be more
		/// consistent. The default is 5 updates per second.
		/// </summary>
		public double FpsUpdateFrequency { get; set; } = 5;
	
		public TimeSpan DeltaTime { get; private set; }

		public double FramesPerSecond => fps;

		/// <summary>
		/// Advances the time tracker and computes the amount of time passed since 
		/// </summary>
		public void Advance()
		{
			double now = clock.CurrentTime.TotalMilliseconds;
			double millisecondsPassed = 0;

			frames++;

			if (ranOnce)
			{
				millisecondsPassed = now - lastTime;
				lastTime = now;

				if ((now - fpsStart) > 1000 / FpsUpdateFrequency)
				{
					double time = (now - fpsStart) * 0.001;

					// average current framerate with that of the last update
					fps = (frames / time) * 0.8 + fps * 0.2;

					fpsStart = now;
					frames = 0;
				}

				// hack to make sure delta time is never zero.
				if (millisecondsPassed < MinDeltaTime)
				{
					millisecondsPassed = MinDeltaTime;
				}
			}
			else
			{
				lastTime = now;

				fpsStart = now;
				frames = 0;

				ranOnce = true;
			}

			DeltaTime = TimeSpan.FromMilliseconds(millisecondsPassed);
		}

	}
}
