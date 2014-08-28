using AgateLib.ApplicationModels;
using AgateLib.Platform.WindowsStoreCommon.Factories;
using AgateLib.Platform.WindowsStoreCommon;
using SharpDX.SimpleInitializer;
using System;
using AgateLib.Platform.Windows.Factories;

namespace AgateLib.Platform.Windows
{
	static class WindowsInitializer
	{
		static WindowsFactory factory;

		internal static void Initialize(SharpDXContext context, IRenderTargetAdapter renderTarget, AssetLocations assets)
		{
			factory = new WindowsFactory(context, renderTarget);
			Core.Initialize(factory, assets);
		}
	}
}
