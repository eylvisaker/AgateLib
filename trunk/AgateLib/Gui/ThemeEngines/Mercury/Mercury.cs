using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Gui.ThemeEngines.Mercury.Cache;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	public class Mercury : IGuiThemeEngine
	{
		public Mercury()
			: this(MercuryScheme.CreateDefaultScheme())
		{ }
		public Mercury(MercuryScheme scheme)
		{
			this.Scheme = scheme;
		}

		public MercuryScheme Scheme { get; set; }
		public static bool DebugOutlines { get; set; }

		#region --- Updates ---

		public void Update(GuiRoot guiRoot)
		{
			UpdateCaches(guiRoot);
		}

		private void UpdateCaches(Container container)
		{
			foreach (var widget in container.Children)
			{
				if (widget is Container)
					UpdateCaches((Container)widget);
				else if (widget is TextBox)
					Scheme.TextBox.UpdateCache((TextBox)widget);
			}
		}

		#endregion

		#region --- Interface Dispatchers ---

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

		public Size RequestClientAreaSize(Container widget, Size clientSize)
		{
			throw new NotImplementedException();
		}
		public Size CalcMinSize(Widget widget)
		{
			return Scheme.Themer(widget).MinSize(widget);
		}
		public Size CalcMaxSize(Widget widget)
		{
			return Scheme.Themer(widget).MaxSize(widget);
		}
		public bool HitTest(Widget widget, Point screenLocation)
		{
			Point client = widget.PointToClient(screenLocation);

			return Scheme.Themer(widget).HitTest(widget, client);
		}

		public int ThemeMargin(Widget widget)
		{
			return Scheme.Themer(widget).Margin;
		}

		public void MouseDownInWidget(Widget widget, Point clientLocation)
		{
			Scheme.Themer(widget).MouseDownInWidget(widget, clientLocation);
		}
		public void MouseMoveInWidget(Widget widget, Point clientLocation)
		{
			Scheme.Themer(widget).MouseMoveInWidget(widget, clientLocation);
		}
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


		public Rectangle GetClientArea(Container widget)
		{
			return Scheme.Themer(widget).ClientArea(widget);
		}
	}
}