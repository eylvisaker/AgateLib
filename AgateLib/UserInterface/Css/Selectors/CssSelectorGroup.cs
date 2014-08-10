using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Selectors
{
	public class CssSelectorGroup : IEquatable<CssSelectorGroup>
	{
		string mText;
		static readonly char[] comma = new char[] { ',' };

		public CssSelectorGroup() { }
		public CssSelectorGroup(string text)
		{
			Text = text;
		}
		public static implicit operator CssSelectorGroup(string text)
		{
			return new CssSelectorGroup { Text = text };
		}

		public string Text
		{
			get { return mText; }
			set
			{
				mText = value;

				IndividualSelectors = mText
					.Split(comma, StringSplitOptions.RemoveEmptyEntries)
					.Select(x => CreateSelector(x.Trim()));
			}
		}

		private ICssSelector CreateSelector(string text)
		{
			if (text.Contains(" "))
			{
				return new CssSelectorChain(text);
			}
			else
				return new CssSelector(text);
		}

		public IEnumerable<ICssSelector> IndividualSelectors { get; private set; }

		public bool Equals(CssSelectorGroup other)
		{
			return Text.Equals(other.Text, StringComparison.Ordinal);
		}

		public override string ToString()
		{
			return Text;
		}
	}
}
