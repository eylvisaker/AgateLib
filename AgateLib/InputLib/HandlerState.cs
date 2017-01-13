using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.InputLib
{
	/// <summary>
	/// Internal class used to track what information is sent to the 
	/// input handler.
	/// </summary>
	internal class HandlerState
	{
		public IInputHandler Handler { get; set; }

		public HashSet<KeyCode> KeysPressed { get; private set; } = new HashSet<KeyCode>();
	}
}
