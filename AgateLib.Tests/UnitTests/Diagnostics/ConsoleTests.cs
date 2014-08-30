using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgateLib.InputLib;
using AgateLib.Diagnostics;
using AgateLib.InputLib.Legacy;

namespace AgateLib.Diagnostics
{
	[TestClass]
	public class ConsoleTests
	{
		AgateConsole console;
		int describeCallCount = 0;

		[TestInitialize]
		public void Init()
		{
			console = new AgateConsoleImpl();

			console.CommandProcessor.DescribeCommand += (cmd) => { describeCallCount++; return string.Empty; };
		}
		[TestCleanup]
		public void Cleanup()
		{
			console.Dispose();
		}

		[TestMethod]
		public void HelpCommand()
		{
			console.ProcessKeys("help");
			console.ProcessKeyDown(KeyCode.Enter, "\n");

			Assert.AreEqual(0, describeCallCount);

			console.ProcessKeys("help help");
			console.ProcessKeyDown(KeyCode.Enter, "\n");
			Assert.AreEqual(1, describeCallCount);

			console.ProcessKeys("help unknown_command");
			console.ProcessKeyDown(KeyCode.Enter, "\n");
			Assert.AreEqual(1, describeCallCount);
		}

		[TestMethod]
		public void KeystrokeEditing()
		{
			console.ProcessKeys("help");
			console.ProcessKeyDown(KeyCode.BackSpace, "");

			Assert.AreEqual("hel", console.CurrentLine);

			console.ProcessKeys("x");
			Assert.AreEqual("helx", console.CurrentLine);
		}

		[TestMethod]
		public void HistoryBrowsing()
		{
			console.ProcessKeys("help\nhelp help\nhelp unknown_command\n");
			console.ProcessKeyDown(KeyCode.Up, "");
			console.ProcessKeys("x");
			Assert.AreEqual("help unknown_commandx", console.CurrentLine);

			console.ProcessKeys("\n");
			Assert.AreEqual("", console.CurrentLine);

			console.ProcessKeyDown(KeyCode.Up, "");
			console.ProcessKeyDown(KeyCode.Up, "");
			console.ProcessKeyDown(KeyCode.Up, "");
			console.ProcessKeyDown(KeyCode.BackSpace, "");
			Assert.AreEqual("help hel", console.CurrentLine);

		}
	}
}