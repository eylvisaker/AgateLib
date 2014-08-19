﻿using AgateLib.DisplayLib.ImplementationBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Drivers
{
	public interface IDisplayFactory
	{
		DisplayImpl CreateDisplayImpl();
	}
}