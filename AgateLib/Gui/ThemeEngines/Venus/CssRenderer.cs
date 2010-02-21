using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Gui.ThemeEngines.Venus
{
	public class CssRenderer : WidgetRenderer 
	{
		Venus mEngine;
		Widget mWidget;
		string mClass;

		public CssRenderer(Venus engine, Widget w)
		{
			mEngine = engine;
			mWidget = w;
		}
		public override void DrawWidget(Widget w)
		{
			if (mClass != ClassName)
			{
				LoadCssInfo();
			}
		}

		private void LoadCssInfo()
		{
		}

		string ClassName { get; set; }

	}
}
