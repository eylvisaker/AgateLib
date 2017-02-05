namespace AgateLib.Configuration
{
	public class AgateAppClock : IClock
	{
		public double CurrentTime => AgateApp.GetTime();
	}
}