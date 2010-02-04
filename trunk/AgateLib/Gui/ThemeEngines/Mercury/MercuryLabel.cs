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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	/// <summary>
	/// Class which draws labels for the Mercury theme engine.
	/// </summary>
	public class MercuryLabel : MercuryWidget 
	{
		/// <summary>
		/// Constructs a MercuryLabel object.
		/// </summary>
		/// <param name="scheme"></param>
		public MercuryLabel(MercuryScheme scheme)
			: base(scheme)
		{ }

		/// <summary>
		/// Draws the label.
		/// </summary>
		/// <param name="w"></param>
		public override void DrawWidget(Widget w)
		{
			DrawLabel((Label)w);
		}
		/// <summary>
		/// Draws a label.
		/// </summary>
		/// <param name="label"></param>
		public void DrawLabel(Label label)
		{
			Point location = new Point();

			location = DisplayLib.Origin.Calc(label.TextAlignment, label.Size);
			location.X += label.ScreenLocation.X;
			location.Y += label.ScreenLocation.Y;

			SetControlFontColor(label);

			WidgetFont.DisplayAlignment = label.TextAlignment;
			WidgetFont.DrawText(location, label.Text);
		}

		/// <summary>
		/// Gets minimum size of a label.
		/// </summary>
		/// <param name="w"></param>
		/// <returns></returns>
		public override Size MinSize(Widget w)
		{
			return CalcMinLabelSize((Label)w);
		}
		/// <summary>
		/// Gets the minimum size of a label.
		/// </summary>
		/// <param name="label"></param>
		/// <returns></returns>
		public Size CalcMinLabelSize(Label label)
		{
			Size retval = WidgetFont.MeasureString(label.Text);

			return retval;
		}

	}
}
