using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Selectors
{
	class CssSelectorChain : ICssSelector
	{
		List<ICssSelector> mSelectors = new List<ICssSelector>();

		public CssSelectorChain(string text)
		{
			Text = text;

			mSelectors.AddRange(text
				.Split(Extensions.WhiteSpace, StringSplitOptions.RemoveEmptyEntries)
				.Select(x => new CssSelector(x)));
		}

		public string Text { get; private set; }

		public IEnumerable<ICssSelector> Selectors { get { return mSelectors; } }

		public bool Matches(Widget control, string id, CssPseudoClass pc, IEnumerable<string> classes)
		{
			//for (int i = mSelectors.Count - 1; i >= 0; i--)
			//{
			//	var sel = mSelectors[i];

			//	if (sel.Matches(control, id, classes) == false)
			//}
			return false;
			throw new NotImplementedException();
		}
	}
}
