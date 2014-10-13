using AgateLib.ApplicationModels;
using AgateLib.Platform.WindowsStore.Factories;
using AgateLib.Platform.WindowsStore;
using SharpDX.SimpleInitializer;
using System;
using AgateLib.Platform.WindowsPhone.Factories;
using AgateLib.IO;

namespace AgateLib.Platform.WindowsPhone
{
	static class WindowsPhoneInitializer
	{
		static WindowsPhoneFactory factory;

		internal static void Initialize(IRenderTargetAdapter renderTargetAdapter, AssetLocations assets)
		{
			if (factory == null)
			{
				factory = new WindowsPhoneFactory(assets);
				Core.Initialize(factory);
			}

			factory.RenderTargetAdapter = renderTargetAdapter;
			Core.InitAssetLocations(assets);
		}
	}
}
