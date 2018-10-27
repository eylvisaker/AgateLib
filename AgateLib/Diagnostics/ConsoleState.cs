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

using AgateLib.Diagnostics.ConsoleAppearance;
using System;
using System.Collections.Generic;

namespace AgateLib.Diagnostics
{
    public class ConsoleState
    {
        Stack<string> pathStack = new Stack<string>();

        public ConsoleState()
        {
            pathStack.Push("/");
        }

        public event Action PathChanged;

        public Action Quit { get; set; }

        public IConsoleTheme Theme { get; set; }

        public List<ConsoleMessage> Messages { get; } = new List<ConsoleMessage>();

        public ConsoleDisplayMode DisplayMode { get; set; }

        public int ViewShift { get; set; }

        public bool Debug { get; set; }

        public string InputText { get; set; } = "";

        public int InsertionPoint { get; set; }

        public void WriteLine(string text)
        {
        }

        public string CurrentPath
        {
            get => pathStack.Peek();
            set
            {
                pathStack.Pop();
                pathStack.Push(value);

                PathChanged?.Invoke();
            }
        }

        /// <summary>
        /// Event to validate paths when set. 
        /// Throw an exception to disallow the path.
        /// </summary>
        internal event Func<string, string> PathValidate;

        public void SetCurrentPath(string path)
        {
            if (PathValidate != null)
            {
                path = PathValidate(path);
            }

            CurrentPath = path;
        }
    }
}
