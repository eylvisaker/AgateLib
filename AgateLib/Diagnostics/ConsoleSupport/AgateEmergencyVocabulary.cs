using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.ApplicationModels;

namespace AgateLib.Diagnostics.ConsoleSupport
{
	class AgateEmergencyVocabulary : ICommandVocabulary
	{
		const string helpCommand = "help";
		private AgateConsoleImpl agateConsoleImpl;

		public AgateEmergencyVocabulary(AgateConsoleImpl agateConsoleImpl)
		{
			this.agateConsoleImpl = agateConsoleImpl;
		}

		[ConsoleCommand("Provides help for commands. You can type 'help' or 'help <command>' to get more information.", Hidden = true)]
		public void Help([JoinArgs] string command = null)
		{
			var commandLibraries = agateConsoleImpl.CommandLibrarySet;

			if (commandLibraries.Any())
			{
				if (string.IsNullOrEmpty(command))
				{
					WriteLine("Available Commands:");

					foreach (var commandProcessor in commandLibraries)
					{
						commandProcessor.Help();
					}
				}
				else
				{
					foreach (var commandProcessor in commandLibraries)
					{
						commandProcessor.Help(command);
					}

					return;
				}
			}
			else
			{
				WriteLine("No command processors installed.");
				WriteLine("Available Commands:");
			}

			WriteLine("    debug [on|off]");
			WriteLine("    quit");
		}

		[ConsoleCommand("Quits the application. No option to save is given.", Hidden = true)]
		public void Quit()
		{
			throw new ExitGameException();
		}

		[ConsoleCommand("Enable or disable debug info with 'debug on' or 'debug off'", Hidden = true)]
		public void Debug(string mode = "")
		{
			if (mode == "off")
			{
				AgateConsole.WriteLine("Disabling debug information.");
				Core.State.Debug = false;
				return;
			}
			if (mode == "on")
			{
				AgateConsole.WriteLine("Enabling debug information. Type 'debug off' to turn it off.");
				Core.State.Debug = true;
				return;
			}

			AgateConsole.WriteLine("Type 'debug on' to enable debug information.");
			AgateConsole.WriteLine("Type 'debug off' to disable debug information.");
		}

		private void WriteLine(string message)
		{
			AgateConsole.WriteLine(message);
		}

	}
}
