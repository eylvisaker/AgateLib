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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.InputLib
{
	/// <summary>
	/// Class which represents data for an input event.
	/// </summary>
	public class AgateInputEventArgs : EventArgs
	{
		#region --- Static Members ---

		/// <summary>
		/// Constructs an AgateInputEventArgs for a mouse down event.
		/// </summary>
		/// <param name="mousePosition"></param>
		/// <param name="button"></param>
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
		/// <param name="mousePosition"></param>
		/// <param name="button"></param>
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
		/// <param name="mousePosition"></param>
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
		/// <param name="mousePosition"></param>
		/// <param name="mouseButton"></param>
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
		/// <param name="mousePosition"></param>
		/// <param name="wheelDelta"></param>
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
				KeyString = GetKeyString(code, modifiers),
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
				KeyString = GetKeyString(code, modifiers),
				KeyModifiers = modifiers,
			};
		}
		/// <summary>
		/// Creates a string from the specified KeyCode and KeyModifiers.
		/// Unfortunately this is tied to the US English keyboard, so it needs a better solution.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="mods"></param>
		/// <returns></returns>
		public static string GetKeyString(KeyCode key, KeyModifiers mods)
		{
			if ((int)key >= 'A' && (int)key <= 'Z')
			{
				char result;

				if (mods.Shift)
					result = (char)key;
				else
					result = (char)((int)key + 'a' - 'A');

				return result.ToString();
			}


			switch (key)
			{
				case KeyCode.Tab:
					return "\t";

				case KeyCode.Return:
					//case KeyCode.Enter:
					return "\n";

				case KeyCode.Space:
					return " ";

				// I'd love a better way of doing this:
				// likely, this is not very friendly to non US keyboard layouts.
				case KeyCode.D0:
					if (mods.Shift)
						return ")";
					else
						return "0";

				case KeyCode.NumPad0:
					return "0";

				case KeyCode.D1:
					if (mods.Shift)
						return "!";
					else return "1";


				case KeyCode.NumPad1:
					return "1";

				case KeyCode.D2:
					if (mods.Shift)
						return "@";
					else return "2";

				case KeyCode.NumPad2:
					return "2";

				case KeyCode.D3:
					if (mods.Shift)
						return "#";
					else return "3";

				case KeyCode.NumPad3:
					return "3";

				case KeyCode.D4:
					if (mods.Shift)
						return "$";
					else return "4";

				case KeyCode.NumPad4:
					return "4";

				case KeyCode.D5:
					if (mods.Shift)
						return "%";
					else return "5";

				case KeyCode.NumPad5:
					return "5";

				case KeyCode.D6:
					if (mods.Shift)
						return "^";
					else return "6";

				case KeyCode.NumPad6:
					return "6";

				case KeyCode.D7:
					if (mods.Shift)
						return "&";
					else return "7";

				case KeyCode.NumPad7:
					return "7";

				case KeyCode.D8:
					if (mods.Shift)
						return "*";
					else return "8";


				case KeyCode.NumPad8:
					return "8";

				case KeyCode.D9:
					if (mods.Shift)
						return "(";
					else return "9";

				case KeyCode.NumPad9:
					return "9";

				case KeyCode.NumPadMinus:
					return "-";
				case KeyCode.NumPadMultiply:
					return "*";
				case KeyCode.NumPadPeriod:
					return ".";
				case KeyCode.NumPadPlus:
					return "+";
				case KeyCode.NumPadSlash:
					return "/";

				case KeyCode.Semicolon:
					if (mods.Shift)
						return ":";
					else
						return ";";

				case KeyCode.Plus:
					if (mods.Shift)
						return "+";
					else
						return "=";

				case KeyCode.Comma:
					if (mods.Shift)
						return "<";
					else
						return ",";

				case KeyCode.Minus:
					if (mods.Shift)
						return "_";
					else
						return "-";

				case KeyCode.Period:
					if (mods.Shift)
						return ">";
					else
						return ".";

				case KeyCode.Slash:
					if (mods.Shift)
						return "?";
					else
						return "/";

				case KeyCode.Tilde:
					if (mods.Shift)
						return "~";
					else
						return "`";

				case KeyCode.OpenBracket:
					if (mods.Shift)
						return "{";
					else
						return "[";

				case KeyCode.BackSlash:
					if (mods.Shift)
						return "|";
					else
						return @"\";

				case KeyCode.CloseBracket:
					if (mods.Shift)
						return "}";
					else
						return "]";

				case KeyCode.Quotes:
					if (mods.Shift)
						return "\"";
					else
						return "'";

			}

			return "";
		}

		/// <summary>
		/// Creates an event args for a change in a joystick axis
		/// </summary>
		/// <param name="joystick"></param>
		/// <param name="axisIndex"></param>
		/// <returns></returns>
		public static AgateInputEventArgs JoystickAxisChanged(IJoystick joystick, int axisIndex)
		{
			return new AgateInputEventArgs
			{
				InputEventType = InputEventType.JoystickAxisChanged,
				JoystickIndex = Input.IndexOfJoystick(joystick),
				JoystickAxisIndex = axisIndex,
			};
		}

		/// <summary>
		/// Creates an event args for a joystick button press.
		/// </summary>
		/// <param name="joystick"></param>
		/// <param name="buttonIndex"></param>
		/// <returns></returns>
		public static AgateInputEventArgs JoystickButtonPressed(IJoystick joystick, int buttonIndex)
		{
			return new AgateInputEventArgs
			{
				InputEventType = InputEventType.JoystickButtonPressed,
				JoystickIndex = Input.IndexOfJoystick(joystick),
				JoystickButtonIndex = buttonIndex,
			};
		}

		/// <summary>
		/// Creates an event args for a joystick button release event.
		/// </summary>
		/// <param name="joystick"></param>
		/// <param name="buttonIndex"></param>
		/// <returns></returns>
		public static AgateInputEventArgs JoystickButtonReleased(IJoystick joystick, int buttonIndex)
		{
			return new AgateInputEventArgs
			{
				InputEventType = InputEventType.JoystickButtonReleased,
				JoystickIndex = Input.IndexOfJoystick(joystick),
				JoystickButtonIndex = buttonIndex,
			};
		}

		/// <summary>
		/// Creates an event args for a change in a joystick POV hat state.
		/// </summary>
		/// <param name="joystick"></param>
		/// <param name="hatIndex"></param>
		/// <returns></returns>
		public static AgateInputEventArgs JoystickHatStateChanged(IJoystick joystick, int hatIndex)
		{
			return new AgateInputEventArgs
			{
				InputEventType = InputEventType.JoystickHatChanged,
				JoystickIndex = Input.IndexOfJoystick(joystick),
				JoystickHatIndex = hatIndex,
			};
		}

		#endregion

		/// <summary>
		/// Set this to true to indicate that this event was handled 
		/// and should not be passed to any lower event handlers.
		/// </summary>
		public bool Handled { get; set; }

		/// <summary>
		/// Gets the type of input event.
		/// </summary>
		public InputEventType InputEventType { get; set; }

		/// <summary>
		/// Gets the KeyCode for a key down/up event.
		/// </summary>
		public KeyCode KeyCode { get; set; }

		/// <summary>
		/// Gets the text string for a key input event.
		/// </summary>
		public string KeyString { get; set; }

		/// <summary>
		/// Gets the key modifiers structure indicating whether modifier
		/// keys are pressed for the event.
		/// </summary>
		public KeyModifiers KeyModifiers { get; set; }

		/// <summary>
		/// Gets the mouse position for the event.
		/// </summary>
		public Point MousePosition { get; set; }

		/// <summary>
		/// Gets the mouse button for the event.
		/// </summary>
		public MouseButton MouseButton { get; set; }

		/// <summary>
		/// Gets how far the mouse wheel was moved.
		/// </summary>
		public int MouseWheelDelta { get; set; }

		/// <summary>
		/// Gets the joystick index for joystick events.
		/// </summary>
		public int JoystickIndex { get; set; }

		/// <summary>
		/// Gets the joystick axis index for joystick axis changed events.
		/// </summary>
		public int JoystickAxisIndex { get; set; }

		/// <summary>
		/// Gets the joystick button for joystick button events.
		/// </summary>
		public int JoystickButtonIndex { get; set; }

		/// <summary>
		/// Gets the joystick hat index for POV hat events.
		/// </summary>
		public int JoystickHatIndex { get; set; }

		/// <summary>
		/// Gets the DisplayWindow this event occurred in. This property is null for 
		/// all events except mouse events.
		/// </summary>
		public DisplayWindow MouseWindow { get; internal set; }

		/// <summary>
		/// Gets whether this event is a mouse event.
		/// </summary>
		public bool IsMouseEvent
		{
			get
			{
				switch (InputEventType)
				{
					case InputEventType.MouseDown:
					case InputEventType.MouseUp:
					case InputEventType.MouseWheel:
					case InputEventType.MouseDoubleClick:
					case InputEventType.MouseMove:
						return true;

					default:
						return false;
				}
			}
		}

		/// <summary>
		/// Gets whether this event is a keyboard event.
		/// </summary>
		public bool IsKeyboardEvent
		{
			get
			{
				switch (InputEventType)
				{
					case InputEventType.KeyDown:
					case InputEventType.KeyUp:
						return true;

					default:
						return false;
				}
			}
		}

		/// <summary>
		/// Gets whether this event is a joystick button event.
		/// </summary>
		public bool IsJoystickButtonEvent
		{
			get
			{
				switch (InputEventType)
				{
					case InputEventType.JoystickButtonPressed:
					case InputEventType.JoystickButtonReleased:
						return true;

					default:
						return false;
				}
			}
		}

		/// <summary>
		/// Returns a string representation.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			switch (InputEventType)
			{
				case InputEventType.MouseMove:
					return $"Mouse Move: {MousePosition}";

				case InputEventType.MouseDown:
				case InputEventType.MouseDoubleClick:
				case InputEventType.MouseUp:
					return $"{InputEventType}: {MouseButton} @ {MousePosition}";

				case InputEventType.MouseWheel:
					return $"Mouse Wheel: {MouseWheelDelta} @ {MousePosition}";

				case InputEventType.KeyDown:
				case InputEventType.KeyUp:
					return $"{InputEventType}: {KeyCode}";

				case InputEventType.JoystickButtonPressed:
				case InputEventType.JoystickButtonReleased:
					return $"{InputEventType}: {JoystickButtonIndex}";

				case InputEventType.JoystickAxisChanged:
					return $"{InputEventType}: {JoystickAxisIndex}";

				case InputEventType.JoystickHatChanged:
					return $"Joystick Hat Changed";
			}

			return base.ToString();
		}
	}

}
