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
	public class CssLayout : ICssPropertyFromText
	{
		[CssAlias("layout-kind")]
		public CssLayoutKind Kind { get; set; }

		[CssAlias("layout-grid-columns")]
		public int GridColumns { get; set; }

		public void SetValueFromText(string value)
		{
			string[] values = value.Split(' ');

			foreach(var v in values)
			{
				CssLayoutKind result;
				int columns;

				if (Enum.TryParse<CssLayoutKind>(v, true, out result))
				{
					Kind = result;
				}
				else if (int.TryParse(v, out columns))
				{
					GridColumns = columns;
				}
			}
		}
	}
}
