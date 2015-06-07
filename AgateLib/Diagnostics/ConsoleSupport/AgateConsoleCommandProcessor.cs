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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Diagnostics.ConsoleSupport
{
	public class AgateConsoleCommandProcessor : ICommandProcessor
	{
		ConsoleDictionary mCommands = new ConsoleDictionary();

		public AgateConsoleCommandProcessor()
		{
			mCommands.Add("help", new Action<string>(HelpCommand));
		}

		public ConsoleDictionary Commands { get { return mCommands; } }

		public void ExecuteCommand(string[] tokens)
		{
			if (mCommands.ContainsKey(tokens[0]) == false)
			{
				WriteLine("Invalid command: " + tokens[0]);
			}
			else
			{
				ExecuteDelegate(mCommands[tokens[0]], tokens);
			}
		}

		private void ExecuteDelegate(Delegate p, string[] tokens)
		{
			var method = p.GetMethodInfo();
			var parameters = method.GetParameters();
			object[] args = new object[parameters.Length];
			bool notEnoughArgs = false;
			bool badArgs = false;

			for (int i = 0; i < parameters.Length || i < tokens.Length - 1; i++)
			{
				if (i < args.Length && i < tokens.Length - 1)
				{
					try
					{
						args[i] = Convert.ChangeType(tokens[i + 1], parameters[i].ParameterType, System.Globalization.CultureInfo.InvariantCulture);
					}
					catch
					{
						WriteLine("Argument #" + (i + 1).ToString() + " invalid: \"" +
							tokens[i + 1] + "\" not convertable to " + parameters[i].ParameterType.Name);
						badArgs = true;
					}
				}
				else if (i < args.Length)
				{
					if (parameters[i].IsOptional)
					{
						args[i] = Type.Missing;
					}
					else
					{
						if (notEnoughArgs == false)
						{
							WriteLine("Insufficient arguments for command: " + tokens[0]);
						}
						notEnoughArgs = true;

						WriteLine("    missing " + parameters[i].ParameterType.Name + " argument: " + parameters[i].Name);
					}
				}
				else
				{
					WriteLine("[Ignoring extra argument: " + tokens[i + 1] + "]");
				}
			}

			if (badArgs || notEnoughArgs)
				return;

			object result = method.Invoke(p.Target, args);

			if (method.ReturnType != typeof(void) && result != null)
			{
				WriteLine(result.ToString());
			}
		}


		private void HelpCommand(string command = "")
		{
			command = command.ToLowerInvariant().Trim();

			if (string.IsNullOrEmpty(command) || mCommands.ContainsKey(command) == false)
			{
				WriteLine("Available Commands:");

				foreach (var cmd in mCommands.Keys)
				{
					if (cmd == "help")
						continue;

					WriteLine("    " + cmd);
				}

				WriteLine("Type \"help <command>\" for help on a specific command.");
			}
			else
			{
				Delegate d = mCommands[command];
				var method = d.GetMethodInfo();
				Write("Usage: ");
				Write(command + " ");

				var parameters = method.GetParameters();
				for (int i = 0; i < parameters.Length; i++)
				{
					if (parameters[i].IsOptional)
						Write("[");

					Write(parameters[i].Name);

					if (parameters[i].IsOptional)
						Write("]");
				}

				WriteLine("");

				string description = "";

				if (DescribeCommand != null)
				{
					description = DescribeCommand(command);
				}
				if (string.IsNullOrEmpty(description))
				{
					var descripAttrib = (DescriptionAttribute)method.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();

					if (descripAttrib != null)
						description = descripAttrib.Description;
				}

				if (string.IsNullOrEmpty(description) == false)
				{
					WriteLine(description);
				}

			}
		}

		void WriteLine(string text)
		{
			AgateConsole.WriteLine(text);
		}
		void Write(string text)
		{
			AgateConsole.Write(text);
		}

		public event DescribeCommandHandler DescribeCommand;
	}

}
