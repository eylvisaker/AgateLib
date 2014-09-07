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
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.UserInterface.Widgets
{
	public class ProgressBar : Widget
	{
		public int Value { get; set; }
		public int Max { get; set; }
		public Gradient Gradient { get; set; }

		public ProgressBar()
		{
			Gradient = new AgateLib.Geometry.Gradient(Color.White);
		}
		internal override Size ComputeSize(int? minWidth, int? minHeight, int? maxWidth, int? maxHeight)
		{
			Size retval = new Size();

			retval.Width = 40;

			return retval;
		}

		public override void DrawImpl()
		{
			Rectangle destRect = ClientToScreen(
				new Rectangle(0, 0, Width, Height));

			if (Max > 0)
			{
				double percentage = Value / (double)Max;

				int maxBarWidth = Width;
				int width = (int)(percentage * maxBarWidth);

				destRect.Width = width;

				var grad = new Gradient(Gradient.TopLeft);
				grad.TopRight = Gradient.Interpolate(width, 0);
				grad.BottomRight = grad.TopRight;

				Display.FillRect(destRect, grad);
			}
		}
	}
}
