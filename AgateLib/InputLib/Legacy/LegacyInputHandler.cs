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
				case InputEventType.MouseDown:
					Mouse.Position = args.MousePosition;
					Mouse.OnMouseDown(args); 
					break;
				case InputEventType.MouseUp: 
					Mouse.Position = args.MousePosition; 
					Mouse.OnMouseUp(args); 
					break;
				case InputEventType.MouseWheel: 
					Mouse.Position = args.MousePosition; 
					Mouse.OnMouseWheel(args); 
					break;
				case InputEventType.MouseMove: 
					Mouse.Position = args.MousePosition;
					Mouse.OnMouseMove(args); 
					break;
			}
		}

		public bool ForwardUnhandledEvents { get { return true;}}
	}
}
