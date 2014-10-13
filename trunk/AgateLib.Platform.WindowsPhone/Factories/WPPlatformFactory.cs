using AgateLib.Drivers;
using AgateLib.Platform.WindowsStore.PlatformImplementation;
using AgateLib.Platform.WindowsStore.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.ApplicationModels;
using AgateLib.IO;

namespace AgateLib.Platform.WindowsPhone.Factories
{
	class WPPlatformFactory : WindowsStorePlatformFactory
	{
		public WPPlatformFactory(AssetLocations assets)
			: base(true, assets)
		{ }
	}
}
