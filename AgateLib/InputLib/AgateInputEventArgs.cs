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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
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
		#region --- Static Members ---

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
		/// <param name="keyID"></param>
		/// <param name="mods"></param>
		/// <returns></returns>
		public static string GetKeyString(KeyCode keyID, KeyModifiers mods)
		{
			if ((int)keyID >= 'A' && (int)keyID <= 'Z')
			{
				char result;

				if (mods.Shift)
					result = (char)keyID;
				else
					result = (char)((int)keyID + 'a' - 'A');

				return result.ToString();
			}


			switch (keyID)
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

		#endregion

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
