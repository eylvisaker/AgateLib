using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Parser.Lexer
{

	public class Token<T>
	{
		public readonly T TokenType;
		public readonly string Value;
		public Token(T type, string value)
		{
			TokenType = type;
			Value = value;
		}
	}

}
