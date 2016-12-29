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
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics.ConsoleSupport;

namespace AgateLib.Diagnostics
{
	/// <summary>
	/// A library is an object that represents several entrypoints - methods which are called by 
	/// name by the user in the console window. 
	/// </summary>
	public class LibraryVocabulary : ICommandLibrary
	{
		class CommandInfo
		{
			public Delegate Delegate { get; set; }
			public ConsoleCommandAttribute CommandAttribute { get; set; }
		}

		Dictionary<string, CommandInfo> commands = new Dictionary<string, CommandInfo>();

		private ICommandVocabulary vocabulary;

		public LibraryVocabulary(ICommandVocabulary commandLibrary)
		{
			this.vocabulary = commandLibrary;

			BuildCommands();
		}

		public void Help()
		{
			StringBuilder builder = new StringBuilder();

			int i = 0;
			foreach (var key in commands.Keys)
			{
				var info = commands[key];
				if (info.CommandAttribute.Hidden)
					continue;

				builder.Append("    ");
				i++;

				if (i % 3 == 0)
				{
					builder.AppendLine(key);
				}
				else
				{
					builder.Append((key + new string(' ', 30)).Substring(0, Math.Max(30, key.Length)));
				}
			}

			if (builder.Length > 0)
			{
				AgateConsole.WriteLine(builder.ToString());
			}
		}

		public void Help(string command)
		{
			if (commands.ContainsKey(command) == false)
				return;

			var methodInfo = commands[command].Delegate.GetMethodInfo();

			var commandAttribute = methodInfo?.GetCustomAttribute<ConsoleCommandAttribute>();

			AgateConsole.WriteLine(commandAttribute?.Description ?? "No description found.");
		}

		public bool Execute(string command)
		{
			string[] tokens = ConsoleTokenizer.Tokenize(command);

			if (commands.ContainsKey(tokens[0]))
			{
				ExecuteDelegate(commands[tokens[0]].Delegate, tokens);
				return true;
			}

			return false;
		}

		private void WriteLine(string text, params object[] args)
		{
			AgateConsole.WriteLineFormat(text, args);
		}

		private void BuildCommands()
		{
			commands.Clear();

			var methods = vocabulary.GetType().GetTypeInfo().DeclaredMethods
				.Where(x => x.GetCustomAttribute<ConsoleCommandAttribute>() != null);

			foreach (var method in methods)
			{
				var attrib = method.GetCustomAttribute<ConsoleCommandAttribute>();

				var name = method.Name.ToLowerInvariant();

				if (!string.IsNullOrWhiteSpace(attrib.Name))
					name = attrib.Name;

				commands.Add(name, new CommandInfo
				{
					CommandAttribute = attrib,
					Delegate = method.CreateDelegate(Expression.GetDelegateType(method.GetParameters()
						.Select(x => x.ParameterType)
						.Concat(new[] { method.ReturnType })
						.ToArray()), vocabulary)
				});
			}
		}

		private void ExecuteDelegate(Delegate p, string[] tokens)
		{
			var method = p.GetMethodInfo();
			var parameters = method.GetParameters();
			object[] args = new object[parameters.Length];
			bool notEnoughArgs = false;
			bool badArgs = false;

			for (int i = 0, j = 1; i < parameters.Length || j < tokens.Length; i++, j++)
			{
				if (i < parameters.Length && 
					parameters[i].GetCustomAttribute<JoinArgsAttribute>() != null && 
					parameters[i].ParameterType == typeof(string))
				{
					args[i] = string.Join(" ", tokens.Skip(1));
				}
				else if (i < args.Length && j < tokens.Length)
				{
					try
					{
						args[i] = Convert.ChangeType(tokens[j], parameters[i].ParameterType, System.Globalization.CultureInfo.InvariantCulture);
					}
					catch
					{
						WriteLine("Argument #" + j.ToString() + " invalid: \"" +
							tokens[j] + "\" not convertable to " + parameters[i].ParameterType.Name);
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
					WriteLine("[Ignoring extra argument: " + tokens[j] + "]");
				}
			}

			if (badArgs || notEnoughArgs)
			{
				AgateConsole.Execute("help " + string.Join(" ", tokens));
				return;
			}

			object result = method.Invoke(p.Target, args);

			if (method.ReturnType != typeof(void) && result != null)
			{
				WriteLine(result.ToString());
			}
		}
	}
}
