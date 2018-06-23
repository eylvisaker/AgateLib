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
using System.Text;
using System.Text.RegularExpressions;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Styling.Themes
{
    public interface IWidgetSelector
    {
        bool Matches(IWidget widget, IWidgetStackState state);
    }

    public class ParentWidgetSelector : WidgetSelector
    {
        private readonly string parentType;

        public ParentWidgetSelector(string parentType)
        {
            this.parentType = parentType;
        }

        public override bool Matches(IWidget widget, IWidgetStackState state)
        {
            return IsType(state.Parent, parentType);
        }
    }

    public abstract class WidgetSelector : IWidgetSelector
    {
        public bool IsType(IWidget widget, string typeIdentifier)
        {
            if (widget == null)
                return typeIdentifier == null;

            return widget.StyleTypeIdentifier.Equals(typeIdentifier, StringComparison.OrdinalIgnoreCase);
        }

        public abstract bool Matches(IWidget widget, IWidgetStackState state);
    }

    public class PatternWidgetSelector : WidgetSelector
    {
        public static string TransformPattern(string pattern)
        {
            StringBuilder result = new StringBuilder();

            if (pattern == "*")
                return ".*";

            for (int i = 0; i < pattern.Length; i++)
            {
                if (i + 2 <= pattern.Length && pattern.Substring(i, 2) == "..")
                {
                    // matches any parents, so it should have a surround dots.
                    result.Append(@"(\..*\.|\.)");
                    i++;
                }
                else if (pattern[i] == '*')
                {
                    result.Append(".*");
                }
                else if (pattern[i] == '.')
                {
                    result.Append(@"\.");
                }
                else
                {
                    result.Append(pattern[i]);
                }
            }

            return $@"^({result}|.*\.{result})$";
        }

        private Regex regex;

        public PatternWidgetSelector(string pattern)
        {
            this.regex = new Regex(TransformPattern(pattern), RegexOptions.IgnoreCase);
        }

        public override string ToString() => regex.ToString();

        public override bool Matches(IWidget widget, IWidgetStackState state)
        {
            var id = BuildStateIdentifier(widget, state);

            return Matches(id);
        }

        public bool Matches(string stateIdentifier)
        {
            bool result = regex.IsMatch(stateIdentifier);

            return result;
        }

        private string BuildStateIdentifier(IWidget widget, IWidgetStackState state)
        {
            if (state.Parent == null)
                return widget.StyleTypeIdentifier;

            return state.Parent.StyleTypeIdentifier + "." + widget.StyleTypeIdentifier;
        }
    }
}
