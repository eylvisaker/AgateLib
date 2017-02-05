using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics;
using AgateLib.InputLib;
using AgateLib.Platform.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.Configuration
{
	[TestClass]
	public class InitializationTests
	{
		[TestMethod]
		public void FirstInputHandlerSetCorrectly()
		{
			using (new AgateUnitTestPlatform().Initialize())
			{
				Assert.IsNotNull(AgateApp.State.Input.FirstHandler);

				Assert.AreSame(AgateConsole.Instance,
					AgateApp.State.Input.FirstHandler);
			}
		}
	}
}
