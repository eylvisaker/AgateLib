using System;

namespace AgateLib.Platform
{
	public interface IStopwatch
	{
		void Dispose();
		void ForceResume();
		bool IsPaused { get; }
		void Pause();
		void Reset();
		void Resume();
		double TotalMilliseconds { get; }
		double TotalSeconds { get; }
	}
}
