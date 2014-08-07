using AgateLib.Geometry;
using AgateLib.InputLib.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.InputLib
{
	public class AgateInputEventArgs : EventArgs
	{
		public object Sender { get; set; }
		public bool Handled { get; set; }

		public InputEventType InputEventType { get; set; }

		public KeyCode KeyCode { get; set; }
		public string KeyString { get; set; }
		public KeyModifiers KeyModifiers { get; set; }

		public Point MousePosition { get; set; }
		public MouseButton MouseButton { get; set; }
		public int MouseWheelDelta { get; set; }


		public static AgateInputEventArgs KeyDown(KeyCode code, KeyModifiers modifiers)
		{
			return new AgateInputEventArgs
			{
				InputEventType = InputEventType.KeyDown,
				KeyCode = code,
				KeyString = Keyboard.GetKeyString(code, modifiers),
				KeyModifiers = modifiers,
			};
		}

		public static AgateInputEventArgs KeyUp(KeyCode code, KeyModifiers modifiers)
		{
			return new AgateInputEventArgs
			{
				InputEventType = InputEventType.KeyUp,
				KeyCode = code,
				KeyString = Keyboard.GetKeyString(code, modifiers),
				KeyModifiers = modifiers,
			};
		}
	}

	public enum InputEventType
	{
		KeyDown,
		KeyUp,

		MouseDown,
		MouseMove,
		MouseUp,

		JoystickAxisChanged,
		JoystickButton,
		JoystickPovHat,
		MouseWheel,
	}
}
