//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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
