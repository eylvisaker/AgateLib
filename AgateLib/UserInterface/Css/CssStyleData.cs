﻿//     The contents of this file are subject to the Mozilla Public License
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
using AgateLib.UserInterface.Css.Parser;
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.Css
{
	public class CssStyleData
	{
		public CssStyleData()
		{
			Clear();
		}

		public void Clear()
		{
			PositionData = new CssRectangle();
			Font = new CssFont();
			Background = new CssBackground();
			Margin = new CssBoxComponent();
			Padding = new CssBoxComponent();
			Border = new CssBorder();
			Layout = new CssLayout();
			Transition = new CssTransition();
			Text = new CssText();

			Display = CssDisplay.Initial;
			Overflow = Overflow.Visible;
		}

		public CssDisplay Display { get; set; }
		public Overflow Overflow { get; set; }
		public CssPosition Position { get; set; }

		[CssPromoteProperties]
		public CssRectangle PositionData { get; set; }

		[CssPromoteProperties]
		public CssFont Font { get; set; }

		[CssPromoteProperties(prefix: "background")]
		public CssBackground Background { get; set; }

		[CssPromoteProperties(prefix: "margin")]
		public CssBoxComponent Margin { get; set; }

		[CssPromoteProperties(prefix: "padding")]
		public CssBoxComponent Padding { get; set; }

		[CssPromoteProperties(prefix: "border")]
		public CssBorder Border { get; set; }

		[CssPromoteProperties]
		public CssLayout Layout { get; set; }

		[CssPromoteProperties(prefix: "transition")]
		public CssTransition Transition { get; set; }

		[CssPromoteProperties]
		public CssText Text { get; set; }

	}
}
