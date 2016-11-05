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
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.Css.Documents
{
	public class CssTransition : ICssPropertyFromText, ITransitionStyle
	{
		public CssTransition()
		{
			Clear();
		}

		public void SetValueFromText(string value)
		{
			var values = value.Split(' ');

			foreach(var v in values)
			{
				WindowTransitionType type;
				TransitionDirection dir;
				double time;

				if (double.TryParse(v, out time))
					Time = time;
				else if (Enum.TryParse(v, true, out type))
					Type = type;
				else if (Enum.TryParse(v, true, out dir))
					Direction = dir;
			}
		}

		public void Clear()
		{
			Type = WindowTransitionType.None;
			Time = 0.5;
		}

		public WindowTransitionType Type { get; set; }
		public TransitionDirection Direction { get; set; }
		public double Time { get; set; }
	}
}
