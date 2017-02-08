using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Configuration;

namespace AgateLib.Platform
{
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

		public TimeSpan DeltaTime { get; private set; }

		public TimeSpan CurrentTime => TimeSpan.FromMilliseconds(currentTimeMs);

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

				if ((now - fpsStart) > 1000 / FpsUpdateFrequency)
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

			DeltaTime = TimeSpan.FromMilliseconds(timePassedMs);
		}
	}
}
