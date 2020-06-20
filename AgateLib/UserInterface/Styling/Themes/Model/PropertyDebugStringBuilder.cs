﻿//
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

using System.Text;

namespace AgateLib.UserInterface.Styling.Themes.Model
{
    internal class PropertyDebugStringBuilder
    {
        private StringBuilder builder = new StringBuilder();

        public string Delimiter { get; set; } = "\n";

        public void Add(string text)
        {
            builder.Append(text);
            builder.Append(Delimiter);
        }

        public void Add(string name, object value)
        {
            if (value == null)
            {
                return;
            }

            var valueString = value.ToString();

            if (string.IsNullOrEmpty(valueString))
            {
                return;
            }

            builder.Append($"{name}: {value}");
            builder.Append(Delimiter);
        }

        public override string ToString()
        {
            return builder.ToString();
        }
    }
}
