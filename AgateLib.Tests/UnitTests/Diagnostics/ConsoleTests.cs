using System;
using AgateLib.Diagnostics;
using AgateLib.InputLib;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.Diagnostics
{
	[TestClass]
	public class ConsoleTests : AgateUnitTest
	{
		class HelpProbe : ICommandLibrary
		{
			public int HelpCount { get; private set; }
			public int HelpCommandCount { get; private set; }
			public string LastHelpCommand { get; private set; }

			public bool Execute(string command)
			{
				throw new NotImplementedException();
			}

			public void Help()
			{
				HelpCount++;
			}

			public void Help(string command)
			{
				HelpCommandCount++;

				LastHelpCommand = command;
			}
		}

		HelpProbe probe = new HelpProbe();
		AgateConsoleImpl console;

		[TestInitialize]
		public void Init()
		{
			console = new AgateConsoleImpl();

			AgateConsole.Initialize(console);
			AgateConsole.CommandProcessors.Add(probe);
		}

		[TestMethod]
		public void ConsoleHelp()
		{
			console.ProcessKeys("help");
			console.ProcessKeyDown(KeyCode.Enter, "\n");

			Assert.AreEqual(1, probe.HelpCount);
			Assert.AreEqual(0, probe.HelpCommandCount);
		}

		[TestMethod]
		public void ConsoleHelpCommand()
		{ 
			console.ProcessKeys("help help");
			console.ProcessKeyDown(KeyCode.Enter, "\n");
			Assert.AreEqual(1, probe.HelpCommandCount);
			Assert.AreEqual("help", probe.LastHelpCommand);

			console.ProcessKeys("help unknown_command");
			console.ProcessKeyDown(KeyCode.Enter, "\n");
			Assert.AreEqual(2, probe.HelpCommandCount);
			Assert.AreEqual("unknown_command", probe.LastHelpCommand);
		}

		[TestMethod]
		public void KeystrokeEditing()
		{
			console.ProcessKeys("help");
			console.ProcessKeyDown(KeyCode.BackSpace, "");

			Assert.AreEqual("hel", console.InputText);

			console.ProcessKeys("x");
			Assert.AreEqual("helx", console.InputText);
		}

		[TestMethod]
		public void HistoryBrowsing()
		{
			console.ProcessKeys("help\nhelp help\nhelp unknown_command\n");
			console.ProcessKeyDown(KeyCode.Up, "");
			console.ProcessKeys("x");
			Assert.AreEqual("help unknown_commandx", console.InputText);

			console.ProcessKeys("\n");
			Assert.AreEqual("", console.InputText);

			console.ProcessKeyDown(KeyCode.Up, "");
			console.ProcessKeyDown(KeyCode.Up, "");
			console.ProcessKeyDown(KeyCode.Up, "");
			console.ProcessKeyDown(KeyCode.BackSpace, "");
			Assert.AreEqual("help hel", console.InputText);

		}
	}
}