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

namespace AgateLib.UserInterface.Layout
{
	public interface ILayoutBuilder
	{
		/// <summary>
		/// Calculates the box size of the widget, given the passed constraints.
		/// The box size includes with margin, padding and border.
		/// </summary>
		/// <param name="widget">The widget who's box size is to be computed</param>
		/// <param name="maxWidth">The maximum width of the widget's box</param>
		/// <param name="maxHeight">The maximum height of the widget's box</param>
		/// <returns></returns>
		void ComputeBoxSize(WidgetStyle widget, int? maxWidth = null, int? maxHeight = null);

		/// <summary>
		/// Calculates the size of the widget in the absence of any constraints.
		/// </summary>
		/// <param name="widget"></param>
		bool ComputeNaturalSize(WidgetStyle widget);

		/// <summary>
		/// Returns the style object of the specified widget.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		WidgetStyle StyleOf(Widget widget);
	}
}
