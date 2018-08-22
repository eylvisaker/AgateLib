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
using AgateLib.UserInterface;

namespace AgateLib.UserInterface.Styling.Themes
{
    public interface IWidgetSelector
    {
        SelectorMatch FindMatch(IRenderElement element, RenderElementStack stack);
    }

    /// <summary>
    /// CSS style element matcher.
    /// </summary>
    /// <remarks>
    /// Currently only supports the following CSS-style selectors:
    /// * Chains: parent child
    /// * Classes: .parent .child
    /// * Identifiers: #parent #child
    /// * First decendent: .parent > .child
    /// * Wild cards: .parent * (matches all ancestors of a parent), or .parent * .child (matches all grandchildren and further descendents of a parent)
    /// * Comma: allows multiple distinct patterns to match a widget.
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
            @"(?<separator>,)|" + 
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
            Separator,

            Invalid,
        }

        class Token
        {
            public TokenType Type { get; set; }

            public string Value { get; set; }

            public override string ToString() => $"{Type}: {Value}";
        }

        /// <summary>
        /// Stores tokens in REVERSE order.
        /// </summary>
        List<Token> tokens = new List<Token>();
        string selector;

        List<Matcher> matchers = new List<Matcher>();

        public CssWidgetSelector(string selector)
        {
            Selector = selector?.Trim() ?? throw new ArgumentNullException("Selector should not be null");
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

                IsValid = ValidateTokens();
            }
        }
        
        private int CalcSpecificity(Matcher matcher)
        {
            int result = 0;

            foreach(var itemMatcher in matcher)
            {
                if (!string.IsNullOrWhiteSpace(itemMatcher.TypeId) && itemMatcher.TypeId != "*")
                    result += 1;

                result += itemMatcher.ClassNames.Count * 10;
                result += itemMatcher.PseudoClasses.Count * 10;
                result += itemMatcher.Identifiers.Count * 100;
            }

            return result;
        }

        private void BuildMatchers()
        {
            Matcher matcher = new Matcher();

            ItemMatcher itemMatcher = new ItemMatcher();
            bool hasValue = false;

            void PushItemMatcher()
            {
                if (!hasValue)
                    return;

                matcher.Add(itemMatcher);
                itemMatcher = new ItemMatcher { MatchType = MatcherType.Ancestor };
                hasValue = false;
            }

            void PushMatcher()
            {
                if (hasValue)
                    PushItemMatcher();

                matchers.Add(matcher);
                matcher = new Matcher();
            }
            

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Wildcard:
                        itemMatcher.TypeId = "*";
                        hasValue = true;
                        break;

                    case TokenType.TypeId:
                        itemMatcher.TypeId = token.Value;
                        hasValue = true;
                        break;

                    case TokenType.Class:
                        itemMatcher.ClassNames.Add(token.Value.Substring(1).ToLowerInvariant());
                        hasValue = true;
                        break;

                    case TokenType.Id:
                        itemMatcher.Identifiers.Add(token.Value.Substring(1).ToLowerInvariant());
                        hasValue = true;
                        break;

                    case TokenType.Pseudoclass:
                        itemMatcher.PseudoClasses.Add(token.Value.Substring(1).ToLowerInvariant());
                        hasValue = true;
                        break;

                    case TokenType.Whitespace:
                        PushItemMatcher();
                        break;

                    case TokenType.Separator:
                        PushMatcher();
                        break;

                    case TokenType.Child:
                        PushItemMatcher();
                        itemMatcher.MatchType = MatcherType.Parent;
                        break;
                }
            }

            PushMatcher();
        }

        private bool ValidateTokens()
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
                    if (tokens.Count > 1)
                        return false;
                    break;

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

        public SelectorMatch FindMatch(IRenderElement element, RenderElementStack stack)
        {
            foreach(var matcher in matchers)
            {
                if (Matches(matcher, element, stack))
                {
                    return new SelectorMatch(matcher, CalcSpecificity(matcher), matcher.First().PseudoClasses);
                }
            }

            return null;
        }

        private bool Matches(Matcher matcher, IRenderElement element, RenderElementStack stack)
        {
            if (!IsValid)
                return false;

            // first check if the final token is a potential match to the actual element.
            if (!IsMatch(matcher.First(), element))
                return false;

            int stackPtr = stack.ParentStack.Count - 1;

            for (int i = 1; i < matcher.Count; i++)
            {
                var itemMatcher = matcher[i];

                if (stackPtr < 0)
                    return false;

                while (!IsMatch(itemMatcher, stack.ParentStack[stackPtr]))
                {
                    if (itemMatcher.MatchType == MatcherType.Ancestor)
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

        private bool IsMatch(ItemMatcher matcher, IRenderElement element)
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
                if (!cls.Equals(element.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString() => Selector;
    }

    public class SelectorMatch
    {
        private readonly Matcher matcher;

        public SelectorMatch(Matcher matcher, int specificity, IEnumerable<string> pseudoClasses)
        {
            this.matcher = matcher;
            Specificity = specificity;
            PseudoClasses = pseudoClasses.ToList();
        }

        public int Specificity { get; }

        public IReadOnlyCollection<string> PseudoClasses { get; }

        public override string ToString() => matcher.ToString();
    }


    public class Matcher : List<ItemMatcher>
    {
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            foreach(var item in this)
            {
                switch(item.MatchType)
                {
                    case MatcherType.Self:
                    case MatcherType.Ancestor:
                        result.Append(item.ToString());
                        result.Append(" ");
                        break;

                    case MatcherType.Parent:
                        result.Append(" > ");
                        result.Append(item.ToString());
                        break;
                }
            }

            return result.ToString();
        }
    }

    public class ItemMatcher
    {
        public MatcherType MatchType { get; set; }

        public string TypeId { get; set; }

        public List<string> ClassNames { get; set; } = new List<string>();
        public List<string> Identifiers { get; set; } = new List<string>();
        public List<string> PseudoClasses { get; set; } = new List<string>();

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append(TypeId);

            if (ClassNames.Count > 0)
            {
                result.Append(".");
                result.Append(string.Join(".", ClassNames));
            }

            if (Identifiers.Count > 0)
            {
                result.Append("#");
                result.Append(string.Join("#", Identifiers));
            }

            if (PseudoClasses.Count > 0)
            {
                result.Append(":");
                result.Append(string.Join(":", PseudoClasses));
            }

            return result.ToString();
        }
    }

    public enum MatcherType
    {
        Self,
        Parent,
        Ancestor
    }

}
