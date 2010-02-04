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
	/// Class which draws listboxes for the Mercury theme engine.
	/// </summary>
	public class MercuryListBox : MercuryWidget
	{
		public Surface Image { get; set; }
		public Surface Disabled { get; set; }
		public Surface Hover { get; set; }
		public Surface Focus { get; set; }
		public Rectangle StretchRegion { get; set; }

		public MercuryListBox(MercuryScheme scheme)
			:base(scheme)
		{
			Margin = 3;
		}

		public override void DrawWidget(Widget w)
		{
			DrawListBox((ListBox)w);
		}

		private void DrawListBox(ListBox listBox)
		{
			Surface image = Image;

			if (listBox.Enabled == false)
				image = Disabled;

			Point location = listBox.PointToScreen(new Point(0, 0));
			Size size = listBox.Size;

			DrawStretchImage(location, size, image, StretchRegion);
		}


	}

}
