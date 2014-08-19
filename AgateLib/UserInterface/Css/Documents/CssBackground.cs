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
using AgateLib.Geometry;
using AgateLib.UserInterface.Css.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface.Css.Documents
{
	public class CssBackground : ICssPropertyFromText
	{
		public CssBackground()
		{
			Initialize();
		}

		private void Initialize()
		{
			Color = Color.FromArgb(0, 0, 0, 0);
			Image = null;
			Repeat = 0;
			Position = new CssBackgroundPosition();
			Clip = 0;
		}

		public Color Color { get; set; }
		public string Image { get; set; }
		public CssBackgroundRepeat Repeat { get; set; }
		public CssBackgroundClip Clip { get; set; }
		public CssBackgroundPosition Position { get; set; }

		public void SetValueFromText(string value)
		{
			if (value == "none")
			{
				Initialize();
			}
			else
				throw new NotImplementedException();
		}
	}

}
