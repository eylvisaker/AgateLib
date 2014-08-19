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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface.Css.Properties
{
	public class CssBackgroundPosition : ICssPropertyFromText
	{
		public CssBackgroundPosition()
		{
			Left = new CssDistance();
			Top = new CssDistance();
		}

		public CssDistance Left;
		public CssDistance Top;

		public void SetValueFromText(string value)
		{
			int index = value.IndexOf(' ');

			if (index >= 0)
			{
				Left = CssDistance.FromString(value.Substring(0, index));
				Top = CssDistance.FromString(value.Substring(index + 1));
			}
			else
			{
				Left = CssDistance.FromString(value);
				Top = Left;
			}
		}
	}
}
