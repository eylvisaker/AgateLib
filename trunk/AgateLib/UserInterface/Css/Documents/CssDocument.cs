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
using AgateLib.UserInterface.Css.Binders;
using AgateLib.UserInterface.Css.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Documents
{
	public class CssDocument
	{
		List<CssMediaSelector> mMedia = new List<CssMediaSelector>();

		public CssDocument()
		{
			var defaultMedium = new CssMediaSelector("all");
			Media.Add(defaultMedium);
		}
		public static CssDocument Load(string filename)
		{
			CssDocument doc = new CssDocument();
			CssParser parser = new CssParser();

			parser.Load(doc, filename);

			return doc;
		}
		public static CssDocument FromText(string text)
		{
			CssDocument doc = new CssDocument();
			CssParser parser = new CssParser();

			parser.ParseCss(doc, text);

			return doc;
		}


		public List<CssMediaSelector> Media { get { return mMedia; } }

		public CssMediaSelector DefaultMedium { get { return mMedia.First(); } }
	}
}
