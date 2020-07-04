//
//    Copyright (c) 2006-2018 Erik Ylvisaker
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
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Diagnostics.CommandLibraries
{
    /// <summary>
    /// A vocabulary is an object that represents several entrypoints - methods which are called by 
    /// name by the user in the console window. 
    /// </summary>
    public class VocabularyCommands : ICommandLibrary
    {
        private class CommandInfo
        {
            public Delegate Delegate { get; set; }

            public ConsoleCommandAttribute CommandAttribute { get; set; }
        }

        private Dictionary<string, CommandInfo> commands;

        private readonly IVocabulary vocabulary;
        private IConsoleShell shell;

        /// <summary>
        /// Constructs a VocabularyCommands object.
        /// </summary>
        /// <param name="vocabulary">The IVocabulary object which has methods
        /// decorated with the ConsoleCommandAttribute.</param>
        public VocabularyCommands(IVocabulary vocabulary)
        {
            commands = new Dictionary<string, CommandInfo>(StringComparer.OrdinalIgnoreCase);

            this.vocabulary = vocabulary;

            BuildCommands();
        }

        public IConsoleShell Shell
        {
            get => shell;
            set
            {
                shell = value;
                vocabulary.Shell = shell;
            }
        }

        /// <summary>
        /// Shows help for this library.
        /// </summary>
        public void Help()
        {
            StringBuilder builder = new StringBuilder();

            int i = 0;
            foreach (var key in commands.Keys)
            {
                var info = commands[key];
                if (info.CommandAttribute.Hidden)
                {
                    continue;
                }

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
                Shell.WriteLine(builder.ToString());
            }
        }

        /// <summary>
        /// Shows help for a command.
        /// </summary>
        /// <param name="command"></param>
        public bool Help(string command)
        {
            if (commands.ContainsKey(command) == false)
            {
                return false;
            }

            var methodInfo = commands[command].Delegate.GetMethodInfo();

            var commandAttribute = methodInfo?.GetCustomAttribute<ConsoleCommandAttribute>();

            string usage = "    " + command;
            StringBuilder desc = new StringBuilder();

            foreach (ParameterInfo parameter in methodInfo.GetParameters())
            {
                var paramAttribute = parameter.GetCustomAttribute<ParamAttribute>();
                bool isOptional = parameter.IsOptional;

                if (paramAttribute?.Required == true)
                {
                    isOptional = false;
                }

                if (isOptional)
                {
                    usage += $" [{parameter.Name}]";
                }
                else
                {
                    usage += " " + parameter.Name;
                }

                desc.Append($"    {parameter.Name} ({FriendlyTypeName(parameter.ParameterType)})");

                if (paramAttribute != null)
                {
                    desc.Append($" - {paramAttribute.Description}");
                }

                desc.AppendLine();
            }

            Shell.WriteLine("Usage:");
            Shell.WriteLine();
            Shell.WriteLine(usage);
            Shell.WriteLine();
            Shell.WriteLine(commandAttribute?.Description ?? "No description found.");

            string descStr = desc.ToString();

            if (descStr.Trim().Length > 0)
            {
                Shell.WriteLine();
                Shell.WriteLine(desc.ToString());
            }

            return true;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
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

        private void BuildCommands()
        {
            commands.Clear();

            var methods = vocabulary.GetType().GetTypeInfo().DeclaredMethods
                .Where(x => x.GetCustomAttribute<ConsoleCommandAttribute>() != null);

            foreach (var method in methods)
            {
                var attrib = method.GetCustomAttribute<ConsoleCommandAttribute>();

                var name = ToDashConvention(method.Name);

                if (!string.IsNullOrWhiteSpace(attrib.Name))
                {
                    name = attrib.Name;
                }

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

        public IEnumerable<string> AutoCompleteEntries(string inputString)
        {
            foreach (var command in commands.Keys)
            {
                if (command.StartsWith(
                    inputString, StringComparison.OrdinalIgnoreCase))
                {
                    yield return command;
                }
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
                        Shell.WriteLine(
                            $"Argument #{j} invalid: \"{tokens[j]}\" not convertable to {FriendlyTypeName(parameters[i].ParameterType)}");
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
                            Shell.WriteLine($"Insufficient arguments for command: {tokens[0]}");
                        }
                        notEnoughArgs = true;

                        Shell.WriteLine($"    missing {parameters[i].ParameterType.Name} argument: {parameters[i].Name}");
                    }
                }
                else
                {
                    Shell.WriteLine($"[Ignoring extra argument: {tokens[j]}]");
                }
            }

            if (badArgs || notEnoughArgs)
            {
                Shell.Execute("help " + string.Join(" ", tokens));
                return;
            }

            object result = method.Invoke(p.Target, args);

            if (result is Task task)
            {
                task = task.ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.Faulted)
                    {
                        if (t.Exception is AggregateException ae)
                        {
                            foreach (var ex in ae.InnerExceptions)
                            {
                                Shell.WriteLine($"{ex.GetType().Name}: {ex.Message}");
                            }
                        }
                        else
                        {
                            Shell.WriteLine($"{t.Exception.GetType().Name}: {t.Exception.Message}");
                        }
                    }
                    else
                    {
                        Type type = t.GetType();
                        TypeInfo typeInfo = type.GetTypeInfo();

                        var voidTaskType = typeof(Task<>).MakeGenericType(Type.GetType("System.Threading.Tasks.VoidTaskResult"));
                        if (!voidTaskType.IsAssignableFrom(type))
                        {
                            Shell.WriteLine(typeInfo.GetDeclaredProperty("Result").GetValue(t).ToString());
                        }
                    }
                });

                Shell.State.AwaitingTask = task;
            }
            else if (method.ReturnType != typeof(void) && result != null)
            {
                Shell.WriteLine(result.ToString());
            }
        }

        private string ToDashConvention(string methodName)
        {
            StringBuilder result = new StringBuilder();
            var lower = methodName.ToLowerInvariant();

            for (int i = 0; i < methodName.Length; i++)
            {
                if (i > 0 && methodName[i] >= 'A' && methodName[i] <= 'Z')
                {
                    result.Append("-");
                }

                result.Append(lower[i]);
            }

            return result.ToString();
        }

        private string FriendlyTypeName(Type type)
        {
            if (type == typeof(string))
            {
                return "string";
            }

            if (type == typeof(int) || type == typeof(short))
            {
                return "integer";
            }

            if (type == typeof(byte))
            {
                return "byte";
            }

            if (type == typeof(double) || type == typeof(float))
            {
                return "float";
            }

            return type.Name;
        }

        public override string ToString() => $"Commands: <{vocabulary.GetType().Name}>";
    }
}
