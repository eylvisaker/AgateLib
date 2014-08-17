using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
	public class WidgetEventArgs : EventArgs
	{
		public WidgetEventArgs(Widget widget)
		{
			this.Widget = widget;
		}

		public Widget Widget { get; private set; }
	}
}
