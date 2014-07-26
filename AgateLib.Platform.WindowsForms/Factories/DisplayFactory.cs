using AgateLib.Drivers;
using AgateLib.Platform.WindowsForms.DisplayImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsForms.Factories
{
	class DisplayFactory : IDisplayFactory
	{
		public DisplayLib.ImplementationBase.DisplayImpl CreateDisplayImpl()
		{
			return new DesktopGLDisplay();
		}
	}
}
