using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Configuration
{
	class TimeTracker
	{
		private const double minDeltaTime = 0.0000001;

		private bool mInFrame = false;
		private double mLastTime;
		private bool mRanOnce = false;

		private double mFPSStart;
		private int mFrames = 0;
		private double mFPS = 0;
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

		public double FramesPerSecond => mFPS;

		/// <summary>
		/// Advances the time tracker and computes the amount of time passed since 
		/// </summary>
		public void Advance()
		{
			double now = clock.CurrentTime.TotalMilliseconds;
			double millisecondsPassed = 0;

			mFrames++;

			if (mRanOnce)
			{
				millisecondsPassed = now - mLastTime;
				mLastTime = now;

				if ((now - mFPSStart) > 1000 / FpsUpdateFrequency)
				{
					double time = (now - mFPSStart) * 0.001;

					// average current framerate with that of the last update
					mFPS = (mFrames / time) * 0.8 + mFPS * 0.2;

					mFPSStart = now;
					mFrames = 0;
				}

				// hack to make sure delta time is never zero.
				if (millisecondsPassed < minDeltaTime)
				{
					millisecondsPassed = minDeltaTime;
				}
			}
			else
			{
				mLastTime = now;

				mFPSStart = now;
				mFrames = 0;

				mRanOnce = true;
			}

			DeltaTime = TimeSpan.FromMilliseconds(millisecondsPassed);
		}

	}
}
