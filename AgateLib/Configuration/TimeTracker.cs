using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
