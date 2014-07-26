using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AgateLib.Diagnostics
{
	public static class Log
	{
		public static void WriteLine(string message)
		{
			Debug.WriteLine(message);
		}
		public static void WriteLine(string format, params object[] args)
		{
			Debug.WriteLine(format, args);
		}
	}
}
