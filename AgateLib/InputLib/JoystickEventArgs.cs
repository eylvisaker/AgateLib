using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.InputLib
{
	public enum JoystickEventType
	{
		Button,
		Hat,
	}

	public class JoystickEventArgs : EventArgs
	{
		private JoystickEventType mEventType;
		private int mIndex;

		public JoystickEventArgs(JoystickEventType joystickEventType, int index)
		{
			// TODO: Complete member initialization
			this.mEventType = joystickEventType;
			this.mIndex = index;
		}

		public JoystickEventType JoystickEventType
		{
			get { return mEventType; }
		}

		public int Index
		{
			get { return mIndex; }
		}

	}
}
