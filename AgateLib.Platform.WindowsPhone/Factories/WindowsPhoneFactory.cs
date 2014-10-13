using AgateLib.ApplicationModels;
using AgateLib.DisplayLib;
using AgateLib.Drivers;
using AgateLib.Drivers.NullDrivers;
using AgateLib.IO;
using AgateLib.Platform.WindowsStore;
using AgateLib.Platform.WindowsStore.Factories;
using SharpDX.SimpleInitializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.WindowsPhone.Factories
{
	class WindowsPhoneFactory : IAgateFactory
	{
		DisplayFactory mDisplayFactory;

		public WindowsPhoneFactory(AssetLocations assets)
		{
			mDisplayFactory = new DisplayFactory();
			PlatformFactory = new WPPlatformFactory(assets);

			AudioFactory = new AudioFactoryDX();
			InputFactory = new NullInputFactory();
		}

		public IDisplayFactory DisplayFactory
		{
			get { return mDisplayFactory; }
		}
		public IAudioFactory AudioFactory { get; private set; }
		public IInputFactory InputFactory { get; private set; }
		public IPlatformFactory PlatformFactory { get; private set; }
		public FontSurface DefaultFont { get; set; }

		public IRenderTargetAdapter RenderTargetAdapter
		{
			get { return mDisplayFactory.RenderTargetAdapter; }
			set { mDisplayFactory.RenderTargetAdapter = value; }
		}
	}
}
