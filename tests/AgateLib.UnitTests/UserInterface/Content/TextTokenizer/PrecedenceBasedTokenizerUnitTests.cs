using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AgateLib.UserInterface.Content.TextTokenizer
{
    public class PrecedenceBasedTokenizerUnitTests
    {
        [Fact]
        public void TokenizeTextWithDash()
        {
            var tokenizer = new PrecendenceBasedTokenizer();

            List<TokenMatch> tokens = tokenizer.Tokenize("Weapon - Knife").ToList();

            tokens[0].TokenType.Should().Be(TokenType.Word);
            tokens[0].Value.Should().Be("Weapon");

            tokens[1].TokenType.Should().Be(TokenType.WhiteSpace);
            tokens[1].Value.Should().Be(" ");

            tokens[2].TokenType.Should().Be(TokenType.Word);
            tokens[2].Value.Should().Be("-");

            tokens[3].TokenType.Should().Be(TokenType.WhiteSpace);
            tokens[3].Value.Should().Be(" ");

            tokens[4].TokenType.Should().Be(TokenType.Word);
            tokens[4].Value.Should().Be("Knife");
        }
    }
}
