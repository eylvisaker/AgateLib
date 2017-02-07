using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Configuration;
using AgateLib.Platform;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.Platform
{
	[TestClass]
	public class TimingTests
	{
		class FakeStopwatch : IClock
		{
			public TimeSpan CurrentTime { get; set; }
		}

		[TestMethod]
		public void AdvanceTest()
		{
			var watch = new FakeStopwatch();
			var clock = new MasterClock(watch);
			var tracker = new GameClock(clock);

			watch.CurrentTime = TimeSpan.FromSeconds(1);
			clock.Advance();

			watch.CurrentTime = TimeSpan.FromSeconds(2);
			clock.Advance();

			Assert.AreEqual(1, tracker.DeltaTime.TotalSeconds, 0.001);

			watch.CurrentTime = TimeSpan.FromSeconds(0.5);
			clock.Advance();

			Assert.AreEqual(0.5, tracker.DeltaTime.TotalSeconds, 0.001);
		}
	}
}
