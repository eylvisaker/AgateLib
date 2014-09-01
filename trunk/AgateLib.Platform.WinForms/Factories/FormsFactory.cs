using AgateLib.ApplicationModels;
using AgateLib.DisplayLib;
using AgateLib.Drivers;
using AgateLib.Platform.WinForms.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WinForms.Factories
{
	class FormsFactory : IAgateFactory
	{
		FontSurface mDefaultFont;
		PlatformFactory mPlatformFactory;

		public FormsFactory()
		{
			DisplayFactory = new DisplayFactory();
			mPlatformFactory = new PlatformFactory();

			var sdl = new SdlFactory();

			AudioFactory = sdl;
			InputFactory = sdl;
		}

		public void SetAssetLocations(AssetLocations assetLocations)
		{
			mPlatformFactory.SetAssetLocations(assetLocations);
		}

		public IDisplayFactory DisplayFactory { get; private set; }
		public IAudioFactory AudioFactory { get; private set; }
		public IInputFactory InputFactory { get; private set; }
		public IPlatformFactory PlatformFactory { get { return mPlatformFactory; } }

	}
}
