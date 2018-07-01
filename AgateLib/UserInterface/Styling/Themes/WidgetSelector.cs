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
        IReadOnlyCollection<string> PseudoClasses { get; }

        int Specificity { get; }

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
    public class CssWidgetSelector : IWidgetSelector
    {
        private static Regex pattern = new Regex(
            @"(?<whitespace>\s+)|" +
            @"(?<typeid>[a-zA-Z_][a-zA-Z0-9_\-]*)|" +
            @"(?<class>\.[a-zA-Z_][a-zA-Z0-9_\-]*)|" +
            @"(?<id>#[a-zA-Z_][a-zA-Z0-9_\-]*)|" +
            @"(?<pseudoclass>:[a-zA-Z_][a-zA-Z0-9_\-]*)|" +
            @"(?<child>\>)|" +
            @"(?<wildcard>\*)|" +
            @"(?<invalid>[^\s]+)");

        enum TokenType
        {
            Whitespace,
            TypeId,
            Class,
            Id,
            Pseudoclass,
            Child,
            Wildcard,

            Invalid,
        }

        class Token
        {
            public TokenType Type { get; set; }

            public string Value { get; set; }

            public override string ToString() => $"{Type}: {Value}";
        }

        enum MatcherType
        {
            Self,
            Parent,
            Ancestor
        }

        class Matcher
        {
            public MatcherType MatchType { get; set; }

            public string TypeId { get; set; }

            public List<string> ClassNames { get; set; } = new List<string>();
            public List<string> Identifiers { get; set; } = new List<string>();
            public List<string> PseudoClasses { get; set; } = new List<string>();
        }

        /// <summary>
        /// Stores tokens in REVERSE order.
        /// </summary>
        List<Token> tokens = new List<Token>();
        string selector;

        List<Matcher> matchers = new List<Matcher>();

        public CssWidgetSelector(string selector)
        {
            Selector = selector.Trim();
        }

        public bool IsValid { get; private set; }

        public string Selector
        {
            get => selector;
            set
            {
                selector = value;
                TokenizeSelector(selector);
                BuildMatchers();
                IsValid = Validate();
                CalcSpecificity();
            }
        }

        public IReadOnlyCollection<string> PseudoClasses => matchers.First()?.PseudoClasses;

        public int Specificity { get; private set; }

        private void CalcSpecificity()
        {
            int result = 0;

            foreach(var matcher in matchers)
            {
                if (!string.IsNullOrWhiteSpace(matcher.TypeId) && matcher.TypeId != "*")
                    result += 1;

                result += matcher.ClassNames.Count * 10;
                result += matcher.PseudoClasses.Count * 10;
                result += matcher.Identifiers.Count * 100;
            }

            Specificity = result;
        }

        private void BuildMatchers()
        {
            Matcher matcher = new Matcher();
            bool hasValue = false;

            void PushMatcher()
            {
                matchers.Add(matcher);
                matcher = new Matcher { MatchType = MatcherType.Ancestor };
                hasValue = false;
            }

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Wildcard:
                        matcher.TypeId = "*";
                        hasValue = true;
                        break;

                    case TokenType.TypeId:
                        matcher.TypeId = token.Value;
                        hasValue = true;
                        break;

                    case TokenType.Class:
                        matcher.ClassNames.Add(token.Value.Substring(1).ToLowerInvariant());
                        hasValue = true;
                        break;

                    case TokenType.Id:
                        matcher.Identifiers.Add(token.Value.Substring(1).ToLowerInvariant());
                        hasValue = true;
                        break;

                    case TokenType.Pseudoclass:
                        matcher.PseudoClasses.Add(token.Value.Substring(1).ToLowerInvariant());
                        hasValue = true;
                        break;

                    case TokenType.Whitespace:
                        if (hasValue)
                            PushMatcher();
                        break;

                    case TokenType.Child:
                        if (hasValue)
                            PushMatcher();

                        matcher.MatchType = MatcherType.Parent;
                        break;
                }
            }

            matchers.Add(matcher);
        }

        private bool Validate()
        {
            if (tokens.Any(x => x.Type == TokenType.Invalid))
                return false;

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
            tokens.Clear();

            MatchCollection matches = pattern.Matches(selector);

            foreach (Match match in matches)
            {
                int i = 0;

                foreach (Group group in match.Groups)
                {
                    string matchValue = group.Value;
                    bool success = group.Success;

                    // ignore capture index 0 (general)
                    if (success && i > 0)
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

        public bool Matches(IRenderElement element, RenderElementStack stack)
        {
            if (!IsValid)
                return false;

            // first check if the final token is a potential match to the actual element.
            if (!IsMatch(matchers.First(), element))
                return false;

            int stackPtr = stack.ParentStack.Count - 1;

            for (int i = 1; i < matchers.Count; i++)
            {
                var matcher = matchers[i];

                if (stackPtr < 0)
                    return false;

                while (!IsMatch(matcher, stack.ParentStack[stackPtr]))
                {
                    if (matcher.MatchType == MatcherType.Ancestor)
                    {
                        stackPtr--;

                        if (stackPtr < 0)
                            return false;
                    }
                    else
                        return false;
                }

                stackPtr--;
            }

            return true;
        }

        private bool IsMatch(Matcher matcher, IRenderElement element)
        {
            if (matcher.TypeId == "*")
                return true;

            if (!string.IsNullOrWhiteSpace(matcher.TypeId)
                && !matcher.TypeId.Equals(element.StyleTypeId))
            {
                return false;
            }

            // TODO: Make this work so it can match multiple classes and identifiers on an element.
            foreach(var cls in matcher.ClassNames)
            {
                if(!cls.Equals(element.StyleClass, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            foreach(var cls in matcher.Identifiers)
            {
                if (!cls.Equals(element.StyleId, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
