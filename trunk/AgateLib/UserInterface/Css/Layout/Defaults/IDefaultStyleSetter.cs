using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Layout.Defaults
{
	public interface IDefaultStyleSetter
	{
		void SetDefaultStyle(CssStyle style);

		void InheritParentProperties(CssStyle style, CssStyle parentStyle);
	}
}
