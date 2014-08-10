using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Widgets
{
	public class KeyboardEventArgs : EventArgs
	{

		public KeyboardEventArgs(AgateLib.InputLib.KeyCode key, string keyString)
		{
			// TODO: Complete member initialization
			this.KeyCode = key;
			this.KeyString = keyString;
		}

		public KeyCode KeyCode { get; private set; }
		public string KeyString { get; private set; }
	}
}
