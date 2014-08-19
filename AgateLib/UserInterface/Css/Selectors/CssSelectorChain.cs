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

		public bool Matches(string typename, string id, CssPseudoClass pc, IEnumerable<string> classes)
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
