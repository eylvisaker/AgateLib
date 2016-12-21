using AgateLib.Drivers;
using AgateLib.Platform;
using AgateLib.Settings;

namespace AgateLib.Configuration.State
{
	class CoreState
	{
		internal class ErrorReportingState
		{
			public string ErrorFile { get; set; } = "errorlog.txt";
			public bool AutoStackTrace { get; set; } = false;
			public bool WroteHeader { get; set; } = false;
		}

		public ErrorReportingState ErrorReporting { get; private set; } = new ErrorReportingState();

		public bool AutoPause { get; set; }
		public bool IsActive { get; set; } = true;
		public bool Inititalized { get; set; }
		public PlatformInfo Platform { get; set; }
		public PersistantSettings Settings { get; set; }
		public IAgateFactory Factory { get; set; }

		public System.Diagnostics.Stopwatch MasterTime { get; private set; } = new System.Diagnostics.Stopwatch();
		/// <summary>
		/// 
		/// </summary>
		public CrossPlatformDebugLevel CrossPlatformDebugLevel { get; set; } = CrossPlatformDebugLevel.Comment;
		public IStopwatch Time { get; set; }

	}
}