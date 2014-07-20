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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using System.ComponentModel;
using AgateLib.DisplayLib.Shaders;

namespace AgateLib
{
	public class AgateConsole : IDisposable
	{
		#region --- Static Members ---

		static AgateConsole sInstance;

		public static bool IsInitialized { get { return sInstance != null; } }

		public static FontSurface Font { get; set; }
		public static KeyCode VisibleToggleKey { get; set; }
		public static bool IsVisible
		{
			get
			{
				if (sInstance == null)
					return false; 
				
				return sInstance.mVisible;
			}
			set
			{
				if (sInstance == null)
					throw new AgateException("You must initalize the console before making it visible.");

				sInstance.mVisible = value;
			}
		}

		public static Color TextColor { get; set; }
		public static Color EntryColor { get; set; }
		public static Color BackgroundColor { get; set; }
		public static AgateConsole Instance { get { return sInstance; } }

		public static ConsoleDictionary Commands { get { return Instance.CommandProcessor.Commands; } }

		public static void Initialize()
		{
			if (sInstance != null)
				return;

			PrivateInitialize();
		}

		private static void PrivateInitialize()
		{
			if (sInstance == null)
				sInstance = new AgateConsole();

			VisibleToggleKey = KeyCode.Tilde;

			TextColor = Color.White;
			EntryColor = Color.Yellow;
			BackgroundColor = Color.FromArgb(192, Color.Black);
		}
		/// <summary>
		/// Draws the console window. Call this right before your Display.EndFrame call.
		/// </summary>
		public static void Draw()
		{
			if (sInstance == null) return;

			if (Font == null)
				Font = FontSurface.AgateSans10;

			sInstance.DrawImpl();
		}

		internal static void Keyboard_KeyDown(InputEventArgs e)
		{
			if (e.KeyCode == VisibleToggleKey)
			{
				sInstance.mVisible = !sInstance.mVisible;
				sInstance.mHeight = Display.RenderTarget.Height * 5 / 12;
			}
			else if (sInstance.mVisible)
			{
				sInstance.ProcessKeyDown(e);
			}
		}
		internal static void Keyboard_KeyUp(InputEventArgs eventArgs)
		{
		}


		/// <summary>
		/// Writes a line to the output part of the console window.
		/// </summary>
		/// <param name="text"></param>
		public static void WriteLine(string text, params object[] args)
		{
			Instance.WriteLineImpl(string.Format(text, args));
		}
		/// <summary>
		/// Writes some text to the output part of the console window.
		/// </summary>
		/// <param name="text"></param>
		public static void Write(string text)
		{
			Instance.WriteImpl(text);
		}

		#endregion

		class AgateConsoleTraceListener : TraceListener
		{
			AgateConsole mOwner;
			ConsoleMessage current;
			Stopwatch watch = new Stopwatch();

			public AgateConsoleTraceListener(AgateConsole owner)
			{
				mOwner = owner;
				Trace.Listeners.Add(this);

				watch.Start();
			}

			public override void Write(string message)
			{
				if (current == null)
				{
					current = new ConsoleMessage();
					mOwner.mMessages.Add(current);
				}

				current.Time = watch.ElapsedMilliseconds;
				current.Text += message;
			}
			public override void WriteLine(string message)
			{
				if (current == null)
				{
					current = new ConsoleMessage();
					mOwner.mMessages.Add(current);
				}

				current.Text += message;
				current.Time = watch.ElapsedMilliseconds;
				current = null;
			}

			public Stopwatch Watch { get { return watch; } }
		}

		List<ConsoleMessage> mInputHistory = new List<ConsoleMessage>();
		List<ConsoleMessage> mMessages = new List<ConsoleMessage>();
		AgateConsoleTraceListener mTraceListener;
		ICommandProcessor mCommandProcessor = new AgateConsoleCommandProcessor();

		bool mVisible = false;
		string mCurrentLine;
		int mHeight;
		int mHistoryIndex;

		public AgateConsole()
		{
			if (sInstance != null)
				throw new InvalidOperationException("Cannot create two AgateConsole objects!");

			sInstance = this;
			Initialize();

			mTraceListener = new AgateConsoleTraceListener(this);
		}

