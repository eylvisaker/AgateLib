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
using AgateLib.UserInterface.Css.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Properties
{
	public class CssRectangle : ICssPropertyFromText
	{
		public CssDistance Top { get; set; }
		public CssDistance Left { get; set; }
		public CssDistance Right { get; set; }
		public CssDistance Bottom { get; set; }

		public CssDistance Width { get; set; }
		public CssDistance Height { get; set; }

		[CssAlias("min-width")]
		public CssDistance MinWidth { get; set; }
		[CssAlias("min-height")]
		public CssDistance MinHeight { get; set; }

		[CssAlias("max-width")]
		public CssDistance MaxWidth { get; set; }
		[CssAlias("max-height")]
		public CssDistance MaxHeight { get; set; }

		public CssRectangle()
		{
			Top = new CssDistance(true);
			Left = new CssDistance(true);
			Right = new CssDistance(true);
			Bottom = new CssDistance(true);

			Width = new CssDistance(true);
			Height = new CssDistance(true);

			MinWidth = new CssDistance(true);
			MinHeight = new CssDistance(true);

			MaxWidth = new CssDistance(true);
			MaxHeight = new CssDistance(true);
		}

		public void SetValueFromText(string value)
		{
			throw new NotImplementedException();
		}
	}
}
