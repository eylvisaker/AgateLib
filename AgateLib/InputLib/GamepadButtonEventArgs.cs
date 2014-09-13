using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.InputLib
{
	public class GamepadButtonEventArgs : EventArgs
	{
		private GamepadButton mappedTarget;

		public GamepadButtonEventArgs(GamepadButton mappedTarget)
		{
			// TODO: Complete member initialization
			this.mappedTarget = mappedTarget;
		}
	}
}
