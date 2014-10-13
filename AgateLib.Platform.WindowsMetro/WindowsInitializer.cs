using AgateLib.ApplicationModels;
using AgateLib.Platform.WindowsStore.Factories;
using AgateLib.Platform.WindowsStore;
using SharpDX.SimpleInitializer;
using System;
using AgateLib.Platform.WindowsMetro.Factories;
using AgateLib.IO;

namespace AgateLib.Platform.WindowsMetro
{
	static class WindowsInitializer
	{
		static WindowsFactory factory;

		internal static void Initialize(IRenderTargetAdapter renderTargetAdapter, AssetLocations assets)
		{
			if (factory == null)
			{
				factory = new WindowsFactory(renderTargetAdapter, assets);
				Core.Initialize(factory);
			}

			factory.RenderTargetAdapter = renderTargetAdapter;
			Core.InitAssetLocations(assets);
		}
	}
}
