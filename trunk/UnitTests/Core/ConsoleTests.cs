using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgateLib.InputLib;

namespace AgateLib.UnitTests.Core
{
	[TestClass]
	public class ConsoleTests
	{
		AgateConsole console;
		int describeCallCount = 0;

		[TestInitialize]
		public void Init()
		{
			console = new AgateConsole();

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
			console.ProcessKeyDown(new InputEventArgs(KeyCode.Enter, new KeyModifiers()));

			Assert.AreEqual(0, describeCallCount);

			console.ProcessKeys("help help");
			console.ProcessKeyDown(new InputEventArgs(KeyCode.Enter, new KeyModifiers()));
			Assert.AreEqual(1, describeCallCount);

			console.ProcessKeys("help unknown_command");
			console.ProcessKeyDown(new InputEventArgs(KeyCode.Enter, new KeyModifiers()));
			Assert.AreEqual(1, describeCallCount);
		}

		[TestMethod]
		public void KeystrokeEditing()
		{
			console.ProcessKeys("help");
			console.ProcessKeyDown(new InputEventArgs(KeyCode.BackSpace, new KeyModifiers()));

			Assert.AreEqual("hel", console.CurrentLine);

			console.ProcessKeys("x");
			Assert.AreEqual("helx", console.CurrentLine);
		}

		[TestMethod]
		public void HistoryBrowsing()
		{
			console.ProcessKeys("help\nhelp help\nhelp unknown_command\n");
			console.ProcessKeyDown(new InputEventArgs(KeyCode.Up, new KeyModifiers()));
			console.ProcessKeys("x");
			Assert.AreEqual("help unknown_commandx", console.CurrentLine);

			console.ProcessKeys("\n");
			Assert.AreEqual("", console.CurrentLine);

			console.ProcessKeyDown(new InputEventArgs(KeyCode.Up, new KeyModifiers()));
			console.ProcessKeyDown(new InputEventArgs(KeyCode.Up, new KeyModifiers()));
			console.ProcessKeyDown(new InputEventArgs(KeyCode.Up, new KeyModifiers()));
			console.ProcessKeyDown(new InputEventArgs(KeyCode.BackSpace, new KeyModifiers()));
			Assert.AreEqual("help hel", console.CurrentLine);

		}
	}
}