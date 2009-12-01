using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	public class MercuryLabel : MercuryWidget 
	{
		public MercuryLabel(MercuryScheme scheme)
			: base(scheme)
		{ }

		public override void DrawWidget(Widget w)
		{
			DrawLabel((Label)w);
		}
		public void DrawLabel(Label label)
		{
			Point location = new Point();

			location = DisplayLib.Origin.Calc(label.TextAlignment, label.Size);
			location.X += label.ScreenLocation.X;
			location.Y += label.ScreenLocation.Y;

			SetControlFontColor(label);

			WidgetFont.DisplayAlignment = label.TextAlignment;
			WidgetFont.DrawText(location, label.Text);
		}

		public override Size MinSize(Widget w)
		{
			return CalcMinLabelSize((Label)w);
		}
		public Size CalcMinLabelSize(Label label)
		{
			Size retval = WidgetFont.MeasureString(label.Text);

			return retval;
		}

	}
}
