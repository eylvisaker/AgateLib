using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Diagnostics.ConsoleSupport
{

	public delegate string DescribeCommandHandler(string command);

	public enum ConsoleMessageType
	{
		Text,
		UserInput,
	}
}
