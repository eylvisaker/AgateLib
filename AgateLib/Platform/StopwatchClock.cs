using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform
{
	public class StopwatchClock : IClock
	{
		private Stopwatch watch = Stopwatch.StartNew();

		public TimeSpan CurrentTime => watch.Elapsed;
	}
}
