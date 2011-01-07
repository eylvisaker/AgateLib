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
using System.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	/// <summary>
	/// The first theme engine written for the AgateLib GUI.
	/// Mercury is used as the name since the planet Mercury is the
	/// first planet in the solar system.
	/// </summary>
	public class Mercury : IGuiThemeEngine
	{
		/// <summary>
		/// Constructs a Mercury object.
		/// </summary>
		public Mercury()
			: this(MercuryScheme.CreateDefaultScheme())
		{
			
		}
		/// <summary>
		/// Constructs a Mercury object.
		/// </summary>
		/// <param name="scheme"></param>
		public Mercury(MercuryScheme scheme)
		{
			this.Scheme = scheme;

			mRendererMap.Add(typeof(TextBox), typeof(MercuryTextBox));
			mRendererMap.Add(typeof(CheckBox), typeof(MercuryCheckBox));
			mRendererMap.Add(typeof(RadioButton), typeof(MercuryCheckBox));
			mRendererMap.Add(typeof(Button), typeof(MercuryButton));
			mRendererMap.Add(typeof(GuiRoot), typeof(MercuryGuiRoot));
			mRendererMap.Add(typeof(Label), typeof(MercuryLabel));
			mRendererMap.Add(typeof(Panel), typeof(MercuryPanel));
			mRendererMap.Add(typeof(Window), typeof(MercuryWindow));


		}


		/// <summary>
		/// Gets or sets the parameters used to draw widgets.
		/// </summary>
		public MercuryScheme Scheme { get; set; }
		/// <summary>
		/// Gets or sets whether or not controls should be drawn with outlines around them.
		/// Mainly for debugging purposes.
		/// </summary>
		public static bool DebugOutlines { get; set; }

		#region --- Updates ---

		/// <summary>
		/// Updates the widgets.
		/// </summary>
		/// <param name="guiRoot"></param>
		public void Update(GuiRoot guiRoot)
		{
			Scheme.ExecuteUpdates();
		}

		public void WidgetNeedsUpdate(Widget w)
		{
			Scheme.WidgetNeedsUpdate(w);
		}
		#endregion

		Dictionary<Type, Type> mRendererMap = new Dictionary<Type, Type>();

		public WidgetRenderer CreateRenderer(Widget widget)
		{
			return (WidgetRenderer)Activator.CreateInstance(mRendererMap[widget.GetType()], this.Scheme, widget);
		}
		#region --- Interface Dispatchers ---

		/// <summary>
		/// Draws a widget.
		/// </summary>
		/// <param name="widget"></param>
		public void DrawWidget(Widget widget)
		{
			if (widget is GuiRoot)
				return;

			if (DebugOutlines)
			{
				Display.DrawRect(new Rectangle(widget.ScreenLocation, widget.Size),
					Color.Red);
			}

			Scheme.Themer(widget).DrawWidget(widget);

		}
		/// <summary>
		/// Gets the size of the widget given the specified client size.
		/// </summary>
		/// <param name="widget"></param>
		/// <param name="clientSize"></param>
		/// <returns></returns>
		public Size RequestClientAreaSize(Container widget, Size clientSize)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Calculates the minimum size of the widget.
		/// </summary>
		/// <param name="widget"></param>
		/// <returns></returns>
		public Size CalcMinSize(Widget widget)
		{
			return Scheme.Themer(widget).MinSize(widget);
		}
		/// <summary>
		/// Calculates the practical maximum size of the widget.
		/// </summary>
		/// <param name="widget"></param>
		/// <returns></returns>
		public Size CalcMaxSize(Widget widget)
		{
			return Scheme.Themer(widget).MaxSize(widget);
		}
		/// <summary>
		/// Checks to see whether the point is inside the specified widget.
		/// </summary>
		/// <param name="widget"></param>
		/// <param name="screenLocation"></param>
		/// <returns></returns>
		public bool HitTest(Widget widget, Point screenLocation)
		{
			Point client = widget.PointToClient(screenLocation);

			return Scheme.Themer(widget).HitTest(widget, client);
		}
		/// <summary>
		/// Gets the margin that should be placed around the specified widget.
		/// </summary>
		/// <param name="widget"></param>
		/// <returns></returns>
		public int ThemeMargin(Widget widget)
		{
			return Scheme.Themer(widget).Margin;
		}
		/// <summary>
		/// Function called when a mouse button is pressed in the widget.
		/// </summary>
		/// <param name="widget"></param>
		/// <param name="clientLocation"></param>
		public void MouseDownInWidget(Widget widget, Point clientLocation)
		{
			Scheme.Themer(widget).MouseDownInWidget(widget, clientLocation);
		}
		/// <summary>
		/// Function called when the mouse is moved in a widget.
		/// </summary>
		/// <param name="widget"></param>
		/// <param name="clientLocation"></param>
		public void MouseMoveInWidget(Widget widget, Point clientLocation)
		{
			Scheme.Themer(widget).MouseMoveInWidget(widget, clientLocation);
		}
		/// <summary>
		/// Function called when the mouse button is released in a widget.
		/// </summary>
		/// <param name="widget"></param>
		/// <param name="clientLocation"></param>
		public void MouseUpInWidget(Widget widget, Point clientLocation)
		{
			Scheme.Themer(widget).MouseUpInWidget(widget, clientLocation);
		}

		#endregion


		[Obsolete]
		private Size StretchRegionFixedSize(Size imageSize, Rectangle stretchRegion)
		{
			return new Size(
				imageSize.Width - stretchRegion.Width,
				imageSize.Height - stretchRegion.Height);
		}

		private void DrawStretchImage(Point loc, Size size,
			Surface surface, Rectangle stretchRegion)
		{
			Rectangle scaled = new Rectangle(
				loc.X + stretchRegion.X,
				loc.Y + stretchRegion.Y,
				size.Width - (surface.SurfaceWidth - stretchRegion.Right) - stretchRegion.X,
				size.Height - (surface.SurfaceHeight - stretchRegion.Bottom) - stretchRegion.Y);

			// draw top left
			surface.Draw(
				new Rectangle(0, 0, stretchRegion.Left, stretchRegion.Top),
				new Rectangle(loc.X, loc.Y, stretchRegion.Left, stretchRegion.Top));

			// draw top middle
			surface.Draw(
				new Rectangle(stretchRegion.Left, 0, stretchRegion.Width, stretchRegion.Top),
				new Rectangle(loc.X + stretchRegion.Left, loc.Y,
					scaled.Width, stretchRegion.Top));

			// draw top right
			surface.Draw(
				new Rectangle(stretchRegion.Right, 0, surface.SurfaceWidth - stretchRegion.Right, stretchRegion.Top),
				new Rectangle(scaled.Right, loc.Y, surface.SurfaceWidth - stretchRegion.Right, stretchRegion.Top));

			// draw middle left
			surface.Draw(
				new Rectangle(0, stretchRegion.Top, stretchRegion.Left, stretchRegion.Height),
				new Rectangle(loc.X, loc.Y + stretchRegion.Top, stretchRegion.Left, scaled.Height));

			// draw middle
			surface.Draw(
				stretchRegion,
				scaled);

			// draw middle right
			surface.Draw(
				new Rectangle(stretchRegion.Right, stretchRegion.Top, surface.SurfaceWidth - stretchRegion.Right, stretchRegion.Height),
				new Rectangle(scaled.Right, scaled.Top, surface.SurfaceWidth - stretchRegion.Right, scaled.Height));

			// draw bottom left
			surface.Draw(
				new Rectangle(0, stretchRegion.Bottom, stretchRegion.Left, surface.SurfaceHeight - stretchRegion.Bottom),
				new Rectangle(loc.X, scaled.Bottom, stretchRegion.Left, surface.SurfaceHeight - stretchRegion.Bottom));

			// draw bottom middle
			surface.Draw(
				new Rectangle(stretchRegion.Left, stretchRegion.Bottom, stretchRegion.Width, surface.SurfaceHeight - stretchRegion.Bottom),
				new Rectangle(scaled.Left, scaled.Bottom, scaled.Width, surface.SurfaceHeight - stretchRegion.Bottom));

			// draw bottom right
			surface.Draw(
				new Rectangle(stretchRegion.Right, stretchRegion.Bottom, surface.SurfaceWidth - stretchRegion.Right, surface.SurfaceHeight - stretchRegion.Bottom),
				new Rectangle(scaled.Right, scaled.Bottom, surface.SurfaceWidth - stretchRegion.Right, surface.SurfaceHeight - stretchRegion.Bottom));

		}

		/// <summary>
		/// Gets the client area of the widget.
		/// </summary>
		/// <param name="widget"></param>
		/// <returns></returns>
		public Rectangle GetClientArea(Container widget)
		{
			return Scheme.Themer(widget).ClientArea(widget);
		}



	}
}