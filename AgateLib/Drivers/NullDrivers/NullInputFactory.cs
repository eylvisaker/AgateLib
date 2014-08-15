using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Drivers.NullDrivers
{
	public class NullInputFactory : IInputFactory
	{
		public InputLib.ImplementationBase.InputImpl CreateJoystickInputImpl()
		{
			return new NullInputImpl();
		}
	}
}
