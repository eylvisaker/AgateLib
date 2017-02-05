using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.Configuration
{
	[TestClass]
	public class TimeTrackerTests
	{
		class Clock : IClock
		{
			public TimeSpan CurrentTime { get; set; }
		}

		[TestMethod]
		public void AdvanceTest()
		{
			var clock = new Clock();
			var tracker = new TimeTracker(clock);

			clock.CurrentTime = TimeSpan.FromSeconds(1);
			tracker.Advance();

			clock.CurrentTime = TimeSpan.FromSeconds(2);
			tracker.Advance();

			Assert.AreEqual(1, tracker.DeltaTime.TotalSeconds, 0.001);
		}


		[TestMethod]
		public void FpsTest()
		{
			var clock = new Clock();
			var tracker = new TimeTracker(clock) { FpsUpdateFrequency = 20 };


			for (int i = 0; i < 40; i++)
			{
				clock.CurrentTime += TimeSpan.FromMilliseconds(40);
				tracker.Advance();
			}

			Assert.AreEqual(25, tracker.FramesPerSecond, 0.0001);
		}
	}
}
