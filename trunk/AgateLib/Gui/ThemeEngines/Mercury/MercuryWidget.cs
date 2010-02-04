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
using AgateLib.Gui.Cache;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	/// <summary>
	/// Base class for classes which draw widgets for the Mercury theme engine.
	/// </summary>
	public abstract class MercuryWidget
	{
		MercuryScheme scheme;

		public MercuryWidget(MercuryScheme scheme)
		{
			this.scheme = scheme;
		}

		public int Margin { get; set; }
		public MercuryScheme Scheme
		{
			get { return scheme; }
		}

		protected internal static void DrawStretchImage(Point loc, Size size,
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

		protected void SetControlFontColor(Widget widget)
		{
			if (widget.Enabled)
				WidgetFont.Color = FontColor;
			else
				WidgetFont.Color = FontColorDisabled;
		}

		protected FontSurface WidgetFont { get { return scheme.WidgetFont; } }
		protected FontSurface TitleFont { get { return scheme.TitleFont; } }

		protected Color FontColor { get { return scheme.FontColor; } }
		protected Color FontColorDisabled { get { return scheme.FontColorDisabled; } }

		protected Size StretchRegionFixedSize(Size imageSize, Rectangle stretchRegion)
		{
			return new Size(
				imageSize.Width - stretchRegion.Width,
				imageSize.Height - stretchRegion.Height);
		}

		protected int InsertionPointHeight { get { return scheme.InsertionPointHeight; } }
		protected int InsertionPointBlinkTime { get { return scheme.InsertionPointBlinkTime; } }

		public virtual Size MinSize(Widget w)
		{
			return new Size(0, 0);
		}
		public virtual Size MaxSize(Widget w)
		{
			return new Size(int.MaxValue / 2, int.MaxValue / 2);
		}
		public abstract void DrawWidget(Widget w);
		public virtual bool HitTest(Widget w, Point clientLocation)
		{
			return true;
		}

		public virtual Rectangle ClientArea(Container widget)
		{
			return new Rectangle(Point.Empty, widget.Size);
		}

		public virtual void MouseDownInWidget(Widget widget, Point clientLocation)
		{
		}
		public virtual void MouseMoveInWidget(Widget widget, Point clientLocation)
		{
		}
		public virtual void MouseUpInWidget(Widget widget, Point clientLocation)
		{
		}

		public virtual void Update(Widget widget)
		{
			DoTransition(widget);
		}

		const float transitionSpeed = 2;
		internal void DoTransition(Widget widget)
		{
			var c = GetOrCreateCache(widget);

			if (Math.Abs(widget.Width - c.DisplayWidth) > 0)
			{
				c.DisplayWidth += UpdateAmount(widget.Width, c.DisplayWidth );
			}
			if (Math.Abs(widget.Height - c.DisplayHeight) > 0)
			{
				c.DisplayHeight += UpdateAmount(widget.Height, c.DisplayHeight);
			}
			if (Math.Abs(widget.Location.X - c.DisplayLocation.X) > 0 || 
				Math.Abs(widget.Location.Y - c.DisplayLocation.Y) > 0)
			{
				c.DisplayLocation = new PointF(
					c.DisplayLocation.X + UpdateAmount(widget.Location.X, c.DisplayLocation.X),
					c.DisplayLocation.Y + UpdateAmount(widget.Location.Y, c.DisplayLocation.Y));
			}
		}

		private static float UpdateAmount(float target, float current)
		{
			return (float)((target - current) * Display.DeltaTime * transitionSpeed / 1000.0);
		}

		public virtual WidgetCache GetOrCreateCache(Widget w)
		{
			var retval = new WidgetCache();

			InitializeCache(w, retval);

			return retval;
		}

		protected void InitializeCache(Widget widget, WidgetCache c)
		{
			c.DisplayLocation = (PointF)widget.Location;
			c.DisplaySize = (SizeF)widget.Size;
		}
	}
}
