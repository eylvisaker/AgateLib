using AgateLib.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsForms.Factories
{
	class FormsFactory : IAgateFactory
	{
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
		public IPlatformFactory PlatformFactory { get; private set;}
	}
}
