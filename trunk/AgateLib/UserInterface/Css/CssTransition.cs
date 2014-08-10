using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface.Css
{
	public class CssTransition : ICssPropertyFromText
	{
		public CssTransition()
		{
			Clear();
		}

		public void SetValueFromText(string value)
		{
			var values = value.Split(' ');

			foreach(var v in values)
			{
				CssTransitionType type;
				CssTransitionDirection dir;
				double time;

				if (double.TryParse(v, out time))
					Time = time;
				else if (Enum.TryParse(v, true, out type))
					Type = type;
				else if (Enum.TryParse(v, true, out dir))
					Direction = dir;
			}
		}

		public void Clear()
		{
			Type = CssTransitionType.None;
			Time = 0.5;
		}

		public CssTransitionType Type { get; set; }
		public CssTransitionDirection Direction { get; set; }
		public double Time { get; set; }
	}

	public enum CssTransitionType
	{
		None,
		Slide,
	}
	public enum CssTransitionDirection
	{
		Left,
		Right,
		Top,
		Bottom,
	}
}
