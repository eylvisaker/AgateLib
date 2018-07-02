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

namespace AgateLib.UserInterface.Widgets
{
    public class AnimationStyle
    {
        static char[] splitter = new char[] { ' ' };

        string _in, _out, _static;

        string inName, outName, staticName;
        IReadOnlyList<string> inArgs, outArgs, staticArgs;

        public string In
        {
            get => _in;
            set
            {
                _in = value;
                GetAnimationNameAndArgs(value, ref inName, ref inArgs);
            }
        }

        public string Out
        {
            get => _out;
            set
            {
                _out = value;
                GetAnimationNameAndArgs(Out, ref outName, ref outArgs);
            }
        }

        public string Static
        {
            get => _static;
            set
            {
                _static = value;
                GetAnimationNameAndArgs(Static, ref staticName, ref staticArgs);
            }
        }

        internal string InName => inName;
        internal IReadOnlyList<string> InArgs => inArgs;
        internal string OutName => outName;
        internal IReadOnlyList<string> OutArgs => outArgs;

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
