using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
			var parameters = p.Method.GetParameters();
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

			object retval = p.Method.Invoke(p.Target, args);

			if (p.Method.ReturnType != typeof(void) && retval != null)
			{
				WriteLine(retval.ToString());
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

				Write("Usage: ");
				Write(command + " ");

				var parameters = d.Method.GetParameters();
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
					var descripAttrib = (DescriptionAttribute)d.Method.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();

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
