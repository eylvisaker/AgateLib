using System;
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
			// This dummy variable ensures the Castle.Core.dll file is copied by mstest.
			// Why the hell hasn't MS fixed this issue?
			Castle.Core.IServiceEnabledComponent dummyVariable;

			AgateApp.Dispose();
		}
	}
}
