//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Diagnostics.ConsoleSupport
{
	class AgateEmergencyVocabulary : IVocabulary
	{
		const string helpCommand = "help";
		private AgateConsoleCore agateConsoleCore;

		public AgateEmergencyVocabulary(AgateConsoleCore agateConsoleCore)
		{
			this.agateConsoleCore = agateConsoleCore;
		}

		public string Namespace => "";

		[ConsoleCommand("Provides help for commands. You can type 'help' or 'help <command>' to get more information.", Hidden = true)]
		public void Help([JoinArgs] string command = null)
		{
			var commandLibraries = agateConsoleCore.CommandLibrarySet;

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
			WriteLine("Use up / down arrow keys to navigate input history.");
			WriteLine("Use shift+up / shift+down to view output history.");
		}

		[ConsoleCommand("Quits the application. No option to save is given.", Hidden = true)]
		public void Quit()
		{
			AgateApp.IsAlive = false;
		}

		[ConsoleCommand("Enable or disable debug info with 'debug on' or 'debug off'", Hidden = true)]
		public void Debug(string mode = "")
		{
			if (mode == "off")
			{
				AgateConsole.WriteLine("Disabling debug information.");
				AgateApp.State.Debug = false;
				return;
			}
			if (mode == "on")
			{
				AgateConsole.WriteLine("Enabling debug information. Type 'debug off' to turn it off.");
				AgateApp.State.Debug = true;
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
