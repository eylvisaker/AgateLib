﻿using AgateLib.DisplayLib;
using AgateLib.Drivers;
using AgateLib.Drivers.NullDrivers;
using AgateLib.Platform.WindowsStoreCommon;
using AgateLib.Platform.WindowsStoreCommon.Factories;
using SharpDX.SimpleInitializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.Windows.Factories
{
	class WindowsFactory : IAgateFactory
	{
		DisplayFactory mDisplayFactory;

		public WindowsFactory(SharpDXContext context, IRenderTargetAdapter renderTarget)
		{
			mDisplayFactory = new DisplayFactory(context, renderTarget);
			PlatformFactory = new WinPlatformFactory();

			AudioFactory = new NullSoundFactory();
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
	}
}
