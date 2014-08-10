using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Layout.Defaults
{
	public class MenuDefaultStyle : BlankDefaultStyle
	{
		public override void SetDefaultStyle(CssStyle style)
		{
			style.Data.Layout.Kind = CssLayoutKind.Column;
		}

	}
}
