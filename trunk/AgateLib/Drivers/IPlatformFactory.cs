using AgateLib.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Drivers
{
	public interface IPlatformFactory
	{
		PlatformInfo CreatePlatformInfo();

		IStopWatch CreateStopwatch();
	}
}
