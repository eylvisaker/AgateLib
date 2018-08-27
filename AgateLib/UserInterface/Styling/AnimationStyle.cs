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
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface
{
    public class AnimationStyle
    {
        public static bool Equals(AnimationStyle a, AnimationStyle b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;

            if (a.Entry != b.Entry) return false;
            if (a.Exit != b.Exit) return false;
            if (a.Static != b.Static) return false;

            return true;
        }

        static char[] splitter = new char[] { ' ' };

        string _entry, _exit, _static;

        string entryName, exitName, staticName;
        IReadOnlyList<string> entryArgs, exitArgs, staticArgs;

        /// <summary>
        /// Animator to use when element is first added to the UI.
        /// </summary>
        public string Entry
        {
            get => _entry;
            set
            {
                _entry = value;
                GetAnimationNameAndArgs(value, ref entryName, ref entryArgs);
            }
        }

        /// <summary>
        /// Animator to use when element is to be removed from the UI.
        /// </summary>
        public string Exit
        {
            get => _exit;
            set
            {
                _exit = value;
                GetAnimationNameAndArgs(Exit, ref exitName, ref exitArgs);
            }
        }

        /// <summary>
        /// Animator to use when element is part of the UI.
        /// </summary>
        public string Static
        {
            get => _static;
            set
            {
                _static = value;
                GetAnimationNameAndArgs(Static, ref staticName, ref staticArgs);
            }
        }

        internal string EntryName => entryName;
        internal IReadOnlyList<string> EntryArgs => entryArgs;
        internal string ExitName => exitName;
        internal IReadOnlyList<string> ExitArgs => exitArgs;

        internal string StaticName => staticName;
        internal IReadOnlyList<string> StaticArgs => staticArgs;

        private void GetAnimationNameAndArgs(string data, ref string name, ref IReadOnlyList<string> args)
        {
            var values = data.Split(splitter, StringSplitOptions.RemoveEmptyEntries);

            name = values[0];
            args = values.Skip(1).ToList();
        }
    }
}
