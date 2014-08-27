using AgateLib.ApplicationModels;
using AgateLib.Platform.WindowsStoreCommon.Factories;
using AgateLib.Platform.WindowsStoreCommon;
using SharpDX.SimpleInitializer;
using System;
using AgateLib.Platform.WindowsPhone8.Factories;

namespace AgateLib.Platform.WindowsPhone8
{
	static class WindowsPhoneInitializer
	{
		static WindowsPhoneFactory factory;

		internal static void Initialize(SharpDXContext context, IRenderTargetAdapter renderTarget, AssetLocations assets)
		{
			factory = new WindowsPhoneFactory(context, renderTarget);
			Core.Initialize(factory, assets);
		}
	}
}
