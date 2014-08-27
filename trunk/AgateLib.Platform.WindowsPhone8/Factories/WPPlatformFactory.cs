using AgateLib.Drivers;
using AgateLib.Platform.WindowsStoreCommon.PlatformImplementation;
using AgateLib.Platform.WindowsStoreCommon.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.WindowsPhone8.Factories
{
	class WPPlatformFactory : WindowsStorePlatformFactory
	{
		public WPPlatformFactory()
			: base(true)
		{ }
	}
}
