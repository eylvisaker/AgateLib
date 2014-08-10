using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Layout.Defaults
{
	public class BlankDefaultStyle : IDefaultStyleSetter
	{
		public virtual void SetDefaultStyle(CssStyle style)
		{
		}

		public virtual void InheritParentProperties(CssStyle style, CssStyle parentStyle)
		{
			style.Data.Font.Color = parentStyle.Data.Font.Color;
		}
	}
}
