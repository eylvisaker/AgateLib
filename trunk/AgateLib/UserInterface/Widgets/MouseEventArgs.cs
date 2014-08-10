using AgateLib.Geometry;
using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Widgets
{
	public class MouseEventArgs : EventArgs
	{
		public MouseEventArgs(Point location, MouseButton buttons)
		{
			this.Location = location;
			this.Buttons = buttons;
		}

		public Point Location { get; private set; }
		public MouseButton Buttons { get; private set; }
	}

}
