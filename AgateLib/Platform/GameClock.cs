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
		public TimeSpan Elapsed => TimeSpan.FromSeconds(
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
		public TimeSpan CurrentTime
		{
			get
			{
				double delta = parent.CurrentTime.TotalSeconds - lastClockSpeedChangeTimeSeconds;

				return TimeSpan.FromSeconds(ClockSpeed * delta + timeOriginSeconds);
			}
		}
	}
}
