﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Layout
{
	public interface ILayoutBuilder
	{
		/// <summary>
		/// Calculates the box size of the widget, given the passed constraints.
		/// The box size includes with margin, padding and border.
		/// </summary>
		/// <param name="child"></param>
		/// <param name="maxWidth"></param>
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