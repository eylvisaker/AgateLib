using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.ApplicationModels
{
	public class ExitGameException : Exception
	{
		public ExitGameException() { }
		public ExitGameException(string message) : base(message) { }
		public ExitGameException(string message, Exception inner) : base(message, inner) { }
	}
}
