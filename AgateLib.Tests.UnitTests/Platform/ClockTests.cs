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
	public class ClockTests
	{
		class FakeStopwatch : IClock
		{
			public TimeSpan CurrentTime { get; set; }
		}

		private FakeStopwatch watch = new FakeStopwatch();
		private MasterClock masterClock;
		private GameClock gameClock;

		[TestInitialize]
		public void Initialize()
		{
			masterClock = new MasterClock(watch)
			{
				MaxElapsed = ClockTimeSpan.FromSeconds(60)
			};

			gameClock = new GameClock(masterClock);
		}

		[TestMethod]
		public void MasterClockMaxElapsed()
		{
			masterClock.MaxElapsed = ClockTimeSpan.FromSeconds(0.1);

			watch.CurrentTime = TimeSpan.FromSeconds(1);
			masterClock.Advance();

			watch.CurrentTime = TimeSpan.FromSeconds(2);
			masterClock.Advance();

			Assert.AreEqual(0.1, masterClock.Elapsed.TotalSeconds);
		}

		[TestMethod]
		public void MasterClockTotalTimeTest()
		{
			watch.CurrentTime = TimeSpan.FromSeconds(1);
			masterClock.Advance();

			watch.CurrentTime = TimeSpan.FromSeconds(2);
			masterClock.Advance();

			Assert.AreEqual(1, masterClock.CurrentTime.TotalSeconds, 0.001);

			watch.CurrentTime = TimeSpan.FromSeconds(2.5);
			masterClock.Advance();

			Assert.AreEqual(1.5, masterClock.CurrentTime.TotalSeconds, 0.001);
		}

		[TestMethod]
		public void GameClockTotalTimeTest()
		{
			watch.CurrentTime = TimeSpan.FromSeconds(1);
			masterClock.Advance();

			watch.CurrentTime = TimeSpan.FromSeconds(2);
			masterClock.Advance();

			Assert.AreEqual(1, gameClock.CurrentTime.TotalSeconds, 0.001);

			watch.CurrentTime = TimeSpan.FromSeconds(2.5);
			masterClock.Advance();

			Assert.AreEqual(1.5, gameClock.CurrentTime.TotalSeconds, 0.001);
		}

		[TestMethod]
		public void AdvanceTest()
		{
			watch.CurrentTime = TimeSpan.FromSeconds(1);
			masterClock.Advance();

			watch.CurrentTime = TimeSpan.FromSeconds(2);
			masterClock.Advance();

			Assert.AreEqual(1, gameClock.Elapsed.TotalSeconds, 0.001);

			watch.CurrentTime = TimeSpan.FromSeconds(2.5);
			masterClock.Advance();

			Assert.AreEqual(0.5, gameClock.Elapsed.TotalSeconds, 0.001);
		}

		[TestMethod]
		public void SlowGameClockTest()
		{
			gameClock.ClockSpeed = 0.5;

			watch.CurrentTime = TimeSpan.FromSeconds(1);
			masterClock.Advance();

			watch.CurrentTime = TimeSpan.FromSeconds(2);
			masterClock.Advance();

			Assert.AreEqual(0.5, gameClock.Elapsed.TotalSeconds, 0.001);

			watch.CurrentTime = TimeSpan.FromSeconds(2.5);
			masterClock.Advance();

			Assert.AreEqual(0.25, gameClock.Elapsed.TotalSeconds, 0.001);
		}

		[TestMethod]
		public void SlowGameClockTotalTimeTest()
		{
			watch.CurrentTime = TimeSpan.FromSeconds(1);
			masterClock.Advance();

			watch.CurrentTime = TimeSpan.FromSeconds(2);
			masterClock.Advance();

			Assert.AreEqual(1, gameClock.CurrentTime.TotalSeconds, 0.001);

			gameClock.ClockSpeed = 0.5;

			watch.CurrentTime = TimeSpan.FromSeconds(3);
			masterClock.Advance();

			Assert.AreEqual(1.5, gameClock.CurrentTime.TotalSeconds, 0.001);
		}


		[TestMethod]
		public void GameClockSpeedChangedTwiceTotalTimeTest()
		{
			watch.CurrentTime = TimeSpan.FromSeconds(1);
			masterClock.Advance();

			watch.CurrentTime = TimeSpan.FromSeconds(2);
			masterClock.Advance();

			Assert.AreEqual(1, gameClock.CurrentTime.TotalSeconds, 0.001);

			gameClock.ClockSpeed = 0.5;

			watch.CurrentTime = TimeSpan.FromSeconds(3);
			masterClock.Advance();

			Assert.AreEqual(1.5, gameClock.CurrentTime.TotalSeconds, 0.001);

			gameClock.ClockSpeed = 2;

			watch.CurrentTime = TimeSpan.FromSeconds(4);
			masterClock.Advance();

			Assert.AreEqual(3.5, gameClock.CurrentTime.TotalSeconds, 0.001);
		}
	}
}