		public ICommandProcessor CommandProcessor
		{
			get { return mCommandProcessor; }
			set { mCommandProcessor = value; }
		}

		public void Dispose()
		{
			Dispose(true);
			sInstance = null;
		}
		protected virtual void Dispose(bool disposing) { }


		protected void WriteLineImpl(string text)
		{
			mTraceListener.WriteLine(text);
		}
		protected void WriteImpl(string text)
		{
			mTraceListener.Write(text);
		}

		void DrawImpl()
		{
			if (mVisible == false)
			{
				long time = mTraceListener.Watch.ElapsedMilliseconds;
				int y = 0;
				Font.DisplayAlignment = OriginAlignment.TopLeft;
				Font.Color = TextColor;

				for (int i = 0; i < mMessages.Count; i++)
				{
					if (time - mMessages[i].Time > 5000)
						continue;
					if (mMessages[i].MessageType != ConsoleMessageType.Text)
						continue;

					Font.DrawText(new Point(0, y), mMessages[i].Text);
					y += Font.FontHeight;
				}

				while (mMessages.Count > 100)
					mMessages.RemoveAt(0);
			}
			else
			{
				Display.Shader = AgateBuiltInShaders.Basic2DShader;
				AgateBuiltInShaders.Basic2DShader.CoordinateSystem = new Rectangle(0, 0, Display.CurrentWindow.Width, Display.CurrentWindow.Height);

				Display.FillRect(new Rectangle(0, 0, Display.RenderTarget.Width, mHeight), BackgroundColor);

				int y = mHeight;
				Font.DisplayAlignment = OriginAlignment.BottomLeft;

				string currentLineText = "> ";

				if (mHistoryIndex != 0)
					currentLineText += EscapeText(mInputHistory[mInputHistory.Count - mHistoryIndex].Text);
				else
					currentLineText += EscapeText(mCurrentLine);

				Font.Color = EntryColor;
				Font.DrawText(0, y, currentLineText);

				// draw insertion point
				if (mTraceListener.Watch.ElapsedMilliseconds % 1000 < 500)
				{
					int x = Font.MeasureString(currentLineText).Width;

					Display.DrawLine(
						new Point(x, y - Font.FontHeight),
						new Point(x, y),
						EntryColor);
				}

				for (int i = mMessages.Count - 1; i >= 0; i--)
				{
					y -= Font.FontHeight;
					var message = mMessages[i];

					if (message.MessageType == ConsoleMessageType.UserInput)
					{
						Font.Color = EntryColor;
						Font.DrawText(0, y, "> " + EscapeText(message.Text));
					}
					else
					{
						Font.Color = TextColor;
						Font.DrawText(0, y, EscapeText(message.Text));
					}

					if (y < 0)
						break;
				}
			}
		}

		private string EscapeText(string p)
		{
			if (p == null)
				return p;

			return p.Replace("{", "{{}");
		}


		/// <summary>
		/// Sends the key string to the console as if the user typed it.
		/// </summary>
		/// <param name="keys"></param>
		/// <remarks>
		/// Control characters are treated specially. A line feed (\n) is
		/// treated as the end of line. \r is ignored. 
		/// \t is converted to a space.
		/// </remarks>
		public void ProcessKeys(string keys)
		{
			keys = keys.Replace('\t', ' ');
			keys = keys.Replace("\r", "");

			ModifyHistoryLine();

			int index = keys.IndexOf('\n');
			while (index > -1)
			{
				mCurrentLine += keys.Substring(0, index);
				ProcessKeyDown(new InputEventArgs(KeyCode.Enter, new KeyModifiers()));

				keys = keys.Substring(index + 1);
				index = keys.IndexOf('\n');
			}

			mCurrentLine += keys;
		}
		/// <summary>
		/// Processes an input key.
		/// </summary>
		/// <param name="e"></param>
		public void ProcessKeyDown(InputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Up)
			{
				mHistoryIndex++;

				if (mHistoryIndex > mInputHistory.Count)
					mHistoryIndex = mInputHistory.Count;
			}
			else if (e.KeyCode == KeyCode.Down)
			{
				mHistoryIndex--;

				if (mHistoryIndex < 0)
					mHistoryIndex = 0;
			}
			else if (e.KeyCode == KeyCode.Enter || e.KeyCode == KeyCode.Return)
			{
				ModifyHistoryLine();

				ConsoleMessage input = new ConsoleMessage
				{
					Text = mCurrentLine,
					MessageType = ConsoleMessageType.UserInput,
					Time = mTraceListener.Watch.ElapsedMilliseconds
				};

				mMessages.Add(input);
				mInputHistory.Add(input);

				ExecuteCommand();
			}
			else if (string.IsNullOrEmpty(e.KeyString) == false)
			{
				ModifyHistoryLine();

				if (e.KeyCode == KeyCode.Tab)
					mCurrentLine += " ";
				else
					mCurrentLine += e.KeyString;
			}
			else if (e.KeyCode == KeyCode.BackSpace)
			{
				ModifyHistoryLine();

				if (mCurrentLine.Length > 0)
				{
					mCurrentLine = mCurrentLine.Substring(0, mCurrentLine.Length - 1);
				}
			}
		}

