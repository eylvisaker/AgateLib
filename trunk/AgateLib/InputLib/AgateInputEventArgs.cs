//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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

		public static AgateInputEventArgs MouseDown(object sender, Point mousePosition, MouseButton button)
		{
			return new AgateInputEventArgs
			{
				InputEventType = InputEventType.MouseDown,
				MousePosition = mousePosition,
				MouseButton = button,
			};
		}
		public static AgateInputEventArgs MouseUp(object sender, Point mousePosition, MouseButton button)
		{
			return new AgateInputEventArgs
			{
				InputEventType = InputEventType.MouseUp,
				MousePosition = mousePosition,
				MouseButton = button,
			};
		}
		public static AgateInputEventArgs MouseMove(object sender, Point mousePosition)
		{
			return new AgateInputEventArgs
			{
				InputEventType = InputEventType.MouseMove,
				MousePosition = mousePosition,
			};
		}

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
