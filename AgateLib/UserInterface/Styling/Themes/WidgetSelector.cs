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
using System.Text.RegularExpressions;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Styling.Themes
{
    public interface IWidgetSelector
    {
        [Obsolete]
        bool Matches(IRenderElement widget, IWidgetStackState state);
        bool Matches(IRenderElement element, RenderElementStack stack);
    }

    /// <summary>
    /// CSS style element matcher.
    /// </summary>
    /// <remarks>
    /// Currently only supports the following CSS selectors:
    /// * Chains: parent child
    /// * Classes: .parent .child
    /// * Identifiers: #parent #child
    /// * First decendent: .parent > .child
    /// * Wild cards: .parent * (matches all ancestors of a parent), or .parent * .child (matches all grandchildren and further descendents of a parent)
    /// </remarks>
    public class CssWidgetSelector : WidgetSelector
    {
        private static Regex pattern = new Regex(
            @"(?<whitespace>\s+)|" +
            @"(?<id>#[a-zA-Z_][a-zA-Z0-9_]*)|" +
            @"(?<class>\.[a-zA-Z_][a-zA-Z0-9_]*)|" +
            @"(?<typeid>[a-zA-Z_][a-zA-Z0-9_]*)|" +
            @"(?<child>\>)|" +
            @"(?<wildcard>\*)|" +
            @"(?<invalid>[^\s]+)");

        enum TokenType
        {
            Whitespace,

            Id,
            Class,
            TypeId,
            Child,
            Wildcard,

            Invalid,
        }

        class Token
        {
            public TokenType Type { get; set; }
            public string Value { get; set; }
        }

        /// <summary>
        /// Stores tokens in REVERSE order.
        /// </summary>
        List<Token> tokens = new List<Token>();

        public CssWidgetSelector(string selector)
        {
            TokenizeSelector(selector);

            IsValid = Validate();
        }

        public bool IsValid { get; }

        private bool Validate()
        {
            switch (tokens.First().Type)
            {
                case TokenType.Child:
                    return false;
            }

            switch (tokens.Last().Type)
            {
                case TokenType.Wildcard:
                case TokenType.Child:
                    return false;
            }

            bool foundChild = false;
            for (int i = 1; i < tokens.Count; i++)
            {
                if (tokens[i].Type == TokenType.Child)
                {
                    if (foundChild)
                        return false;

                    foundChild = true;
                }
                else
                    foundChild = false;
            }

            return true;
        }

        private void TokenizeSelector(string selector)
        {
            MatchCollection matches = pattern.Matches(selector);

            foreach (Match match in matches)
            {
                int i = 0;

                foreach (Group group in match.Groups)
                {
                    string matchValue = group.Value;
                    bool success = group.Success;
                    // ignore capture index 0 and 1 (general and WhiteSpace)
                    if (success && i > 1)
                    {
                        string groupName = pattern.GroupNameFromNumber(i);
                        tokens.Add(new Token
                        {
                            Type = (TokenType)Enum.Parse(typeof(TokenType), groupName, true),
                            Value = matchValue
                        });
                    }
                    i++;
                }
            }

            tokens.Reverse();
        }

        public override bool Matches(IRenderElement widget, IWidgetStackState state)
        {
            return false;
        }

        public override bool Matches(IRenderElement element, RenderElementStack stack)
        {
            if (!IsValid)
                return false;

            // first check if the final token is a potential match to the actual element.
            if (!IsMatch(tokens.First(), element))
                return false;

            int stackPtr = stack.ParentStack.Count - 1;

            for (int i = 1; i < tokens.Count; i++)
            {
                bool foundMatch = false;

                switch (tokens[i].Type)
                {
                    case TokenType.Child:
                        i++;

                        if (stackPtr < 0)
                            return false;

                        if (!IsMatch(tokens[i], stack.ParentStack[stackPtr]))
                            return false;

                        stackPtr--;
                        break;

                    case TokenType.Id:
                    case TokenType.TypeId:
                    case TokenType.Class:
                        while (stackPtr >= 0)
                        {
                            stackPtr--;

                            if (IsMatch(tokens[i], stack.ParentStack[stackPtr+1]))
                            {
                                foundMatch = true;
                                break;
                            }
                        }

                        if (!foundMatch)
                            return false;

                        break;

                    case TokenType.Wildcard:
                        if (stackPtr < 0)
                            return false;

                        stackPtr--;
                        break;
                }
            }

            return true;
        }

        private bool IsMatch(Token token, IRenderElement element)
        {
            if (token.Type == TokenType.Wildcard)
                return true;

            if (token.Type == TokenType.Class)
            {
                if (token.Value.Substring(1).Equals(element.StyleClass, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            else if (token.Type == TokenType.Id)
            {
                if (token.Value.Substring(1).Equals(element.StyleId, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            else if (token.Type == TokenType.TypeId)
            {
                if (token.Value.Equals(element.StyleTypeIdentifier, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }

    public abstract class WidgetSelector : IWidgetSelector
    {
        public bool IsType(IRenderElement widget, string typeIdentifier)
        {
            if (widget == null)
                return typeIdentifier == null;

            return widget.StyleTypeIdentifier.Equals(typeIdentifier, StringComparison.OrdinalIgnoreCase);
        }

        public abstract bool Matches(IRenderElement widget, IWidgetStackState state);

        public abstract bool Matches(IRenderElement element, RenderElementStack stack);
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

        public override bool Matches(IRenderElement widget, IWidgetStackState state)
        {
            var id = BuildStateIdentifier(widget, state.Parent);

            return Matches(id);
        }

        public bool Matches(string stateIdentifier)
        {
            bool result = regex.IsMatch(stateIdentifier);

            return result;
        }

        private string BuildStateIdentifier(IRenderElement widget, IRenderElement parent)
        {
            if (parent == null)
                return widget.StyleTypeIdentifier;

            return parent.StyleTypeIdentifier + "." + widget.StyleTypeIdentifier;
        }

        public override bool Matches(IRenderElement element, RenderElementStack stack)
        {
            var id = BuildStateIdentifier(element, stack.ParentStack.LastOrDefault());

            return Matches(id);
        }
    }
}
