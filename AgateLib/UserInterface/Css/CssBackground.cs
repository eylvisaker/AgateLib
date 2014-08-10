using AgateLib.Geometry;
using AgateLib.UserInterface.Css.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface.Css
{
	public class CssBackground
	{
		public CssBackground()
		{
			Color = Color.FromArgb(0, 0, 0, 0);
			Position = new CssBackgroundPosition();
		}

		public Color Color { get; set; }
		public string Image { get; set; }
		public CssBackgroundRepeat Repeat { get; set; }
		public CssBackgroundClip Clip { get; set; }
		public CssBackgroundPosition Position { get; set; }
	}

	public class CssBackgroundPosition : ICssPropertyFromText
	{
		public CssBackgroundPosition()
		{
			Left = new CssDistance();
			Top = new CssDistance();
		}

		public CssDistance Left;
		public CssDistance Top;

		public void SetValueFromText(string value)
		{
			int index = value.IndexOf(' ');

			if (index >= 0)
			{
				Left = CssDistance.FromString(value.Substring(0, index));
				Top = CssDistance.FromString(value.Substring(index+1));
			}
			else
			{
				Left = CssDistance.FromString(value);
				Top = Left;
			}
		}
	}
	public enum CssBackgroundRepeat
	{
		Repeat,
		Repeat_X,
		Repeat_Y,
		Space,
		Round,
		No_Repeat,
	}
	public enum CssBackgroundClip
	{
		Border_Box,
		Padding_Box,
		Content_Box,
	}
}
