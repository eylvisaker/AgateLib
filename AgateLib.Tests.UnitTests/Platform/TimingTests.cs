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
			var masterClock = new MasterClock(watch);
			var gameClock = new GameClock(masterClock);

			watch.CurrentTime = TimeSpan.FromSeconds(1);
			masterClock.Advance();

			watch.CurrentTime = TimeSpan.FromSeconds(2);
			masterClock.Advance();

			Assert.AreEqual(1, gameClock.DeltaTime.TotalSeconds, 0.001);

			watch.CurrentTime = TimeSpan.FromSeconds(0.5);
			masterClock.Advance();

			Assert.AreEqual(0.5, gameClock.DeltaTime.TotalSeconds, 0.001);
		}
	}
}
