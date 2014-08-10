using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface.Css.Parser.Lexer
{
	public interface ITokenTypeConverter<T>
	{
		string TokenTypeToString(T tokenType);
		T TokenTypeFromString(string tokenType);
	}
}
