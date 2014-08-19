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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Properties
{
	public class CssBorderImage : ICssPropertyFromText
	{
		public CssBorderImage()
		{
			Initialize();
		}

		private void Initialize()
		{
			Source = string.Empty;
			Slice = new CssBorderImageComponent();
			Width = new CssBorderImageComponent();
			Outset = new CssBorderImageComponent();
			Repeat = CssBorderImageRepeat.Initial;
		}

		public string Source { get; set; }
		public CssBorderImageComponent Slice { get; set; }
		public CssBorderImageComponent Width { get; set; }
		public CssBorderImageComponent Outset { get; set; }
		public CssBorderImageRepeat Repeat { get; set; }

		public void SetValueFromText(string value)
		{
			if (value == "none")
			{
				Initialize();
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}

	public class CssBorderImageComponent : CssBoxComponent
	{

	}
}
