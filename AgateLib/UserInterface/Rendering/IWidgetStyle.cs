using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Css;
using AgateLib.UserInterface.Css.Layout;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Rendering
{
	public interface IWidgetStyle
	{
		Widget Widget { get; }

		CssBoxModel BoxModel { get; }

		CssStyleData Data { get; }
	}
}
