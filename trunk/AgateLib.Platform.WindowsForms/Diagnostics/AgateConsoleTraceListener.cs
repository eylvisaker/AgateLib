using AgateLib.Diagnostics.ConsoleSupport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AgateLib.Diagnostics
{
	class AgateConsoleTraceListener : TraceListener
	{
		AgateConsole mOwner;
		ConsoleMessage current;
		Stopwatch watch = new Stopwatch();

		public AgateConsoleTraceListener(AgateConsole owner)
		{
			mOwner = owner;
			Trace.Listeners.Add(this);

			watch.Start();
		}

		public override void Write(string message)
		{
			if (current == null)
			{
				current = new ConsoleMessage();
				mOwner.Messages.Add(current);
			}

			current.Time = watch.ElapsedMilliseconds;
			current.Text += message;
		}
		public override void WriteLine(string message)
		{
			if (current == null)
			{
				current = new ConsoleMessage();
				mOwner.Messages.Add(current);
			}

			current.Text += message;
			current.Time = watch.ElapsedMilliseconds;
			current = null;
		}

		public Stopwatch Watch { get { return watch; } }
	}

}
