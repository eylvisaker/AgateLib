using AgateLib.Drivers;
using AgateLib.Drivers.NullDrivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Testing.Fakes
{
	public class FakeAgateFactory : IAgateFactory
	{
		public IDisplayFactory DisplayFactory
		{
			get { return new FakeDisplayFactory(); }
		}

		public IAudioFactory AudioFactory
		{
			get { return new NullSoundFactory(); }
		}

		public IInputFactory InputFactory
		{
			get { return new NullInputFactory(); }
		}

		public IPlatformFactory PlatformFactory
		{
			get { return new FakePlatformFactory(); }
		}

		public DisplayLib.FontSurface DefaultFont
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
	}
}
