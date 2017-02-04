﻿//     The contents of this file are subject to the Mozilla Public License
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
	class AgateConsoleCore : IAgateConsole
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

		public event EventHandler VisibleChanged;

		public event EventHandler KeyProcessed;

		private long CurrentTime => AgateApp.State.App.MasterTime.ElapsedMilliseconds;

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
				Time = AgateApp.State.App.MasterTime.ElapsedMilliseconds,
				MessageType = ConsoleMessageType.Text,
			};

			WriteMessage(message);
		}

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
			ClearTemporaryMessage();

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
				ClearInputText();
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
	}
}