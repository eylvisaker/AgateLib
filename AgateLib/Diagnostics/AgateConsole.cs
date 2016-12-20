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
using AgateLib.Diagnostics.ConsoleSupport;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.InputLib.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AgateLib.Diagnostics
{
	public abstract class AgateConsole : IDisposable, IInputHandler
	{
		#region --- Static Members ---

		protected static AgateConsole sInstance
		{
			get { return Core.State.Console.Instance; }
			set { Core.State.Console.Instance = value; }
		}

		public static bool IsInitialized { get { return sInstance != null; } }

		public static IFont Font
		{
			get { return Core.State.Console.Font; }
			set { Core.State.Console.Font = value; }
		}

		public static KeyCode VisibleToggleKey
		{
			get { return Core.State.Console.VisibleToggleKey; }
			set { Core.State.Console.VisibleToggleKey = value; }
		}

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

				if (sInstance.mVisible)
					Input.Handlers.BringToTop(sInstance);
				else
					Input.Handlers.SendToBack(sInstance);
			}
		}

		public static Color TextColor
		{
			get { return Core.State.Console.TextColor; }
			set { Core.State.Console.TextColor = value; }
		}
		public static Color EntryColor
		{
			get { return Core.State.Console.EntryColor; }
			set { Core.State.Console.EntryColor = value; }
		}
		public static Color BackgroundColor
		{
			get { return Core.State.Console.BackgroundColor; }
			set { Core.State.Console.BackgroundColor = value; }
		}

		public static AgateConsole Instance { get { return sInstance; } }

		public static ConsoleDictionary Commands { get { return Instance.CommandProcessor.Commands; } }

		public static void Initialize()
		{
			if (sInstance != null)
				return;

			sInstance = Core.Factory.PlatformFactory.CreateConsole();
			Input.Handlers.Add(sInstance);

			PrivateInitialize();
		}

		private static void PrivateInitialize()
		{
			if (sInstance == null)
				throw new InvalidOperationException();

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
				Font = DefaultAssets.Fonts.AgateMono;

			sInstance.DrawImpl();
		}

		/// <summary>
		/// Writes a line to the output part of the console window.
		/// </summary>
		/// <param name="text"></param>
		public static void WriteLine(string text, params object[] args)
		{
			if (sInstance == null)
				return;

			Instance.WriteLineImpl(string.Format(text, args));
		}
		/// <summary>
		/// Writes some text to the output part of the console window.
		/// </summary>
		/// <param name="text"></param>
		public static void Write(string text)
		{
			if (sInstance == null)
				return;

			Instance.WriteImpl(text);
		}

		#endregion

		protected bool mVisible = false;
		protected List<ConsoleMessage> mInputHistory = new List<ConsoleMessage>();
		List<ConsoleMessage> mMessages = new List<ConsoleMessage>();

		string mCurrentLine;
		int mHeight;
		int mHistoryIndex;

		ICommandProcessor mCommandProcessor = new AgateConsoleCommandProcessor();

		public AgateConsole()
		{
			if (sInstance != null)
				throw new InvalidOperationException("Cannot create two AgateConsole objects!");

			sInstance = this;
		}

		public void Dispose()
		{
			Dispose(true);
			sInstance = null;
		}
		protected virtual void Dispose(bool disposing) { }

		public ICommandProcessor CommandProcessor
		{
			get { return mCommandProcessor; }
			set { mCommandProcessor = value; }
		}

		public List<ConsoleMessage> Messages { get { return mMessages; } }

		void DrawImpl()
		{
			if (mVisible == false)
			{
				long time = CurrentTime;
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
				if (CurrentTime % 1000 < 500)
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

		protected abstract void WriteLineImpl(string text);
		protected abstract void WriteImpl(string text);

		#region --- Input Handling ---

		void IInputHandler.ProcessEvent(AgateInputEventArgs args)
		{
			ProcessEvent(args);
		}
		bool IInputHandler.ForwardUnhandledEvents
		{
			get { return true; }
		}

		private void ProcessEvent(AgateInputEventArgs args)
		{
			if (args.InputEventType == InputEventType.KeyDown &&
				VisibleToggleKey == args.KeyCode)
			{
				IsVisible = !IsVisible;
				args.Handled = true;

				sInstance.mHeight = Display.Coordinates.Height * 5 / 12;
			}
			else if (IsVisible)
			{
				if (args.InputEventType == InputEventType.KeyDown)
				{
					ProcessKeyDown(args.KeyCode, args.KeyString);
				}

				args.Handled = true;
			}
		}

		public void ProcessKeyDown(KeyCode keyCode, string keystring)
		{
			if (keyCode == KeyCode.Up)
			{
				mHistoryIndex++;

				if (mHistoryIndex > mInputHistory.Count)
					mHistoryIndex = mInputHistory.Count;
			}
			else if (keyCode == KeyCode.Down)
			{
				mHistoryIndex--;

				if (mHistoryIndex < 0)
					mHistoryIndex = 0;
			}
			else if (keyCode == KeyCode.Enter || keyCode == KeyCode.Return)
			{
				ModifyHistoryLine();

				ConsoleMessage input = new ConsoleMessage
				{
					Text = mCurrentLine,
					MessageType = ConsoleMessageType.UserInput,
					Time = CurrentTime
				};

				mMessages.Add(input);
				mInputHistory.Add(input);

				ExecuteCommand();
			}
			else if (string.IsNullOrEmpty(keystring) == false)
			{
				ModifyHistoryLine();

				if (keyCode == KeyCode.Tab)
					mCurrentLine += " ";
				else
					mCurrentLine += keystring;
			}
			else if (keyCode == KeyCode.BackSpace)
			{
				ModifyHistoryLine();

				if (mCurrentLine.Length > 0)
				{
					mCurrentLine = mCurrentLine.Substring(0, mCurrentLine.Length - 1);
				}
			}
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
				ProcessKeyDown(KeyCode.Enter, "\n");

				keys = keys.Substring(index + 1);
				index = keys.IndexOf('\n');
			}

			mCurrentLine += keys;
		}

		#endregion

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
				CommandProcessor.ExecuteCommand(tokens);
			}
			catch (System.Reflection.TargetInvocationException ex)
			{
				var e = ex.InnerException;

				WriteLine("Caught exception: {0}", e.GetType());
				WriteLine(e.Message);
			}
		}

		public string CurrentLine { get { return mCurrentLine; } }


		private void ModifyHistoryLine()
		{
			if (mHistoryIndex > 0)
			{
				mCurrentLine = mInputHistory[mInputHistory.Count - mHistoryIndex].Text;
				mHistoryIndex = 0;
			}
		}


		protected abstract long CurrentTime { get; }

	}
}
