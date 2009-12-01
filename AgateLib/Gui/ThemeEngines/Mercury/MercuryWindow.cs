using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	public class MercuryWindow : MercuryWidget
	{
		public Surface NoTitle { get; set; }
		public Surface WithTitle { get; set; }
		public Surface TitleBar { get; set; }
		public Rectangle NoTitleStretchRegion { get; set; }
		public Rectangle WithTitleStretchRegion { get; set; }
		public Rectangle TitleBarStretchRegion { get; set; }

		public bool CenterTitle { get; set; }

		public Surface CloseButton { get; set; }
		public Surface CloseButtonHover { get; set; }
		public Surface CloseButtonInactive { get; set; }

		public int DropShadowSize { get; set; }

		public MercuryWindow(MercuryScheme scheme) : base(scheme)
		{
			CenterTitle = true;
		}

		public override void DrawWidget(Widget w)
		{
			DrawWindow((Window)w);
		}
		public void DrawWindow(Window window)
		{
			DrawWindowBackground(window);
			DrawWindowTitle(window);
			DrawWindowDecorations(window);
		}

		// TODO: fix this
		public int WindowTitlebarSize
		{
			get { return TitleFont.FontHeight + 6; }
		}

		protected virtual void DrawWindowBackground(Window window)
		{
			if (window.ShowTitleBar)
			{
				DrawStretchImage(window.Parent.PointToScreen(
					new Point(window.Location.X, window.Location.Y + this.WindowTitlebarSize)),
					window.Size, WithTitle, WithTitleStretchRegion);

			}
			else
				throw new NotImplementedException();
		}

		private void DrawDropShadow(Rectangle rect)
		{
			for (int i = 0; i <= DropShadowSize; i++)
			{
				Color fadeColor = Color.Red;// Scheme.WindowBorderColor;
				fadeColor.A = (byte)(
					fadeColor.A * (DropShadowSize - i) / (2 * DropShadowSize));

				Display.DrawRect(rect, fadeColor);

				rect.X--;
				rect.Y--;
				rect.Width += 2;
				rect.Height += 2;
			}
		}
		protected virtual void DrawWindowTitle(Window window)
		{
			Point windowLocation = window.ScreenLocation;

			DrawStretchImage(windowLocation,
				new Size(window.Width, WindowTitlebarSize), TitleBar, TitleBarStretchRegion);

			Point fontPosition = new Point(windowLocation.X + 8, windowLocation.Y + 3);
			if (CenterTitle)
			{
				fontPosition.X = windowLocation.X + window.Width / 2;
				fontPosition.Y = windowLocation.Y + WindowTitlebarSize / 2;
				TitleFont.DisplayAlignment = OriginAlignment.Center;
			}

			TitleFont.Color = FontColor;

			TitleFont.DrawText(
				fontPosition,
				window.Text);

			TitleFont.DisplayAlignment = OriginAlignment.TopLeft;
		}
		protected virtual void DrawWindowDecorations(Window window)
		{
			CloseButton.DisplayAlignment = OriginAlignment.TopRight;
			CloseButton.Draw(
				new Point(window.ScreenLocation.X + window.Size.Width,
					window.ScreenLocation.Y));
		}

		public override Size MinSize(Widget w)
		{
			return new Size(10, 10);
		}

		public override Rectangle ClientArea(Container widget)
		{
			Window window = (Window)widget;

			if (window.ShowTitleBar)
			{
				return new Rectangle(
					WithTitleStretchRegion.Left,
					WithTitleStretchRegion.Top + WindowTitlebarSize,
					widget.Width - (WithTitle.SurfaceWidth - WithTitleStretchRegion.Width),
					widget.Height - (WithTitle.SurfaceHeight - WithTitleStretchRegion.Height));
			}
			else
			{
				throw new NotImplementedException();
			}
		}

	}
}
