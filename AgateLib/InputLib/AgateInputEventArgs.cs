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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib.Legacy;

namespace AgateLib.InputLib
{
	public class AgateInputEventArgs : EventArgs
	{
		/// <summary>
		/// Set this to true to indicate that this event was handled 
		/// and should not be passed to any lower event handlers.
		/// </summary>
		public bool Handled { get; set; }

		public InputEventType InputEventType { get; set; }

		public KeyCode KeyCode { get; set; }
		public string KeyString { get; set; }
		public KeyModifiers KeyModifiers { get; set; }

		public Point MousePosition { get; set; }
		public MouseButton MouseButton { get; set; }
		public int MouseWheelDelta { get; set; }

		/// <summary>
		/// Gets the DisplayWindow this event occurred in. This property is null for 
		/// all events except mouse events.
		/// </summary>
		public DisplayWindow Window { get; internal set; }

		/// <summary>
		/// Constructs an AgateInputEventArgs for a mouse down event.
		/// </summary>
		/// <param name="code"></param>
		/// <param name="modifiers"></param>
		/// <returns></returns>
		public static AgateInputEventArgs MouseDown(Point mousePosition, MouseButton button)
		{
			return new AgateInputEventArgs
			{
				InputEventType = InputEventType.MouseDown,
				MousePosition = mousePosition,
				MouseButton = button,
			};
		}

		/// <summary>
		/// Constructs an AgateInputEventArgs for a mouse upevent.
		/// </summary>
		/// <param name="code"></param>
		/// <param name="modifiers"></param>
		/// <returns></returns>
		public static AgateInputEventArgs MouseUp(Point mousePosition, MouseButton button)
		{
			return new AgateInputEventArgs
			{
				InputEventType = InputEventType.MouseUp,
				MousePosition = mousePosition,
				MouseButton = button,
			};
		}

		/// <summary>
		/// Constructs an AgateInputEventArgs for a mouse move event.
		/// </summary>
		/// <param name="code"></param>
		/// <param name="modifiers"></param>
		/// <returns></returns>
		public static AgateInputEventArgs MouseMove(Point mousePosition)
		{
			return new AgateInputEventArgs
			{
				InputEventType = InputEventType.MouseMove,
				MousePosition = mousePosition,
			};
		}

		/// <summary>
		/// Constructs an AgateInputEventArgs for a double click event.
		/// </summary>
		/// <param name="code"></param>
		/// <param name="modifiers"></param>
		/// <returns></returns>
		public static AgateInputEventArgs MouseDoubleClick(Point mousePosition, MouseButton mouseButton)
		{
			return new AgateInputEventArgs
			{
				InputEventType = InputEventType.MouseDoubleClick,
				MousePosition = mousePosition,
				MouseButton = mouseButton,
			};
		}

		/// <summary>
		/// Constructs an AgateInputEventArgs for a mouse wheel event.
		/// </summary>
		/// <param name="code"></param>
		/// <param name="modifiers"></param>
		/// <returns></returns>
		public static AgateInputEventArgs MouseWheel(Point mousePosition, int wheelDelta)
		{
			return new AgateInputEventArgs
			{
				InputEventType = InputEventType.MouseWheel,
				MousePosition = mousePosition,
				MouseWheelDelta = wheelDelta,
			};
		}
		
		/// <summary>
		/// Constructs an AgateInputEventArgs for a key down event.
		/// </summary>
		/// <param name="code"></param>
		/// <param name="modifiers"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Constructs an AgateInputEventArgs for a key up event.
		/// </summary>
		/// <param name="code"></param>
		/// <param name="modifiers"></param>
		/// <returns></returns>
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
		MouseDoubleClick,
		MouseWheel,

		JoystickAxisChanged,
		JoystickButton,
		JoystickPovHat,
	}
}
