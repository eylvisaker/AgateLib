using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Drivers
{
	public interface IAgateFactory
	{
		IDisplayFactory DisplayFactory { get; }
		IAudioFactory AudioFactory { get; }
		IInputFactory InputFactory { get; }
		IPlatformFactory PlatformFactory { get; }

		DisplayLib.FontSurface DefaultFont { get; set; }
	}
}
