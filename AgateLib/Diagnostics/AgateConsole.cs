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
using AgateLib.Mathematics.Geometry;
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
        [Obsolete("Use PauseGame instead.")]
        bool IsActive { get; }

        /// <summary>
        /// Gets whether the console window is displayed on the screen.
        /// </summary>
        bool IsOpen { get; }

        Keys ToggleKey { get; set; }

        /// <summary>
        /// Gets whether updates to the game should be paused. This is generally
        /// true when the console window is open and waiting for user input.
        /// </summary>
        /// <remarks>
        /// In some cases, the game engine should get updated while the console window is open.
        /// Be sure to check the CapturingInput property to decide whether your game engine should
        /// process user input.
        /// </remarks>
        bool PauseGame { get; }

        /// <summary>
        /// Gets whether the console is capturing input. This is always true
        /// when the console window is open. If this is true, you should not process
        /// input in your game engine.
        /// </summary>
        bool CapturingInput { get; }

        /// <summary>
        /// If set to true, any messages written to the console will show on screen for a few seconds
        /// even when the console is closed. This is automatically set to true if the console window
        /// is opened by the user.
        /// </summary>
        bool DisplayMessagesWhenClosed { get; set; }

        /// <summary>
        /// Increases the font size for high dpi monitors.
        /// </summary>
        double FontScale { get; set; }

        void Update(GameTime time);

        void Draw(GameTime time);

        void AddCommands(IVocabulary vocabulary);

        /// <summary>
        /// Initializes the console to the specified screen size. If the screen size is larger than 1080p, 
        /// fonts will automatically be scaled up.
        /// </summary>
        /// <param name="screenSize"></param>
        void Initialize(Size screenSize);
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

        public bool IsOpen => State.DisplayMode == ConsoleDisplayMode.Full;

        [Obsolete]
        public bool IsActive => PauseGame;

        public bool PauseGame => CapturingInput && State.PauseGame;

        public bool CapturingInput => State.DisplayMode == ConsoleDisplayMode.Full;

        public bool DisplayMessagesWhenClosed
        {
            get => State.DisplayMode != ConsoleDisplayMode.None;
            set
            {
                if (State.DisplayMode == ConsoleDisplayMode.None)
                    State.DisplayMode = ConsoleDisplayMode.RecentMessagesOnly;
            }
        }

        public double FontScale
        {
            get => renderer.FontScale;
            set => renderer.FontScale = value;
        }

        public void Initialize(Size screenSize)
        {
            renderer.SetScreenSize(screenSize);

            FontScale = Math.Max(1.0, screenSize.Height / 1080.0);
        }

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

            if (!CapturingInput)
            {
                OpenIfToggleKeyPressed();
            }

            if (CapturingInput)
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
            if (CapturingInput)
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

        public void OpenIfToggleKeyPressed()
        {
            if (IsOpen)
                return;

            var keys = Keyboard.GetState();

            if (keys.IsKeyDown(ToggleKey))
            {
                Open();
            }
        }

        public void Open(bool ignoreNextToggleKey = true)
        {
            this.ignoreNextToggleKey = ignoreNextToggleKey;

            State.DisplayMode = ConsoleDisplayMode.Full;
        }

        public void Close()
        {
            if (State.DisplayMode == ConsoleDisplayMode.Full)
            {
                if (DisplayMessagesWhenClosed)
                {
                    State.DisplayMode = ConsoleDisplayMode.RecentMessagesOnly;
                }
                else
                {
                    State.DisplayMode = ConsoleDisplayMode.None;
                }


                ConsoleClosed?.Invoke();
            }
        }
    }
}
