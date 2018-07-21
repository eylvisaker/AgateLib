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
using AgateLib.Diagnostics.CommandLibraries;
using AgateLib.Diagnostics.Rendering;
using AgateLib.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AgateLib.Diagnostics
{
    public interface IConsole
    {
        void WriteLine(string text);
    }

    public interface IConsoleSetup : IConsole
    {
        bool IsActive { get; }

        void Update(GameTime time);

        void Draw(GameTime time);

        void AddVocabulary(IVocabulary vocabulary);
    }

    [Singleton]
    public class AgateConsole : IConsoleSetup
    {
        private readonly IConsoleRenderer renderer;
        private readonly ConsoleShell shell = new ConsoleShell();
        private readonly AgateLib.Input.KeyboardInput keyboardInput 
            = new AgateLib.Input.KeyboardInput();

        private bool suppressToggleKey;

        public AgateConsole(IConsoleRenderer renderer)
        {
            this.renderer = renderer;

            renderer.State = shell.State;

            keyboardInput.KeyPress += KeyboardInput_KeyPress;
        }

        public ConsoleState State => shell.State;

        public Keys ToggleKey { get; set; } = Keys.OemTilde;

        public bool IsActive { get; set; }

        /// <summary>
        /// Adds a vocabulary to the console.
        /// Each method that can be called from a console command should be decorated with a ConsoleCommandAttribute
        /// </summary>
        /// <param name="vocabulary">The IVocabulary object which has methods
        /// decorated with the ConsoleCommandAttribute.</param>
        public void AddVocabulary(IVocabulary vocab)
        {
            shell.CommandLibraries.Add(new VocabularyCommands(vocab));
        }

        public void Draw(GameTime time)
        {
            renderer.Draw(time);
        }

        public void Update(GameTime time)
        {
            CheckToggleKey();

            shell.Update(time);

            if (IsActive)
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
            if (IsActive)
            {
                if (e.Key == ToggleKey && suppressToggleKey)
                    return;

                shell.ProcessKeyDown(e.Key, e.KeyString, e.Modifiers);
            }
        }

        private void CheckToggleKey()
        {
            // We avoid using the keyboardInput events here to avoid generating garbage when the 
            // console window is closed.
            var keyState = Keyboard.GetState();
            bool toggleKeyPressed = keyState.IsKeyDown(ToggleKey);
            bool escapeKeyPressed = keyState.IsKeyDown(Keys.Escape);

            if (toggleKeyPressed && !suppressToggleKey)
            {
                suppressToggleKey = true;

                IsActive = !IsActive;
            }
            else if (!toggleKeyPressed && suppressToggleKey)
            {
                suppressToggleKey = false;
            }

            if (escapeKeyPressed && IsActive)
            {
                IsActive = false;
            }

            if (IsActive)
            {
                State.DisplayMode = ConsoleDisplayMode.Full;
            }
            else
            {
                State.DisplayMode = ConsoleDisplayMode.RecentMessagesOnly;
            }
        }
    }
}
