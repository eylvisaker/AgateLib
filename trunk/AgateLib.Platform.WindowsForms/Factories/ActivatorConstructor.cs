using AgateLib.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.WindowsForms.Factories
{
	class ActivatorConstructor : IObjectConstructor
	{
		public object CreateInstance(Type t)
		{
			return Activator.CreateInstance(t, true);
		}
	}
}
