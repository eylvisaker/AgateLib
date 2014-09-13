using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.InputLib
{
	public class DirectionPad
	{
		public bool Up { get; internal set; }
		public bool Down { get; internal set; }
		public bool Left { get; internal set; }
		public bool Right { get; internal set; }

		internal void OnChanged(Gamepad gamepad)
		{
			if (Changed != null)
				Changed(gamepad, EventArgs.Empty);
		}

		public event EventHandler Changed;
	}
}
