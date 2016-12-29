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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.UserInterface.StyleModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Rendering
{
	public interface IWidgetAnimator
	{
		IList<IWidgetAnimator> Children { get; }
		Rectangle ClientRect { get; set; }
		Gesture Gesture { get; set; }
		bool InTransition { get; }
		bool IsDead { get; set; }
		IWidgetAnimator Parent { get; set; }
		WidgetStyle Style { get; }
		bool Visible { get; set; }
		Widget Widget { get; }
		Rectangle WidgetRect { get; }
		IWidgetAnimator ParentCoordinateSystem { get; }

		void Update(double deltaTime);
		Rectangle ClientToScreen(Rectangle rectangle, bool translateForScroll = true);
		Point ClientToScreen(Point translated, bool translateForScroll = true);
		Point ScreenToClient(Point translated);

		int X { get; set; }
		int Y { get; set; }
		int Width { get; set; }
		int Height { get; set; }
	}
}
