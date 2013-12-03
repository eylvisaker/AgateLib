using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgateLib.InputLib;

namespace AgateLib.UnitTests.Core
{
	[TestClass]
	public class ConsoleTests
	{
		[TestMethod]
		public void HelpCommand()
		{
			AgateConsole c = new AgateConsole();
			int count = 0;

			c.CommandProcessor.DescribeCommand += (cmd) => { count++; return string.Empty; };

			c.ProcessKeys("help");
			c.ProcessKeyDown(new InputEventArgs(KeyCode.Enter, new KeyModifiers(false, false, false)));

			Assert.AreEqual(0, count);

			c.ProcessKeys("help help");
			c.ProcessKeyDown(new InputEventArgs(KeyCode.Enter, new KeyModifiers(false, false, false)));
			Assert.AreEqual(1, count);

			c.ProcessKeys("help unknown_command");
			c.ProcessKeyDown(new InputEventArgs(KeyCode.Enter, new KeyModifiers(false, false, false)));
			Assert.AreEqual(1, count);
		}
	}
}
