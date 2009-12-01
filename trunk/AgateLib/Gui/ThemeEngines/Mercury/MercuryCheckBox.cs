using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	public class MercuryCheckBox : MercuryWidget
	{
		public Surface Image { get; set; }
		public Surface Disabled { get; set; }
		public Surface Check { get; set; }
		public Surface Hover { get; set; }
		public Surface Focus { get; set; }
		public int Spacing { get; set; }
		
		public MercuryCheckBox(MercuryScheme scheme)
			: base(scheme)
		{
			Spacing = 5;
			Margin = 2;
		}

		public override void DrawWidget(Widget w)
		{
			if (w is CheckBox)
				DrawCheckbox((CheckBox)w);
			else
				DrawRadioButton((RadioButton)w);
		}
		public void DrawCheckbox(CheckBox checkbox)
		{
			Surface surf;

			if (checkbox.Enabled == false)
				surf = Disabled;
			else
				surf = Image;

			Point destPoint = checkbox.PointToScreen(
				Origin.Calc(OriginAlignment.CenterLeft, checkbox.Size));

			surf.DisplayAlignment = OriginAlignment.CenterLeft;
			surf.Draw(destPoint);

			if (checkbox.Enabled)
			{
				if (checkbox.HasFocus)
				{
					Focus.DisplayAlignment = OriginAlignment.CenterLeft;
					Focus.Draw(destPoint);
				}
				if (checkbox.MouseIn)
				{
					Hover.DisplayAlignment = OriginAlignment.CenterLeft;
					Hover.Draw(destPoint);
				}
				if (checkbox.Checked)
				{
					Check.Color = Color.White;
					Check.DisplayAlignment = OriginAlignment.CenterLeft;
					Check.Draw(destPoint);
				}
			}
			else if (checkbox.Checked)
			{
				Check.Color = Color.Gray;
				Check.DisplayAlignment = OriginAlignment.CenterLeft;
				Check.Draw(destPoint);
			}

			SetControlFontColor(checkbox);

			destPoint.X += surf.DisplayWidth + Spacing;

			WidgetFont.DisplayAlignment = OriginAlignment.CenterLeft;
			WidgetFont.DrawText(destPoint, checkbox.Text);
		}
		public void DrawRadioButton(RadioButton radiobutton)
		{
			Surface surf;

			if (radiobutton.Enabled == false)
				surf = Disabled;
			else
				surf = Image;

			Point destPoint = radiobutton.PointToScreen(
				Origin.Calc(OriginAlignment.CenterLeft, radiobutton.Size));

			surf.DisplayAlignment = OriginAlignment.CenterLeft;
			surf.Draw(destPoint);

			if (radiobutton.Enabled)
			{
				if (radiobutton.HasFocus)
				{
					Focus.DisplayAlignment = OriginAlignment.CenterLeft;
					Focus.Draw(destPoint);
				}
				if (radiobutton.MouseIn)
				{
					Hover.DisplayAlignment = OriginAlignment.CenterLeft;
					Hover.Draw(destPoint);
				}
				if (radiobutton.Checked)
				{
					Check.Color = Color.White;
					Check.DisplayAlignment = OriginAlignment.CenterLeft;
					Check.Draw(destPoint);
				}
			}
			else if (radiobutton.Checked)
			{
				Check.Color = FontColorDisabled;
				Check.DisplayAlignment = OriginAlignment.CenterLeft;
				Check.Draw(destPoint);
			}

			SetControlFontColor(radiobutton);

			destPoint.X += surf.DisplayWidth + Spacing;

			WidgetFont.DisplayAlignment = OriginAlignment.CenterLeft;
			WidgetFont.DrawText(destPoint, radiobutton.Text);
		}

		public override Size MinSize(Widget w)
		{
			Size text = WidgetFont.MeasureString(w.Text);
			Size box = Image.SurfaceSize;

			return new Size(
				box.Width + Spacing + text.Width,
				Math.Max(box.Height, text.Height));
		}

		public override bool HitTest(Widget w, Point screenLocation)
		{
			Point local = w.PointToClient(screenLocation);

			int right = Image.SurfaceWidth +
					WidgetFont.MeasureString(w.Text).Width + Spacing * 2;

			if (local.X > right)
				return false;

			return true;
		}

	}
}
