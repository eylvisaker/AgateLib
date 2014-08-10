using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Parser.Lexer
{
	public class Tokenizer<T>
	{
		StringBuilder mPattern = new StringBuilder();
		List<T> ignoreTypes = new List<T>();
		Regex regex;
		ITokenTypeConverter<T> tokenTypeConverter;

		public Tokenizer()
		{
			if (typeof(T).IsEnum)
				tokenTypeConverter = new EnumParser<T>();
		}
		public Tokenizer(ITokenTypeConverter<T> converter)
		{
			tokenTypeConverter = converter;
		}

		public void Add(T tokenType, string pattern, bool ignore = false)
		{
			if (mPattern.Length > 0)
				mPattern.Append("|");

			mPattern.Append("(?<");
			mPattern.Append(tokenTypeConverter.TokenTypeToString(tokenType));
			mPattern.Append(">");
			mPattern.Append(pattern);
			mPattern.Append(")");

			regex = null;

			if (ignore)
				ignoreTypes.Add(tokenType);
		}

		public List<Token<T>> Tokenize(string text)
		{
			if (regex == null)
				regex = new Regex(mPattern.ToString());

			MatchCollection matches = regex.Matches(text);
			List<Token<T>> tokenList = new List<Token<T>>();

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
						string groupName = regex.GroupNameFromNumber(i);
						T tokenType = tokenTypeConverter.TokenTypeFromString(groupName);

						if (ignoreTypes.Contains(tokenType) == false)
						{
							tokenList.Add(new Token<T>(tokenType, matchValue));
							break;
						}
					}
					i++;
				}
			}

			return tokenList;
		}
	}

}
