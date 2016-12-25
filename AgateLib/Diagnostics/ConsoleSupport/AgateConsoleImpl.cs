using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
		private bool visible = false;
		private List<ConsoleMessage> inputHistory = new List<ConsoleMessage>();
		private List<ConsoleMessage> messages = new List<ConsoleMessage>();

		private string inputText = "";
		private int height;
		private int historyIndex;
		private int insertionPoint;
		private long timeOffset;

		private IList<ICommandLibrary> commandLibraries = new List<ICommandLibrary>();
		private LibraryVocabulary emergencyVocab;

		public AgateConsoleImpl()
		{
			emergencyVocab = new LibraryVocabulary(new AgateEmergencyVocabulary(this));
		}

		private long CurrentTime => Core.State.Core.MasterTime.ElapsedMilliseconds;

		internal IEnumerable<ICommandLibrary> CommandLibrarySet
		{
			get
			{
				yield return emergencyVocab;

				foreach (var library in commandLibraries)
					yield return library;
			}
		}

		public IList<ICommandLibrary> CommandLibraries
		{
			get { return commandLibraries; }
			set
			{
				Condition.RequireArgumentNotNull(value, nameof(CommandLibraries));
				commandLibraries = value;
			}
		}

		public KeyCode VisibleToggleKey
		{
			get { return Core.State.Console.VisibleToggleKey; }
			set { Core.State.Console.VisibleToggleKey = value; }
		}

		public bool IsVisible
		{
			get { return visible; }
			set { visible = value; }
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

		public string InputText { get { return inputText; } }

		public IFont Font { get { return Core.State.Console.Font; } }

		public List<ConsoleMessage> Messages { get { return messages; } }

		public void Draw()
		{
			Display.Shader = AgateBuiltInShaders.Basic2DShader;
			AgateBuiltInShaders.Basic2DShader.CoordinateSystem = new Rectangle(0, 0, Display.CurrentWindow.Width, Display.CurrentWindow.Height);

			if (visible == false)
			{
				DrawRecentMessages();
			}
			else
			{
				DrawConsoleWindow();
			}
		}

		private void DrawRecentMessages()
		{
			long time = CurrentTime;
			int y = 0;
			Font.DisplayAlignment = OriginAlignment.TopLeft;
			Font.Color = TextColor;

			for (int i = 0; i < messages.Count; i++)
			{
				if (time - messages[i].Time > 5000)
					continue;
				if (messages[i].MessageType != ConsoleMessageType.Text)
					continue;

				Font.DrawText(new Point(0, y), messages[i].Text);
				y += Font.FontHeight;
			}

			while (messages.Count > 100)
				messages.RemoveAt(0);
		}

		private void DrawConsoleWindow()
		{
			Display.FillRect(new Rectangle(0, 0, Display.RenderTarget.Width, height), BackgroundColor);

			int y = height;
			Font.DisplayAlignment = OriginAlignment.BottomLeft;

			const string entryPrefix = "> ";
			string currentLineText = entryPrefix;

			currentLineText += EscapeText(inputText);

			Font.Color = EntryColor;
			Font.DrawText(0, y, currentLineText);

			// draw insertion point
			if ((CurrentTime - timeOffset) % 1000 < 500)
			{
				int x = Font.MeasureString(currentLineText.Substring(0, entryPrefix.Length + insertionPoint)).Width;

				Display.DrawLine(
					new Point(x, y - Font.FontHeight),
					new Point(x, y),
					EntryColor);
			}

			for (int i = messages.Count - 1; i >= 0; i--)
			{
				var message = messages[i];

				if (message.Layout == null)
				{
					var text = message.Text;

					if (message.MessageType == ConsoleMessageType.UserInput)
					{
						Font.Color = EntryColor;
						text = entryPrefix + message.Text;
					}
					else
					{
						Font.Color = TextColor;
					}

					message.Layout = Font.LayoutText(text, Display.RenderTarget.Width);
				}

				y -= message.Layout.Height;
				message.Layout.Draw(new Point(0, y));

				if (y < 0)
					break;
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

				height = Display.Coordinates.Height * 5 / 12;
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
			timeOffset = CurrentTime;

			if (keyCode == KeyCode.Up)
			{
				IncrementHistoryIndex();
			}
			else if (keyCode == KeyCode.Down)
			{
				DecrementHistoryIndex();
			}
			else if (keyCode == KeyCode.Left)
			{
				DecrementInsertionPoint();
			}
			else if (keyCode == KeyCode.Right)
			{
				IncrementInsertionPoint();
			}
			else if (keyCode == KeyCode.Enter || keyCode == KeyCode.Return)
			{
				AcceptEntry();
			}
			else if (string.IsNullOrEmpty(keystring) == false)
			{
				InsertKey(keyCode, keystring);
			}
			else if (keyCode == KeyCode.BackSpace)
			{
				Backspace();
			}
			else if (keyCode == KeyCode.Delete)
			{
				Delete();
			}
		}

		private void IncrementHistoryIndex()
		{
			historyIndex++;

			if (historyIndex > inputHistory.Count)
				historyIndex = inputHistory.Count;

			LoadHistoryToInput();
		}

		private void DecrementHistoryIndex()
		{
			historyIndex--;

			if (historyIndex < 0)
				historyIndex = 0;

			LoadHistoryToInput();
		}

		private void LoadHistoryToInput()
		{
			if (historyIndex == 0)
				inputText = "";
			else
			{
				inputText = inputHistory[inputHistory.Count - historyIndex].Text;
			}
			insertionPoint = inputText.Length;
		}

		private void AcceptEntry()
		{
			ConsoleMessage input = new ConsoleMessage
			{
				Text = inputText,
				MessageType = ConsoleMessageType.UserInput,
				Time = CurrentTime
			};

			messages.Add(input);
			inputHistory.Add(input);

			try
			{
				Execute(inputText);
			}
			finally
			{
				inputText = "";
				insertionPoint = 0;
				historyIndex = 0;
			}
		}

		private void IncrementInsertionPoint()
		{
			insertionPoint++;

			if (insertionPoint > inputText.Length)
				insertionPoint = inputText.Length;
		}

		private void DecrementInsertionPoint()
		{
			insertionPoint--;

			if (insertionPoint < 0)
				insertionPoint = 0;
		}

		private void Backspace()
		{
			if (inputText.Length > 0 && insertionPoint > 0)
			{
				if (insertionPoint == inputText.Length)
				{
					inputText = inputText.Substring(0, inputText.Length - 1);
					insertionPoint--;
				}
				else
				{
					inputText = inputText.Substring(0, insertionPoint - 1) + inputText.Substring(insertionPoint);
					insertionPoint--;
				}
			}
		}

		private void Delete()
		{
			if (insertionPoint < inputText.Length - 1)
			{
				inputText = inputText.Substring(0, insertionPoint) + inputText.Substring(insertionPoint + 1);
			}
			else if (insertionPoint == inputText.Length - 1)
			{
				inputText = inputText.Substring(0, insertionPoint);
			}
		}

		private void InsertKey(KeyCode keyCode, string keystring)
		{
			string insertString = keystring;

			if (keyCode == KeyCode.Tab)
				insertString = " ";

			InsertText(insertString);
		}

		private void InsertText(string insertString)
		{
			if (insertionPoint == inputText.Length)
			{
				inputText += insertString;
			}
			else
			{
				inputText = inputText.Substring(0, insertionPoint) + insertString + inputText.Substring(insertionPoint);
			}

			insertionPoint += insertString.Length;
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

			int index = keys.IndexOf('\n');
			while (index > -1)
			{
				InsertText(keys.Substring(0, index));
				ProcessKeyDown(KeyCode.Enter, "\n");

				keys = keys.Substring(index + 1);
				index = keys.IndexOf('\n');
			}

			InsertText(keys);
		}

		#endregion

		public void Execute(string command)
		{
			if (string.IsNullOrEmpty(inputText))
				return;
			if (inputText.Trim() == string.Empty)
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
					ExecuteFailure(e.InnerException);

					return;
				}
				catch (Exception e)
				{
					ExecuteFailure(e);

					return;
				}
			}

			if (!isDebugCommand)
			{
				WriteLine("Unknown command.");
			}
		}

		private void ExecuteFailure(Exception e)
		{
			if (Core.State.Debug)
			{
				WriteLine("Failed to execute command.");
				WriteLine(e.ToString());
			}
			else
			{
				WriteLine(e.Message);
				WriteLine("(Type 'help <command>' to get more information.)");
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
	}
}
