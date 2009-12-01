using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	public class MercuryButton : MercuryWidget
	{
		public Rectangle StretchRegion { get; set; }
		public Surface Image { get; set; }
		public Surface Default { get; set; }
		public Surface Pressed { get; set; }
		public Surface Disabled { get; set; }
		public Surface Hover { get; set; }
		public Surface Focus { get; set; }
		public int TextPadding { get; set; }

		public MercuryButton(MercuryScheme scheme)
			: base(scheme)
		{ }

		public override void DrawWidget(Widget w)
		{
			DrawButton((Button)w);
		}
		public void DrawButton(Button button)
		{
			Surface image = Image;

			bool isDefault = button.IsDefaultButton;

			if (button.Enabled == false)
				image = Disabled;
			else if (button.DrawActivated)
				image = Pressed;
			else if (isDefault)
				image = Default;

			Point location = button.PointToScreen(Point.Empty);
			Size size = new Size(button.Width, button.Height);

			DrawStretchImage(location, size,
				image, StretchRegion);

			if (button.Enabled)
			{
				if (button.MouseIn)
				{
					DrawStretchImage(location, size, Hover, StretchRegion);
				}
				if (button.HasFocus)
				{
					DrawStretchImage(location, size,
						Focus, StretchRegion);
				}
			}


			// Draw button text
			SetControlFontColor(button);

			WidgetFont.DisplayAlignment = OriginAlignment.Center;
			location = Origin.Calc(OriginAlignment.Center, button.Size);

			// drop the text down a bit if the button is being pushed.
			if (button.DrawActivated)
			{
				location.X++;
				location.Y++;
			}

			WidgetFont.DrawText(
				button.PointToScreen(location),
				button.Text);
		}

		public override Size MinSize(Widget w)
		{
			return CalcMinButtonSize((Button)w);
		}
		public Size CalcMinButtonSize(Button button)
		{
			Size textSize = WidgetFont.MeasureString(button.Text);
			Size buttonBorder = new Size(
				Image.SurfaceWidth - StretchRegion.Width,
				Image.SurfaceHeight - StretchRegion.Height);

			textSize.Width += TextPadding * 2;
			textSize.Height += TextPadding * 2;

			return new Size(
				textSize.Width + buttonBorder.Width,
				textSize.Height + buttonBorder.Height);
		}

		public override bool HitTest(Widget w, Point screenLocation)
		{
			return true;
		}
	}
}
