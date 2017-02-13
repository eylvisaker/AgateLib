namespace AgateLib.Platform
{
	public struct ClockTimeSpan
	{
		public static ClockTimeSpan FromSeconds(double timePassedSeconds)
		{
			return new ClockTimeSpan(timePassedSeconds);
		}

		public static ClockTimeSpan FromMilliseconds(double timePassedMs)
		{
			return new ClockTimeSpan(timePassedMs * 0.001);
		}
		public static ClockTimeSpan Zero => new ClockTimeSpan();

		private double timeSpanSeconds;

		private ClockTimeSpan(double timeSpanSeconds)
		{
			this.timeSpanSeconds = timeSpanSeconds;
		}

		public double TotalMilliseconds => timeSpanSeconds * 1000;

		public double TotalSeconds => timeSpanSeconds;
	}
}