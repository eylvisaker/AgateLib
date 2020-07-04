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

using AgateLib.Diagnostics.CommandLibraries;
using AgateLib.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Diagnostics
{
    public interface IConsoleShell
    {
        /// <summary>
        /// Gets the state data for the console.
        /// </summary>
        ConsoleState State { get; }

        /// <summary>
        /// Returns true if the user has the console window open.
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Writes a line to the output portion of the screen.
        /// </summary>
        /// <param name="text"></param>
        void WriteLine(string text = "");

        /// <summary>
        /// Creates an object that allows updates to the game while it is active.
        /// </summary>
        /// <returns></returns>
        IDisposable AllowGameUpdates();

        /// <summary>
        /// Executes a command, as if the user had typed it in.
        /// </summary>
        /// <param name="command"></param>
        void Execute(string command);
    }

    [Singleton]
    public class ConsoleShell : IConsoleShell
    {
        private class GameUpdater : IDisposable
        {
            private ConsoleState state;

            public GameUpdater(ConsoleState state)
            {
                this.state = state;

                state.PauseGame = false;
            }

            public void Dispose()
            {
                state.PauseGame = true;
            }
        }

        private List<ConsoleMessage> inputHistory = new List<ConsoleMessage>();

        private int historyIndex;

        private List<ICommandLibrary> commandLibraries = new List<ICommandLibrary>();

        private VocabularyCommands emergencyVocab;
        private GameTime lastTime = new GameTime();
        private long CurrentTime;

        private Task awaitingTask;

        public ConsoleShell()
        {
            emergencyVocab = new VocabularyCommands(new AgateEmergencyVocabulary(this));

            commandLibraries.Add(new VocabularyCommands(new AgateConsoleVocabulary()));
        }

        public ConsoleState State { get; } = new ConsoleState();

        /// <summary>
        /// Event raised after processing a user keystroke.
        /// </summary>
        public event EventHandler KeyProcessed;

        public bool IsVisible => State.DisplayMode == ConsoleDisplayMode.Full;

        private string InputText
        {
            get => State.InputText;
            set => State.InputText = value;
        }

        private int InsertionPoint
        {
            get => State.InsertionPoint;
            set => State.InsertionPoint = value;
        }

        private List<ConsoleMessage> Messages => State.Messages;

        public IDisposable AllowGameUpdates()
        {
            return new GameUpdater(State);
        }

        public void WaitForTask(Task task)
        {
            if (awaitingTask != null)
            {
                awaitingTask = Task.WhenAll(awaitingTask, task);
            }
            else
            {
                awaitingTask = task;
            }
        }

        /// <summary>
        /// Returns the entire list of command libraries, including those
        /// built-into AgateLib.
        /// </summary>
        internal IEnumerable<ICommandLibrary> CommandLibrarySet
        {
            get
            {
                foreach (ICommandLibrary library in commandLibraries)
                {
                    yield return library;
                }

                yield return emergencyVocab;
            }
        }

        /// <summary>
        /// Gets the list of command libraries for the application has
        /// installed.
        /// </summary>
        public IReadOnlyList<ICommandLibrary> CommandLibraries
        {
            get => commandLibraries;
        }

        public void AddCommands(ICommandLibrary commands)
        {
            commandLibraries.Add(commands);
        }

        public void Update(GameTime time)
        {
            lastTime.ElapsedGameTime = time.ElapsedGameTime;
            lastTime.IsRunningSlowly = time.IsRunningSlowly;
            lastTime.TotalGameTime = time.TotalGameTime;

            CurrentTime = (long)time.TotalGameTime.TotalMilliseconds;
        }

        public void WriteMessage(ConsoleMessage message)
        {
            if (message.MessageType == ConsoleMessageType.Temporary)
            {
                ClearTemporaryMessage();
            }

            while (Messages.Count > 100)
            {
                Messages.RemoveAt(0);
            }

            Messages.Add(message);
        }

        private void ClearTemporaryMessage()
        {
            Messages.RemoveAll(x => x.MessageType == ConsoleMessageType.Temporary);
        }

        public void WriteLine(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);

            // Hack because ContentLayout won't print a completely empty string.
            if (string.IsNullOrEmpty(text))
            {
                text = " ";
            }

            string[] lines = text.Split('\n');

            foreach (var line in lines)
            {
                var message = new ConsoleMessage
                {
                    Text = line,
                    Time = CurrentTime,
                    MessageType = ConsoleMessageType.Text,
                };

                WriteMessage(message);
            }
        }

        public void Execute(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                return;
            }

            if (command.Trim() == string.Empty)
            {
                return;
            }

            bool isDebugCommand = IsDebugCommand(command);

            foreach (var commandProcessor in CommandLibrarySet)
            {
                try
                {
                    commandProcessor.Shell = this;

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
            var values = CommandLibrarySet.SelectMany(x => x.AutoCompleteEntries(InputText.Substring(0, InsertionPoint))).ToList();

            if (values.Count == 1)
            {
                var text = values.Single() + " ";
                var remainder = InputText.Substring(InsertionPoint);

                InputText = text + remainder;

                InsertionPoint = text.Length;

                ClearTemporaryMessage();
            }
            else if (values.Count > 0)
            {
                const int maxDisplay = 6;

                var text = new StringBuilder();

                foreach (var value in values.Take(maxDisplay))
                {
                    text.AppendLine($"    {value}");
                }

                if (values.Count > maxDisplay)
                {
                    text.AppendLine($"... and {values.Count - maxDisplay} more.");
                }

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
            if (e is AgateConsoleException)
            {
                WriteLine(e.Message);
            }
            else if (State.Debug)
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

        public void ProcessKeyDown(Keys keyCode, string keystring, IKeyModifiers modifiers)
        {
            if (State.ExecutingTask)
            {
                return;
            }

            if (keyCode == Keys.C && modifiers.Control)
            {
                ClearInputText();
            }
            else if (keyCode == Keys.Up)
            {
                if (modifiers.Shift)
                {
                    ShiftViewUp();
                }
                else
                {
                    IncrementHistoryIndex();
                }
            }
            else if (keyCode == Keys.Down)
            {
                if (modifiers.Shift)
                {
                    ShiftViewDown();
                }
                else
                {
                    DecrementHistoryIndex();
                }
            }
            else if (keyCode == Keys.Left)
            {
                DecrementInsertionPoint();
            }
            else if (keyCode == Keys.Right)
            {
                IncrementInsertionPoint();
            }
            else if (keyCode == Keys.Enter)
            {
                AcceptEntry();
            }
            else if (keyCode == Keys.Tab)
            {
                AutoCompleteEntry();
            }
            else if (string.IsNullOrEmpty(keystring) == false)
            {
                InsertKey(keyCode, keystring);
            }
            else if (keyCode == Keys.Back)
            {
                Backspace();
            }
            else if (keyCode == Keys.Delete)
            {
                Delete();
            }

            KeyProcessed?.Invoke(this, EventArgs.Empty);
        }

        private void ClearInputText()
        {
            InputText = "";
            InsertionPoint = 0;
        }

        private void ShiftViewUp()
        {
            State.ViewShift++;
        }

        private void ShiftViewDown()
        {
            State.ViewShift--;

            if (State.ViewShift < 0)
            {
                State.ViewShift = 0;
            }
        }

        private void IncrementHistoryIndex()
        {
            historyIndex++;

            if (historyIndex > inputHistory.Count)
            {
                historyIndex = inputHistory.Count;
            }

            LoadHistoryToInput();
        }

        private void DecrementHistoryIndex()
        {
            historyIndex--;

            if (historyIndex < 0)
            {
                historyIndex = 0;
            }

            LoadHistoryToInput();
        }

        private void LoadHistoryToInput()
        {
            InputText = historyIndex == 0 ? "" : inputHistory[inputHistory.Count - historyIndex].Text;
            InsertionPoint = InputText.Length;
        }

        private void AcceptEntry()
        {
            ClearTemporaryMessage();

            ConsoleMessage input = new ConsoleMessage
            {
                Text = InputText,
                MessageType = ConsoleMessageType.UserInput,
                Time = CurrentTime
            };

            Messages.Add(input);

            if (!string.IsNullOrWhiteSpace(InputText))
            {
                inputHistory.Add(input);
            }

            var command = InputText;

            ClearInputText();
            historyIndex = 0;
            State.ViewShift = 0;

            Execute(command);
        }

        private void IncrementInsertionPoint()
        {
            InsertionPoint++;

            if (InsertionPoint > InputText.Length)
            {
                InsertionPoint = InputText.Length;
            }
        }

        private void DecrementInsertionPoint()
        {
            InsertionPoint--;

            if (InsertionPoint < 0)
            {
                InsertionPoint = 0;
            }
        }

        private void Backspace()
        {
            if (InputText.Length > 0 && InsertionPoint > 0)
            {
                if (InsertionPoint == InputText.Length)
                {
                    InputText = InputText.Substring(0, InputText.Length - 1);
                    InsertionPoint--;
                }
                else
                {
                    InputText = InputText.Substring(0, InsertionPoint - 1) + InputText.Substring(InsertionPoint);
                    InsertionPoint--;
                }
            }
        }

        private void Delete()
        {
            if (InsertionPoint < InputText.Length - 1)
            {
                InputText = InputText.Substring(0, InsertionPoint) + InputText.Substring(InsertionPoint + 1);
            }
            else if (InsertionPoint == InputText.Length - 1)
            {
                InputText = InputText.Substring(0, InsertionPoint);
            }
        }

        private void InsertKey(Keys keyCode, string keystring)
        {
            string insertString = keystring;

            if (keyCode == Keys.Tab)
            {
                insertString = " ";
            }

            InsertText(insertString);
        }

        private void InsertText(string insertString)
        {
            if (InsertionPoint == InputText.Length)
            {
                InputText += insertString;
            }
            else
            {
                InputText = InputText.Substring(0, InsertionPoint) + insertString + InputText.Substring(InsertionPoint);
            }

            InsertionPoint += insertString.Length;
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
                ProcessKeyDown(Keys.Enter, "\n", KeyModifiers.NoModifierKeyPressed);

                keys = keys.Substring(index + 1);
                index = keys.IndexOf('\n');
            }

            InsertText(keys);
        }

        #endregion

    }
}
