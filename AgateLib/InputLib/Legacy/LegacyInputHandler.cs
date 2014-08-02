using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.InputLib.Legacy
{
	class LegacyInputHandler : IInputHandler
	{
		public void ProcessEvent(AgateInputEventArgs args)
		{
			switch(args.InputEventType)
			{
				case InputEventType.KeyDown: Keyboard.OnKeyDown(args); break;
				case InputEventType.KeyUp: Keyboard.OnKeyUp(args); break;
				case InputEventType.MouseDown: Mouse.OnMouseDown(args); break;
				case InputEventType.MouseUp: Mouse.OnMouseUp(args); break;
				case InputEventType.MouseWheel: Mouse.OnMouseWheel(args); break;
				case InputEventType.MouseMove: Mouse.OnMouseMove(args); break;
			}
		}

		public bool ForwardUnhandledEvents { get { return true;}}
	}
}
