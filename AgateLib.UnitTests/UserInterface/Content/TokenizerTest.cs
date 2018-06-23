using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace AgateLib.UserInterface.Content
{
	public class TokenizerTest
	{
		Tokenizer tokenizer = new Tokenizer();

		[Fact]
		public void TokenizePureText()
		{
			string text = "This is a test";

			var result = tokenizer.Tokenize(text);

			result.Count.Should().Be(1, "There should only be one token.");
            result[0].Should().Be(text);
		}

		[Fact]
		public void TokenizeSingleCommand()
		{
			var result = tokenizer.Tokenize("This is a {Color red}test.");

			result.Count.Should().Be(3, "There should be exactly three tokens.");
			result[0].Should().Be("This is a ");
			result[1].Should().Be("{Color red}");
			result[2].Should().Be("test.");
		}

		[Fact]
		public void TokenizeMultipleCommand()
		{
			var result = tokenizer.Tokenize("This is a {Color red}test{Color white}.");

			result.Count.Should().Be(5, "There should be exactly five tokens.");
			result[0].Should().Be("This is a ");
			result[1].Should().Be("{Color red}");
			result[2].Should().Be("test");
			result[3].Should().Be("{Color white}");
			result[4].Should().Be(".");
		}

		[Fact]
		public void TokenizeCommandAtStart()
		{
			var result = tokenizer.Tokenize("{Color red}Have a sandwich{Color white}!");

			result.Count.Should().Be(4, "There should be exactly four tokens.");
			result[0].Should().Be("{Color red}");
			result[1].Should().Be("Have a sandwich");
			result[2].Should().Be("{Color white}");
			result[3].Should().Be("!");
		}

		[Fact]
		public void TokenizeCommandAtEnd()
		{
			var result = tokenizer.Tokenize("This is {Color red}really bad.{Color white}");

			result.Count.Should().Be(4, "There should be exactly four tokens.");
			result[0].Should().Be("This is ");
			result[1].Should().Be("{Color red}");
			result[2].Should().Be("really bad.");
			result[3].Should().Be("{Color white}");
		}
	}
}
