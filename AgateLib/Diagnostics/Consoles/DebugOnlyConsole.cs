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

using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace AgateLib.Diagnostics.Consoles
{
    [Obsolete("This should be moved to a test framework library.")]
    public class DebugOnlyConsole : IConsoleSetup
    {
        public bool IsOpen => true;

        public bool IsActive => true;

        public bool PauseGame => true;

        public bool CapturingInput => true;

        public Keys ToggleKey { get; set; } = Keys.OemTilde;

        public bool DisplayMessagesWhenClosed { get; set; }

        public double FontScale { get; set; } = 1.0;

        public void AddCommands(IVocabulary boxVocabulary)
        {
            throw new NotImplementedException();
        }

        public void Draw(GameTime time)
        {
        }

        public void Initialize(Size screenSize)
        { }

        public void Update(GameTime time)
        {
        }

        public void WriteLine(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
            System.Console.WriteLine(text);
        }
    }
}
