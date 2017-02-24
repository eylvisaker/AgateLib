using System;
using System.Collections.Generic;
using AgateLib.Diagnostics;
using AgateLib.Diagnostics.ConsoleSupport;
using AgateLib.InputLib;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.Diagnostics
{
	[TestClass]
	public class ConsoleKeyInputTest : AgateUnitTest
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

			public IEnumerable<string> AutoCompleteEntries(string inputString)
			{
				yield break;
			}
		}

		HelpProbe probe = new HelpProbe();
		AgateConsoleCore console;

		[TestInitialize]
		public void Init()
		{
			console = new AgateConsoleCore();

			AgateConsole.Initialize(console);
			AgateConsole.CommandLibraries.Add(probe);
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

		[TestMethod]
		public void CursorEditingBackspace()
		{
			console.ProcessKeys("123456789");
			console.ProcessKeyDown(KeyCode.Left, "");
			console.ProcessKeyDown(KeyCode.Left, "");
			console.ProcessKeyDown(KeyCode.Left, "");
			console.ProcessKeyDown(KeyCode.BackSpace, "");

			Assert.AreEqual("12345789", console.InputText);
		}

		[TestMethod]
		public void CursorEditingBackspaceAtEnd()
		{
			console.ProcessKeys("123456789");
			console.ProcessKeyDown(KeyCode.BackSpace, "");

			Assert.AreEqual("12345678", console.InputText);
		}

		[TestMethod]
		public void CursorEditingBackspaceAtBegin()
		{
			console.ProcessKeys("12");
			console.ProcessKeyDown(KeyCode.Left, "");
			console.ProcessKeyDown(KeyCode.Left, "");
			console.ProcessKeyDown(KeyCode.Left, "");
			console.ProcessKeyDown(KeyCode.BackSpace, "");

			Assert.AreEqual("12", console.InputText);
		}

		[TestMethod]
		public void CursorEditingDelete()
		{
			console.ProcessKeys("123456789");
			console.ProcessKeyDown(KeyCode.Left, "");
			console.ProcessKeyDown(KeyCode.Left, "");
			console.ProcessKeyDown(KeyCode.Left, "");
			console.ProcessKeyDown(KeyCode.Delete, "");

			Assert.AreEqual("12345689", console.InputText);
		}

		[TestMethod]
		public void CursorEditingDeleteAtEnd()
		{
			console.ProcessKeys("123456789");
			console.ProcessKeyDown(KeyCode.Delete, "");
			Assert.AreEqual("123456789", console.InputText);

			console.ProcessKeyDown(KeyCode.Left, "");
			console.ProcessKeyDown(KeyCode.Delete, "");
			
			Assert.AreEqual("12345678", console.InputText);
		}

		[TestMethod]
		public void CursorEditingDeleteAtBegin()
		{
			console.ProcessKeys("123");
			console.ProcessKeyDown(KeyCode.Left, "");
			console.ProcessKeyDown(KeyCode.Left, "");
			console.ProcessKeyDown(KeyCode.Left, "");
			console.ProcessKeyDown(KeyCode.Left, "");
			console.ProcessKeyDown(KeyCode.Delete, "");

			Assert.AreEqual("23", console.InputText);
		}

		[TestMethod]
		public void ConsoleAutoCompletion()
		{
			console.ProcessKeys("He");
			console.ProcessKeyDown(KeyCode.Tab, "\t");

			StringAssert.StartsWith(console.InputText, "help");
		}
	}
}