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
using AgateLib.UserInterface.Css.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Documents
{
	public class CssBoxComponent : ICssPropertyFromText, ICssBoxComponent
	{
		public CssDistance Top { get; set; }
		public CssDistance Bottom { get; set; }
		public CssDistance Left { get; set; }
		public CssDistance Right { get; set; }

		public CssBoxComponent()
		{
			Top = new CssDistance(false);
			Bottom = new CssDistance(false);
			Right = new CssDistance(false);
			Left = new CssDistance(false);
		}

		static char[] sep = new char[] { '\t', '\n', '\r', ' ' };
		static int[,] indices = new int[4, 4] { { 0, 0, 0, 0 }, {0, 1, 0, 1}, {0, 1, 2, 1}, {0, 1, 2, 3} };
		
		public void SetValueFromText(string value)
		{
			string[] values = value.Split(sep, StringSplitOptions.RemoveEmptyEntries);

			Top = CssDistance.FromString(values[indices[values.Length-1, 0]]);
			Right = CssDistance.FromString(values[indices[values.Length-1, 1]]);
			Bottom = CssDistance.FromString(values[indices[values.Length-1, 2]]);
			Left = CssDistance.FromString(values[indices[values.Length-1, 3]]);
		}
	}
}
