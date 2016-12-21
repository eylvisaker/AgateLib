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
		ConsoleDictionary commands = new ConsoleDictionary();

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
				builder.Append("    ");
				builder.Append((key + new string(' ', 20)).Substring(0, Math.Max(20, key.Length)));
				i++;

				if (i % 2 == 0)
				{
					builder.AppendLine();
				}
			}

			AgateConsole.WriteLine(builder.ToString());
		}

		public void Help(string command)
		{
			var methodInfo = commands[command].GetMethodInfo();
			
			var description = methodInfo?.GetCustomAttribute<DescriptionAttribute>();

			AgateConsole.WriteLine(description?.Description ?? "No description found.");
		}
		
		public bool Execute(string command)
		{
			string[] tokens = ConsoleTokenizer.Tokenize(command);

			if (commands.ContainsKey(tokens[0]))
			{
				ExecuteDelegate(commands[tokens[0]], tokens);
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

				commands.Add(name, method.CreateDelegate(Expression.GetDelegateType(
					(from parameter in method.GetParameters() select parameter.ParameterType)
					.Concat(new[] { method.ReturnType })
					.ToArray()), vocabulary));
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
