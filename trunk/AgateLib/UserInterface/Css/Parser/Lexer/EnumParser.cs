using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Parser.Lexer
{
	public class EnumParser<T> : ITokenTypeConverter<T>
	{
		public string TokenTypeToString(T tokenType)
		{
			return tokenType.ToString();
		}

		public T TokenTypeFromString(string value)
		{
			return (T)Enum.Parse(typeof(T), value, true);
		}
	}
}
