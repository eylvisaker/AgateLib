﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.DisplayLib
{
	public class AgateUninitializedUnitTest
	{
		public AgateUninitializedUnitTest()
		{
			Core.Dispose();
		}
	}
}
