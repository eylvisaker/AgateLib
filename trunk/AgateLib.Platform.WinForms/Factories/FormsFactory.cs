using AgateLib.AgateSDL;
using AgateLib.ApplicationModels;
using AgateLib.DisplayLib;
using AgateLib.Drivers;
using AgateLib.IO;
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

			var sdl = new AgateSdlFactory();

			AudioFactory = sdl;
			InputFactory = sdl;
		}

		public IDisplayFactory DisplayFactory { get; private set; }
		public IAudioFactory AudioFactory { get; private set; }
		public IInputFactory InputFactory { get; private set; }
		public IPlatformFactory PlatformFactory { get { return mPlatformFactory; } }

	}
}
