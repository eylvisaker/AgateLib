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
using AgateLib.Diagnostics;
using AgateLib.Geometry;
using AgateLib.UserInterface.Css.Binders;
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Css.Parser.Lexer;
using AgateLib.UserInterface.Css.Selectors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css
{
	public class CssParser
	{
		CssDocument target;
		CssPropertyMap propertyMap = new CssPropertyMap();

		List<Token<CssTokenType>> tokenList;
		int index;

		CssMediaSelector defaultMedium;
		CssMediaSelector currentMedium;
		CssRuleBlock currentBlock;

		Token<CssTokenType> currentToken;
		bool exitedBlock = false;

		CssBindingMapper mBindingMapper;

		public CssParser()
		{
			mBindingMapper = new CssBindingMapper(propertyMap);
		}
		public void Load(CssDocument doc, string filename)
		{
			string css = AgateLib.IO.FileProvider.UserInterfaceAssets.ReadAllText(filename);

			ParseCss(doc, css);
		}

		public void ParseCss(CssDocument doc, string css)
		{
			target = doc;

			defaultMedium = doc.DefaultMedium;

			currentMedium = defaultMedium;

			ParseCss(css);
		}
		private void ParseCss(string css)
		{
			tokenList = TokenizeCss(css);

			index = 0;

			ParseContents();
		}

		private Token<CssTokenType> ReadNextToken()
		{
			if (index >= tokenList.Count)
				return null;

			exitedBlock = false;
			currentToken = tokenList[index];
			index++;

			// skip comments
			if (currentToken.TokenType == CssTokenType.CommentOpen)
			{
				while (tokenList[index].TokenType != CssTokenType.CommentClose
					&& index < tokenList.Count)
				{
					index++;
				}

				return ReadNextToken();
			}

			return currentToken;
		}

		private void ParseContents()
		{
			while (index < tokenList.Count)
			{
				ReadNextToken();

				if (currentToken.TokenType == CssTokenType.Identifier)
				{
					if (currentToken.Value.StartsWith("@"))
					{
						ParseAtRule(currentToken.Value);
					}
					else if (currentBlock == null)
					{
						ParseBlock(currentToken.Value);
					}
					else
					{
						ParseProperty(currentToken.Value);
					}
				}

				if (currentToken.TokenType == CssTokenType.BlockClose && exitedBlock == false)
				{
					exitedBlock = true;
					return;
				}

			}
		}

		private void ParseProperty(string property)
		{
			var name = ReadPropertyName(property);
			var value = ReadPropertyValue();

			if (mBindingMapper.FindPropertyChain(property))
			{
				currentBlock.AddProperty(name, value);
			}
			else
			{
				Log.WriteLine(string.Format("Unrecognized property {0} with value {1}.", name, value));
			}
		}

		private void ParseBlock(string token)
		{
			CssSelector selector = ReadSelector(token);

			currentBlock = currentMedium.RuleBlocks.FindExactMatch(selector);

			if (currentBlock == null)
			{
				currentBlock = new CssRuleBlock(propertyMap) { Selector = selector };
				currentMedium.RuleBlocks.Add(currentBlock);
			}

			ParseContents();

			currentBlock = null;
		}

		private void ParseAtRule(string rule)
		{
			if (rule.StartsWith("@media"))
			{
				CssMediaSelector selector = ReadMediaSelector();

				currentMedium = target.Media.FindExactMatch(selector);

				if (currentMedium == null)
				{
					currentMedium = selector;
					target.Media.Add(currentMedium);
				}

				ParseContents();

				currentMedium = defaultMedium;
			}
		}

		private CssSelector ReadSelector(string startText = "")
		{
			return new CssSelector(ReadUntilToken(startText, CssTokenType.BlockOpen));

		}
		private CssMediaSelector ReadMediaSelector(string startText = "")
		{
			return new CssMediaSelector(ReadUntilToken(startText, CssTokenType.BlockOpen));

		}

		private string ReadPropertyName(string startText = "")
		{
			return ReadUntilToken(startText, CssTokenType.Colon);
		}
		private string ReadPropertyValue(string startText = "")
		{
			return ReadUntilToken(startText, CssTokenType.SemiColon, CssTokenType.BlockClose);
		}

		private string ReadUntilToken(string startText, params CssTokenType[] endTypes)
		{
			StringBuilder b = new StringBuilder();

			b.Append(startText);

			var token = ReadNextToken();

			while (token != null && endTypes.Contains(token.TokenType) == false)
			{
				if (token.TokenType == CssTokenType.Whitespace)
					b.Append(" ");
				else
					b.Append(token.Value);

				token = ReadNextToken();
			}

			return b.ToString().Trim();
		}


		public static List<Token<CssTokenType>> TokenizeCss(string css)
		{
			var tokenizer = new Tokenizer<CssTokenType>();

			tokenizer.Add(CssTokenType.Whitespace, @"\s+");
			tokenizer.Add(CssTokenType.Identifier, @"@?[a-zA-Z0-9\.\#\-_%/\\]+");
			tokenizer.Add(CssTokenType.BlockOpen, @"{");
			tokenizer.Add(CssTokenType.BlockClose, @"}");
			tokenizer.Add(CssTokenType.SemiColon, @";");
			tokenizer.Add(CssTokenType.Colon, @":");
			tokenizer.Add(CssTokenType.CommentOpen, @"/\*");
			tokenizer.Add(CssTokenType.CommentClose, @"\*/");
			tokenizer.Add(CssTokenType.Punctuation, @"[\(\)]+");
			tokenizer.Add(CssTokenType.Comma, ",");
			tokenizer.Add(CssTokenType.Invalid, @"[^\s]+", ignore: true);

			return tokenizer.Tokenize(css);
		}
	}



	public enum CssTokenType
	{
		Whitespace,
		Identifier,
		BlockOpen,
		BlockClose,
		SemiColon,
		Colon,
		CommentOpen,
		CommentClose,

		Invalid,
		Punctuation,
		Comma,
	}
}
