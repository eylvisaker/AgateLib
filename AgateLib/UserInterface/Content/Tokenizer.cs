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

using System.Collections.Generic;

namespace AgateLib.UserInterface.Content
{
	public class Tokenizer
	{
		private static readonly TokenizerContext textContext = new TokenizerContext('{');
		private static readonly TokenizerContext commandContext = new TokenizerContext('}') { IncludeBreak = true};

		public List<string> Tokenize(string text)
		{
			List<string> result = new List<string>();

			int start = 0;
			var context = textContext;

			while (start < text.Length) 
			{
				if (text[start] == '{')
					context = commandContext;
				else
				{
					context = textContext;
				}

				var nextTokenStart = NextTokenStart(text, start+1, context);
				
				result.Add(text.Substring(start, nextTokenStart - start));

				start = nextTokenStart;
			}

			return result;
		}

		private int NextTokenStart(string text, int startIndex, TokenizerContext context)
		{
			int min = int.MaxValue;

			foreach (char x in context.TokenBreaks)
			{
				var dist = Find(text, x, startIndex);

				if (min > dist)
					min = dist;
			}

			if (context.IncludeBreak)
				min++;

			return min;
		}

		private int Find(string text, char search, int startIndex)
		{
			var result = text.IndexOf(search, startIndex);

			if (result == -1)
				return text.Length;

			return result;
		}
	}

	public class TokenizerContext
	{
		public TokenizerContext()
		{
		}

		public TokenizerContext(params char[] tokenBreaks)
		{
			TokenBreaks.AddRange(tokenBreaks);
		}

		public List<char> TokenBreaks { get; } = new List<char>();

		/// <summary>
		/// Set to true to include the breaking token as part of the current token.
		/// </summary>
		public bool IncludeBreak { get; set; }
	}
}