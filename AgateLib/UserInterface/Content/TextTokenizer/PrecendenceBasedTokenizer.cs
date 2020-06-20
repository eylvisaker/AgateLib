using System.Collections.Generic;
using System.Linq;

namespace AgateLib.UserInterface.Content.TextTokenizer
{
    /// <summary>
    /// Algorithm for this is borrowed from
    /// https://jack-vanlightly.com/blog/2016/2/24/a-more-efficient-regex-tokenizer
    /// </summary>
    public class PrecendenceBasedTokenizer
    {
        private List<TokenDefinition> tokenDefs;

        public PrecendenceBasedTokenizer()
        {
            tokenDefs = new List<TokenDefinition>
            {
                new TokenDefinition(TokenType.Word,
                @"[\w\.,\!\?'""@#$%^&*\(\)_\+\=\[\]\{\};:\<\>/\\-]+", 1),
                new TokenDefinition(TokenType.NewLine, @"\r?\n", 1),
                new TokenDefinition(TokenType.WhiteSpace, @"[ \t]", 1),
                new TokenDefinition(TokenType.NotDefined, @".+", 1)
            };
        }

        public IEnumerable<TokenMatch> Tokenize(string text)
        {
            var tokenMatches = FindTokenMatches(text);

            var groupedByIndex = tokenMatches.GroupBy(x => x.StartIndex)
                .OrderBy(x => x.Key)
                .ToList();

            TokenMatch lastMatch = null;
            for (int i = 0; i < groupedByIndex.Count; i++)
            {
                var bestMatch = groupedByIndex[i].OrderBy(x => x.Precedence).First();
                if (lastMatch != null && bestMatch.StartIndex < lastMatch.EndIndex)
                {
                    continue;
                }

                yield return bestMatch;

                lastMatch = bestMatch;
            }
        }

        private List<TokenMatch> FindTokenMatches(string errorMessage)
        {
            var tokenMatches = new List<TokenMatch>();

            foreach (var tokenDefinition in tokenDefs)
            {
                tokenMatches.AddRange(tokenDefinition.FindMatches(errorMessage).ToList());
            }

            return tokenMatches;
        }
    }
}
