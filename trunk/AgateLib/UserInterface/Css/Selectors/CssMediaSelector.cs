using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface.Css.Selectors
{
	public class CssMediaSelector : IEquatable<CssMediaSelector>
	{
		public static implicit operator CssMediaSelector(string text)
		{
			return new CssMediaSelector { Text = text };
		}

		string mText;
		static readonly char[] comma = new char[] { ',' };

		public CssMediaSelector()
		{
			RuleBlocks = new List<CssRuleBlock>();
		}
		public CssMediaSelector(string text)
			:this()
		{
			Text = text;
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

		private ICssMediaSelector CreateSelector(string text)
		{
			if (text.Contains(" ") || text.Contains(">"))
			{
				throw new NotImplementedException();
			}
			else
				return new CssMediaIndividualSelector(text);
		}

		public IEnumerable<ICssMediaSelector> IndividualSelectors { get; private set; }

		public bool Equals(CssMediaSelector other)
		{
			return Text.Equals(other.Text, StringComparison.Ordinal);
		}
		public override bool Equals(object obj)
		{
			if (obj is CssMediaSelector)
				return Equals((CssMediaSelector)obj);

			return false;
		}
		public override int GetHashCode()
		{
			return Text.GetHashCode();
		}

		public override string ToString()
		{
			return Text;
		}

		public List<CssRuleBlock> RuleBlocks { get; private set; }
	}
}
