using AgateLib.UserInterface.Css.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css
{
	public class CssBoxComponent : ICssPropertyFromText, AgateLib.UserInterface.Css.ICssBoxComponent
	{
		public CssDistance Top { get; set; }
		public CssDistance Bottom { get; set; }
		public CssDistance Left { get; set; }
		public CssDistance Right { get; set; }

		public CssBoxComponent()
		{
			Top = new CssDistance(false);
			Bottom = new CssDistance(false);
			Right = new CssDistance(false);
			Left = new CssDistance(false);
		}

		static char[] sep = new char[] { '\t', '\n', '\r', ' ' };
		static int[,] indices = new int[4, 4] { { 0, 0, 0, 0 }, {0, 1, 0, 1}, {0, 1, 2, 1}, {0, 1, 2, 3} };
		
		public void SetValueFromText(string value)
		{
			string[] values = value.Split(sep, StringSplitOptions.RemoveEmptyEntries);

			Top = CssDistance.FromString(values[indices[values.Length-1, 0]]);
			Right = CssDistance.FromString(values[indices[values.Length-1, 1]]);
			Bottom = CssDistance.FromString(values[indices[values.Length-1, 2]]);
			Left = CssDistance.FromString(values[indices[values.Length-1, 3]]);
		}
	}
}
