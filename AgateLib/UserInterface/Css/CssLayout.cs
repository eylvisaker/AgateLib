using AgateLib.UserInterface.Css.Binders;
using AgateLib.UserInterface.Css.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css
{
	public class CssLayout : ICssPropertyFromText
	{
		[CssAlias("layout-kind")]
		public CssLayoutKind Kind { get; set; }

		[CssAlias("layout-grid-columns")]
		public int GridColumns { get; set; }

		public void SetValueFromText(string value)
		{
			string[] values = value.Split(' ');

			foreach(var v in values)
			{
				CssLayoutKind result;
				int columns;

				if (Enum.TryParse<CssLayoutKind>(v, true, out result))
				{
					Kind = result;
				}
				else if (int.TryParse(v, out columns))
				{
					GridColumns = columns;
				}
			}
		}
	}

	public enum CssLayoutKind
	{
		Flow,
		Column,
		Row,
		Grid,
	}
}
