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
using AgateLib.Geometry;

namespace AgateLib.Gui
{
	/// <summary>
	/// Interface which must be implemented by any theme engine which draws the widgets.
	/// </summary>
	public interface IGuiThemeEngine
	{
		/// <summary>
		/// Draws the specified widget to the screen.
		/// </summary>
		/// <param name="widget"></param>
		void DrawWidget(Widget widget);

		/// <summary>
		/// Calculates and returns the minimum size for the widget.
		/// </summary>
		/// <param name="widget"></param>
		/// <returns></returns>
		Size CalcMinSize(Widget widget);

		/// <summary>
		/// Calculates and returns the maximum size for the widget.
		/// </summary>
		/// <param name="widget"></param>
		/// <returns></returns>
		Size CalcMaxSize(Widget widget);

		/// <summary>
		/// Gets the minimum margin area around the widget required by the theme.
		/// </summary>
		/// <param name="widget"></param>
		/// <returns></returns>
		int ThemeMargin(Widget widget);

		/// <summary>
		/// Returns the area for the client space in 
		/// the widget, given its size.
		/// </summary>
		/// <param name="widget"></param>
		/// <returns></returns>
		Rectangle GetClientArea(Container widget);
		/// <summary>
		/// Returns the size the container widget should be to 
		/// have the specified client area.
		/// </summary>
		/// <param name="widget"></param>
		/// <param name="clientSize"></param>
		/// <returns></returns>
		Size RequestClientAreaSize(Container widget, Size clientSize);

		/// <summary>
		/// Returns true if the point specified in inside the area of the widget where a mouse
		/// click would count as hitting the control.
		/// </summary>
		/// <param name="widget"></param>
		/// <param name="screenLocation"></param>
		/// <returns></returns>
		bool HitTest(Widget widget, Point screenLocation);

		/// <summary>
		/// Updates the GUI.
		/// </summary>
		/// <param name="guiRoot"></param>
		void Update(GuiRoot guiRoot);

		/// <summary>
		/// Function called when a mouse button is pressed inside a widget.
		/// </summary>
		/// <param name="widget"></param>
		/// <param name="clientLocation"></param>
		void MouseDownInWidget(Widget widget, Point clientLocation);
		/// <summary>
		/// Function called when the mouse is moved inside a widget.
		/// </summary>
		/// <param name="widget"></param>
		/// <param name="clientLocation"></param>
		void MouseMoveInWidget(Widget widget, Point clientLocation);
		/// <summary>
		/// Function called when the mouse button is released inside a widget.
		/// </summary>
		/// <param name="widget"></param>
		/// <param name="clientLocation"></param>
		void MouseUpInWidget(Widget widget, Point clientLocation);
		/// <summary>
		/// Called when a widget is resized or needs some other graphical and
		/// probably transitionary update.
		/// </summary>
		/// <param name="widget"></param>
		void WidgetNeedsUpdate(Widget widget);

		WidgetRenderer CreateRenderer(Widget widget);
	}
}
