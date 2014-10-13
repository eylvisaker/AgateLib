using AgateLib.Drivers;
using AgateLib.Platform.WindowsStore.PlatformImplementation;
using AgateLib.Platform.WindowsStore.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.ApplicationModels;
using AgateLib.IO;

namespace AgateLib.Platform.WindowsMetro.Factories
{
	class WinPlatformFactory : WindowsStorePlatformFactory
	{
		public WinPlatformFactory(AssetLocations assets)
			: base(false, assets)
		{ }
	}
}
