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
using AgateLib.Diagnostics.Rendering;
using AgateLib.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace AgateLib.Diagnostics
{
    public interface IConsole
    {
        void WriteLine(string text);
    }

    public interface IConsoleSetup : IConsole
    {
        bool IsOpen { get; }

        void Update(GameTime time);

        void Draw(GameTime time);

        void AddCommands(IVocabulary vocabulary);
    }

    [Singleton]
    public class AgateConsole : IConsoleSetup
    {
        private readonly IConsoleRenderer renderer;
        private readonly ConsoleShell shell = new ConsoleShell();
        private readonly KeyboardEvents keyboardInput = new KeyboardEvents();

        private bool suppressToggleKey = true;
        private bool ignoreNextToggleKey;

        public AgateConsole(IConsoleRenderer renderer)
        {
            this.renderer = renderer;

            renderer.State = shell.State;

            keyboardInput.KeyPress += KeyboardInput_KeyPress;
            keyboardInput.KeyUp += KeyboardInput_KeyUp;
        }

        public event Action ConsoleClosed;

        public ConsoleState State => shell.State;

        public Keys ToggleKey { get; set; } = Keys.OemTilde;

        public bool IsOpen { get; private set; }

        /// <summary>
        /// Adds a vocabulary to the console.
        /// Each method that can be called from a console command should be decorated with a ConsoleCommandAttribute
        /// </summary>
        /// <param name="vocabulary">The IVocabulary object which has methods
        /// decorated with the ConsoleCommandAttribute.</param>
        public void AddCommands(IVocabulary vocab)
        {
            AddCommands(new VocabularyCommands(vocab));
        }

        /// <summary>
        /// Adds a command library to the console.
        /// An ICommandLibrary object provides full flexibility for the processing of user entered commands,
        /// but is more complex to implement than an IVocabulary object.
        /// </summary>
        /// <param name="commands"></param>
        public void AddCommands(ICommandLibrary commands)
        {
            shell.AddCommands(commands);
        }

        public void Draw(GameTime time)
        {
            renderer.Draw(time);
        }

        public void Update(GameTime time)
        {
            shell.Update(time);

            if (IsOpen)
            {
                keyboardInput.Update(time);
            }

            renderer.Update(time);
        }


        public void WriteLine(string text)
        {
            shell.WriteLine(text);
        }

        private void KeyboardInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (IsOpen)
            {
                if (e.Key == ToggleKey && suppressToggleKey)
                    return;

                shell.ProcessKeyDown(e.Key, e.KeyString, e.Modifiers);
            }
        }

        private void KeyboardInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == ToggleKey || e.Key == Keys.Escape)
            {
                if (ignoreNextToggleKey)
                {
                    ignoreNextToggleKey = false;
                }
                else
                {
                    Close();
                }
            }
        }

        public void Open(bool ignoreNextToggleKey = true)
        {
            this.ignoreNextToggleKey = ignoreNextToggleKey;
            IsOpen = true;
            State.DisplayMode = ConsoleDisplayMode.Full;
        }

        public void Close()
        {
            if (IsOpen)
            {
                IsOpen = false;
                State.DisplayMode = ConsoleDisplayMode.RecentMessagesOnly;

                ConsoleClosed?.Invoke();
            }
        }
    }
}
