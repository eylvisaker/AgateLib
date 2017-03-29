//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//
using System;

namespace AgateLib.InputLib.GamepadModel
{
	/// <summary>
	/// A data object which indicates how an event should be mapped to a gamepad button, axis or d-pad.
	/// </summary>
	public class KeyMapItem
	{
		/// <summary>
		/// Creates a mapping to an axis of the left stick of the gamepad.
		/// </summary>
		/// <param name="gamepad"></param>
		/// <param name="axis"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static KeyMapItem ToLeftStick(IGamepad gamepad, Axis axis, double value)
		{
			return ToLeftStick(gamepad.Index, axis, value);
		}

		/// <summary>
		/// Creates a mapping to an axis of the right stick of the gamepad.
		/// </summary>
		/// <param name="gamepad"></param>
		/// <param name="axis"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static KeyMapItem ToRightStick(IGamepad gamepad, Axis axis, double value)
		{
			return ToRightStick(gamepad.Index, axis, value);
		}

		/// <summary>
		/// Creates a mapping to a button.
		/// </summary>
		/// <param name="gamepad"></param>
		/// <param name="button"></param>
		/// <returns></returns>
		public static KeyMapItem ToButton(IGamepad gamepad, GamepadButton button)
		{
			return ToButton(gamepad.Index, button);
		}

		/// <summary>
		/// Creates a mapping to a d-pad direction.
		/// </summary>
		/// <param name="gamepadIndex"></param>
		/// <param name="axis"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static KeyMapItem ToDirectionPad(int gamepadIndex, Axis axis, int value)
		{
			return new KeyMapItem
			{
				GamepadIndex = gamepadIndex,
				KeyMapType = KeyMapType.DirectionPad,
				Axis = axis,
				Value = value,
			};
		}

		/// <summary>
		/// Creates a mapping to an axis of the left stick of the gamepad.
		/// </summary>
		/// <param name="gamepadIndex">Zero-based gamepad index.</param>
		/// <param name="axis"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static KeyMapItem ToLeftStick(int gamepadIndex, Axis axis, double value)
		{
			return new KeyMapItem
			{
				GamepadIndex = gamepadIndex,
				KeyMapType = KeyMapType.LeftStick,
				Axis = axis,
				Value = value
			};
		}

		/// <summary>
		/// Creates a mapping to an axis of the right stick of the gamepad.
		/// </summary>
		/// <param name="gamepadIndex"></param>
		/// <param name="axis"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static KeyMapItem ToRightStick(int gamepadIndex, Axis axis, double value)
		{
			return new KeyMapItem
			{
				GamepadIndex = gamepadIndex,
				KeyMapType = KeyMapType.RightStick,
				Axis = axis,
				Value = value
			};
		}

		/// <summary>
		/// Creates a mapping to a button.
		/// </summary>
		/// <param name="gamepadIndex"></param>
		/// <param name="button"></param>
		/// <returns></returns>
		public static KeyMapItem ToButton(int gamepadIndex, GamepadButton button)
		{
			return new KeyMapItem
			{
				GamepadIndex = gamepadIndex,
				KeyMapType = KeyMapType.Button,
				Button = button,
			};
		}

		/// <summary>
		/// Creates a mapping to the left trigger.
		/// </summary>
		/// <param name="gamepadIndex"></param>
		/// <returns></returns>
		public static KeyMapItem ToLeftTrigger(int gamepadIndex)
		{
			return new KeyMapItem
			{
				GamepadIndex = gamepadIndex,
				KeyMapType = KeyMapType.LeftTrigger,
				Value = 1,
			};
		}

		/// <summary>
		/// Creates a mapping to the right trigger.
		/// </summary>
		/// <param name="gamepadIndex"></param>
		/// <returns></returns>
		public static KeyMapItem ToRightTrigger(int gamepadIndex)
		{
			return new KeyMapItem
			{
				GamepadIndex = gamepadIndex,
				KeyMapType = KeyMapType.RightTrigger,
				Value = 1,
			};
		}

		/// <summary>
		/// What type of mapping this object represents.
		/// </summary>
		public KeyMapType KeyMapType { get; set; }

		/// <summary>
		/// The index of the gamepad this event is mapped to.
		/// </summary>
		public int GamepadIndex { get; set; }

		/// <summary>
		/// The value for the stick/dpad that should be set when the event happens.
		/// </summary>
		public double Value { get; set; }

		/// <summary>
		/// Which axis should be set with this event.
		/// </summary>
		public Axis Axis { get; set; }

		/// <summary>
		/// The button that should be set when this event happens.
		/// </summary>
		public GamepadButton Button { get; set; }
	}
}