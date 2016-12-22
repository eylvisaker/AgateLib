using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AgateLib.ApplicationModels;
using AgateLib.Diagnostics.ConsoleSupport;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Quality;

namespace AgateLib.Diagnostics
{
	public class AgateConsoleImpl : IAgateConsole
	{
		protected bool mVisible = false;
		protected List<ConsoleMessage> mInputHistory = new List<ConsoleMessage>();
		List<ConsoleMessage> mMessages = new List<ConsoleMessage>();

		string mCurrentLine;
		int mHeight;
		int mHistoryIndex;

		private IList<ICommandLibrary> commandLibraries = new List<ICommandLibrary>();
		private LibraryVocabulary emergencyVocab;

		public AgateConsoleImpl()
		{
			emergencyVocab = new LibraryVocabulary(new AgateEmergencyVocabulary(this));
		}

		private long CurrentTime => Core.State.Core.MasterTime.ElapsedMilliseconds;

		public IList<ICommandLibrary> CommandLibraries
		{
			get { return commandLibraries; }
			set
			{
				Condition.RequireArgumentNotNull(value, nameof(CommandLibraries));
				commandLibraries = value;
			}
		}

		internal IEnumerable<ICommandLibrary> CommandLibrarySet
		{
			get
			{
				yield return emergencyVocab;

				foreach (var library in commandLibraries)
					yield return library;
			}
		}

		public KeyCode VisibleToggleKey
		{
			get { return Core.State.Console.VisibleToggleKey; }
			set { Core.State.Console.VisibleToggleKey = value; }
		}

		public bool IsVisible
		{
			get { return mVisible; }
			set
			{
				mVisible = value;

				if (mVisible)
					Input.Handlers.BringToTop(this);
				else
					Input.Handlers.SendToBack(this);
			}
		}

		public Color TextColor
		{
			get { return Core.State.Console.TextColor; }
			set { Core.State.Console.TextColor = value; }
		}
		public Color EntryColor
		{
			get { return Core.State.Console.EntryColor; }
			set { Core.State.Console.EntryColor = value; }
		}
		public Color BackgroundColor
		{
			get { return Core.State.Console.BackgroundColor; }
			set { Core.State.Console.BackgroundColor = value; }
		}

		public IFont Font { get { return Core.State.Console.Font; } }

		public List<ConsoleMessage> Messages { get { return mMessages; } }

		public void Draw()
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
					var message = mMessages[i];
					var text = message.Text;

					if (message.MessageType == ConsoleMessageType.UserInput)
					{
						Font.Color = EntryColor;
						text = "> " + message.Text;
					}
					else
					{
						Font.Color = TextColor;
					}

					var size = Font.MeasureString(text);

					y -= size.Height;
					Font.DrawText(0, y, text);

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

		public void WriteMessage(ConsoleMessage message)
		{
			Messages.Add(message);
		}

		public void WriteLine(string text)
		{
			var message = new ConsoleMessage
			{
				Text = text,
				Time = Core.State.Core.MasterTime.ElapsedMilliseconds,
				MessageType = ConsoleMessageType.Text,
			};

			WriteMessage(message);
		}

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

				mHeight = Display.Coordinates.Height * 5 / 12;
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

				try
				{
					Execute(mCurrentLine);
				}
				finally
				{
					mCurrentLine = "";
				}
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

		public void Execute(string command)
		{
			if (string.IsNullOrEmpty(mCurrentLine))
				return;
			if (mCurrentLine.Trim() == string.Empty)
				return;

			bool isDebugCommand = IsDebugCommand(command);

			foreach (var commandProcessor in CommandLibrarySet)
			{
				try
				{
					bool execStatus = commandProcessor.Execute(command);

					if (execStatus && !isDebugCommand)
					{
						return;
					}
				}
				catch (TargetInvocationException e)
				{
					WriteLine("Failed to exeute command.");
					WriteLine(e.InnerException.ToString());

					return;
				}
				catch (Exception e)
				{
					WriteLine("Failed to execute command.");
					WriteLine(e.ToString());

					return;
				}
			}

			if (!isDebugCommand)
			{
				WriteLine("Unknown command.");
			}
		}

		private bool IsDebugCommand(string command)
		{
			return command == "debug" || command.StartsWith("debug ");
		}


		private void WriteLineFormat(string format, params object[] args)
		{
			WriteLine(string.Format(format, args));
		}

		public string InputText { get { return mCurrentLine; } }

		private void ModifyHistoryLine()
		{
			if (mHistoryIndex > 0)
			{
				mCurrentLine = mInputHistory[mInputHistory.Count - mHistoryIndex].Text;
				mHistoryIndex = 0;
			}
		}
	}
}
