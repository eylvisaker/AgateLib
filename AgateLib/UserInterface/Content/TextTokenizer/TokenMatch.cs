using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Content.TextTokenizer
{
    public class TokenMatch
    {
        public TokenType TokenType { get; set; }
        public string Value { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int Precedence { get; set; }

        public override string ToString() => $"{TokenType}: {Value}";
    }
}
