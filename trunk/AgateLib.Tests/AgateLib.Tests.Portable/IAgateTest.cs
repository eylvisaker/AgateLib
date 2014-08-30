using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Testing
{
	public interface IAgateTest
	{
		string Name { get; }
		string Category { get; }
	}
}
