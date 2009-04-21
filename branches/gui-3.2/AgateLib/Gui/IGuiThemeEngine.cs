using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.Gui
{
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

		void Update(GuiRoot guiRoot);
	}
}
