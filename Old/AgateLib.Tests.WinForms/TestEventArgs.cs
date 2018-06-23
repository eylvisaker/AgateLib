using System;

namespace AgateLib.Tests
{
	public class TestEventArgs : EventArgs
	{
		public TestInfo Info { get; internal set; }
	}
}