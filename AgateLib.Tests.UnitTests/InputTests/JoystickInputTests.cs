using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Platform.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.InputTests
{
	[TestClass]
	public class JoystickInputTests
	{
		[TestMethod]
		public void CountJoysticks()
		{
			using (new AgateUnitTestPlatform()
					.JoystickCount(4)
					.Initialize())
			{
				Assert.AreEqual(4, InputLib.Input.Joysticks.Count);
			}
		}
	}
}