		private void ModifyHistoryLine()
		{
			if (mHistoryIndex > 0)
			{
				mCurrentLine = mInputHistory[mInputHistory.Count - mHistoryIndex].Text;
				mHistoryIndex = 0;
			}
		}

		static char[] splitters = new char[] { ' ' };

		private void ExecuteCommand()
		{
			if (string.IsNullOrEmpty(mCurrentLine))
				return;
			if (mCurrentLine.Trim() == string.Empty)
				return;

			// regular expression obtained from 
			// http://stackoverflow.com/questions/554013/regular-expression-to-split-on-spaces-unless-in-quotes
			//
			var regexMatches = Regex.Matches(mCurrentLine, @"((""((?<token>.*?)(?<!\\)"")|(?<token>[\w]+))(\s)*)");

			string[] tokens = (from Match m in regexMatches
							   where m.Groups["token"].Success
							   select m.Groups["token"].Value).ToArray();

			tokens[0] = tokens[0].ToLowerInvariant();

			mCurrentLine = string.Empty;

			try
			{
				mCommandProcessor.ExecuteCommand(tokens);
			}
			catch (System.Reflection.TargetInvocationException ex)
			{
				var e = ex.InnerException;

				WriteLine("Caught exception: {0}", e.GetType());
				WriteLine(e.Message);
			}
		}

		public string CurrentLine { get { return mCurrentLine; } }
	}

	public class ConsoleDictionary : Dictionary<string, Delegate>
	{
		public void Add<T>(string key, Action<T> value)
		{
			base.Add(key, value);
		}
		public void Add<T1, T2>(string key, Action<T1, T2> value)
		{
			base.Add(key, value);
		}
		public void Add<T1, T2, T3>(string key, Action<T1, T2, T3> value)
		{
			base.Add(key, value);
		}
		public void Add<T1, T2, T3, T4>(string key, Action<T1, T2, T3, T4> value)
		{
			base.Add(key, value);
		}

		public void Add<TResult>(string key, Func<TResult> value)
		{
			base.Add(key, value);
		}
		public void Add<T1, TResult>(string key, Func<T1, TResult> value)
		{
			base.Add(key, value);
		}
		public void Add<T1, T2, TResult>(string key, Func<T1, T2, TResult> value)
		{
			base.Add(key, value);
		}
		public void Add<T1, T2, T3, TResult>(string key, Func<T1, T2, T3, TResult> value)
		{
			base.Add(key, value);
		}
		public void Add<T1, T2, T3, T4, TResult>(string key, Func<T1, T2, T3, T4, TResult> value)
		{
			base.Add(key, value);
		}
	}
	public delegate string DescribeCommandHandler(string command);

	class ConsoleMessage
	{
		public string Text;
		public long Time;
		public ConsoleMessageType MessageType;
	}

	enum ConsoleMessageType
	{
		Text,
		UserInput,
	}

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
						args[i] = Convert.ChangeType(tokens[i + 1], parameters[i].ParameterType);
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
