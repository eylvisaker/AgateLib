﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgateLib.Drivers;
using AgateLib.InputLib.ImplementationBase;

namespace AgateLib.Platform.Test.Input
{
	public class FakeInputFactory : IInputFactory
	{
		public InputImpl CreateJoystickInputImpl()
		{
			return new FakeInputImpl();
		}
	}
}