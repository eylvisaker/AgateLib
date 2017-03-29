﻿//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
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
