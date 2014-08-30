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

		public FormsFactory()
		{
			DisplayFactory = new DisplayFactory();
			PlatformFactory = new PlatformFactory();

			var sdl = new SdlFactory();

			AudioFactory = sdl;
			InputFactory = sdl;
		}

		public IDisplayFactory DisplayFactory { get; private set; }
		public IAudioFactory AudioFactory { get; private set; }
		public IInputFactory InputFactory { get; private set; }
		public IPlatformFactory PlatformFactory { get; private set; }


		public DisplayLib.FontSurface DefaultFont
		{
			get
			{
				if (mDefaultFont == null)
					mDefaultFont = BuiltinResources.AgateSans10;

				return mDefaultFont;
			}
			set
			{
				throw new NotImplementedException();
			}
		}
	}
}
