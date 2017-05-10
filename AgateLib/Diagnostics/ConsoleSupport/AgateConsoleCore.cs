//
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
using System.Reflection;
using System.Text;
using AgateLib.InputLib;
using AgateLib.Quality;

namespace AgateLib.Diagnostics.ConsoleSupport
{
	internal class AgateConsoleCore : IAgateConsole
	{
		private bool visible = false;
		private List<ConsoleMessage> inputHistory = new List<ConsoleMessage>();
		private List<ConsoleMessage> messages = new List<ConsoleMessage>();

		private string inputText = "";
		private int historyIndex;
		private int insertionPoint;
		private int viewShift;

		private IList<ICommandLibrary> commandLibraries = new List<ICommandLibrary>();
		private LibraryVocabulary emergencyVocab;

		public AgateConsoleCore()
		{
			emergencyVocab = new LibraryVocabulary(new AgateEmergencyVocabulary(this));
		}

		/// <summary>
		/// Event raised when the visibility is changed.
		/// </summary>
		public event EventHandler VisibleChanged;

		/// <summary>
		/// Event raised after processing a user keystroke.
		/// </summary>
		public event EventHandler KeyProcessed;

		private long CurrentTime => (long)AgateApp.State.App.ApplicationClock.CurrentTime.TotalMilliseconds;

		/// <summary>
		/// Returns the entire list of command libraries, including those
		/// built-into AgateLib.
		/// </summary>
		internal IEnumerable<ICommandLibrary> CommandLibrarySet
		{
			get
			{
				yield return emergencyVocab;

				foreach (var library in commandLibraries)
					yield return library;
			}
		}

		/// <summary>
		/// Gets or sets the list of command libraries for the application has
		/// installed.
		/// </summary>
		public IList<ICommandLibrary> CommandLibraries
		{
			get { return commandLibraries; }
			set
			{
				Require.ArgumentNotNull(value, nameof(CommandLibraries));
				commandLibraries = value;
			}
		}

		public KeyCode VisibleToggleKey
		{
			get { return AgateApp.State.Console.VisibleToggleKey; }
			set { AgateApp.State.Console.VisibleToggleKey = value; }
		}

		public bool IsVisible
		{
			get { return visible; }
			set
			{
				visible = value;
				VisibleChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public string InputText => inputText;

		public int ViewShift => viewShift;

		public int InsertionPoint => insertionPoint;

		public IReadOnlyList<ConsoleMessage> Messages => messages;

		public void WriteMessage(ConsoleMessage message)
		{
			if (message.MessageType == ConsoleMessageType.Temporary)
				ClearTemporaryMessage();

			while (messages.Count > 100)
				messages.RemoveAt(0);

			messages.Add(message);
		}

		private void ClearTemporaryMessage()
		{
			messages.RemoveAll(x => x.MessageType == ConsoleMessageType.Temporary);
		}

		public void WriteLine(string text)
		{
			var message = new ConsoleMessage
			{
				Text = text,
				Time = (long)AgateApp.State.App.ApplicationClock.CurrentTime.TotalMilliseconds,
				MessageType = ConsoleMessageType.Text,
			};

			WriteMessage(message);
		}

		public void Execute(string command)
		{
			if (string.IsNullOrEmpty(command))
				return;
			if (command.Trim() == string.Empty)
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

		private void AutoCompleteEntry()
		{
			var values = CommandLibrarySet.SelectMany(x => x.AutoCompleteEntries(InputText.Substring(0, insertionPoint))).ToList();

			if (values.Count == 1)
			{
				var text = values.Single() + " ";
				var remainder = InputText.Substring(insertionPoint);

				inputText = text + remainder;

				insertionPoint = text.Length;

				ClearTemporaryMessage();
			}
			else if (values.Count > 0)
			{
				const int maxDisplay = 6;

				var text = new StringBuilder();

				foreach (var value in values.Take(maxDisplay))
					text.AppendLine($"    {value}");

				if (values.Count > maxDisplay)
					text.AppendLine($"... and {values.Count - maxDisplay} more.");

				var message = new ConsoleMessage
				{
					Text = text.ToString().TrimEnd(),
					MessageType = ConsoleMessageType.Temporary
				};

				WriteMessage(message);
			}
			else
			{
				var message = new ConsoleMessage
				{
					Text = "No autocompletion found.",
					MessageType = ConsoleMessageType.Temporary
				};

				WriteMessage(message);
			}
		}

		private void ExecuteFailure(Exception e)
		{
			if (AgateApp.State.Debug)
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

		#region --- Input Handling ---

		void IInputHandler.ProcessEvent(AgateInputEventArgs args)
		{
			ProcessEvent(args);
		}

		bool IInputHandler.ForwardUnhandledEvents => true;

		private void ProcessEvent(AgateInputEventArgs args)
		{
			if (args.InputEventType == InputEventType.KeyDown &&
				VisibleToggleKey == args.KeyCode)
			{
				IsVisible = !IsVisible;
				args.Handled = true;
			}
			else if (IsVisible)
			{
				if (args.InputEventType == InputEventType.KeyDown)
				{
					ProcessKeyDown(args.KeyCode, args.KeyString, args.KeyModifiers);
				}

				args.Handled = true;
			}
		}

		public void ProcessKeyDown(KeyCode keyCode, string keystring, KeyModifiers modifiers = default(KeyModifiers))
		{
			if (keyCode == KeyCode.Escape)
			{
				IsVisible = false;
			}
			else if (keyCode == KeyCode.C && modifiers.Control)
			{
				ClearInputText();
			}
			else if (keyCode == KeyCode.Up)
			{
				if (modifiers.Shift)
					ShiftViewUp();
				else
					IncrementHistoryIndex();
			}
			else if (keyCode == KeyCode.Down)
			{
				if (modifiers.Shift)
					ShiftViewDown();
				else
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
			else if (keyCode == KeyCode.Tab)
			{
				AutoCompleteEntry();
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

			KeyProcessed?.Invoke(this, EventArgs.Empty);
		}

		private void ClearInputText()
		{
			inputText = "";
			insertionPoint = 0;
		}

		private void ShiftViewUp()
		{
			viewShift++;
		}

		private void ShiftViewDown()
		{
			viewShift--;

			if (viewShift < 0)
				viewShift = 0;
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
			inputText = historyIndex == 0 ? "" : inputHistory[inputHistory.Count - historyIndex].Text;
			insertionPoint = inputText.Length;
		}

		private void AcceptEntry()
		{
			ClearTemporaryMessage();

			ConsoleMessage input = new ConsoleMessage
			{
				Text = inputText,
				MessageType = ConsoleMessageType.UserInput,
				Time = CurrentTime
			};

			messages.Add(input);

			if (!string.IsNullOrWhiteSpace(inputText))
			{
				inputHistory.Add(input);
			}

			var command = inputText;

			ClearInputText();
			historyIndex = 0;
			viewShift = 0;

			Execute(command);
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
	}
}
