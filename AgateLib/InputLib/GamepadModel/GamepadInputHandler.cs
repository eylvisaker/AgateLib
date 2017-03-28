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
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.InputLib.GamepadModel
{
	/// <summary>
	/// Class which represents an InputHandler object which 
	/// </summary>
	public class GamepadInputHandler : IInputHandler, IDisposable
	{
		private readonly List<Gamepad> gamepads = new List<Gamepad>();
		private readonly Dictionary<IJoystick, Gamepad> joypadMapping = new Dictionary<IJoystick, Gamepad>();
		private readonly HashSet<KeyCode> keysPressed = new HashSet<KeyCode>();

		/// <summary>
		/// Constructs a GamepadInputHandler with a single gamepad and the default keymap.
		/// </summary>
		public GamepadInputHandler()
		{
			GamepadCount = 1;

			KeyMap = KeyboardGamepadMap.Default;
		}

		/// <summary>
		/// Disposes of the GamepadInputHandler.
		/// </summary>
		public void Dispose()
		{
			Input.Handlers.Remove(this);
		}

		public KeyboardGamepadMap KeyMap { get; }

		/// <summary>
		/// If set to true (the default), unbound keyboard and mouse events can 
		/// be captured by lower input handlers.
		/// </summary>
		public bool ForwardUnhandledEvents { get; set; } = true;

		/// <summary>
		/// Gets or sets the count of gamepads.
		/// </summary>
		public int GamepadCount
		{
			get { return gamepads.Count; }
			set
			{
				List<Gamepad> newPads = new List<Gamepad>();

				for (int i = 0; i < gamepads.Count && i < value; i++)
					newPads.Add(gamepads[i]);

				var unusedJoysticks = Input.Joysticks.Except(joypadMapping.Keys).ToList();

				while (gamepads.Count < value)
				{
					if (unusedJoysticks.Count > 0)
					{
						var joystick = unusedJoysticks.First();
						unusedJoysticks.RemoveAt(0);

						var gamepad = new Gamepad(gamepads.Count, joystick);

						gamepads.Add(gamepad);

						joypadMapping[joystick] = gamepad;
					}
					else
					{
						gamepads.Add(new Gamepad(gamepads.Count, null));
					}
				}
			}
		}

		/// <summary>
		/// Gets the list of gamepads.
		/// </summary>
		public IReadOnlyList<IGamepad> Gamepads => gamepads;

		/// <summary>
		/// Processes an input event.
		/// </summary>
		/// <param name="args"></param>
		public void ProcessEvent(AgateInputEventArgs args)
		{
			switch (args.InputEventType)
			{
				case InputEventType.JoystickButtonPressed:
					OnJoystickButton(Input.Joysticks[args.JoystickIndex], args.JoystickButtonIndex, true);
					args.Handled = true;
					break;

				case InputEventType.JoystickButtonReleased:
					OnJoystickButton(Input.Joysticks[args.JoystickIndex], args.JoystickButtonIndex, false);
					args.Handled = true;
					break;

				case InputEventType.JoystickHatChanged:
					OnJoystickHat(Input.Joysticks[args.JoystickIndex]);
					args.Handled = true;
					break;

				case InputEventType.JoystickAxisChanged:
					OnJoystickAxis(Input.Joysticks[args.JoystickIndex], args.JoystickAxisIndex);
					args.Handled = true;
					break;

				case InputEventType.KeyDown:
					OnKeyDown(args);
					break;

				case InputEventType.KeyUp:
					OnKeyUp(args);
					break;
			}
		}

		private void OnKeyDown(AgateInputEventArgs args)
		{
			keysPressed.Add(args.KeyCode);

			args.Handled = MapKey(args.KeyCode, true);
		}

		private void OnKeyUp(AgateInputEventArgs args)
		{
			keysPressed.Remove(args.KeyCode);

			args.Handled = MapKey(args.KeyCode, false);
		}

		/// <summary>
		/// Maps the key input to a gamepad. Returns true if successfully mapped, false otherwise.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		private bool MapKey(KeyCode key, bool value)
		{
			if (!KeyMap.ContainsKey(key))
				return false;

			var keyMapItem = KeyMap[key];
			var gamepad = gamepads[keyMapItem.GamepadIndex];

			switch (keyMapItem.KeyMapType)
			{
				case KeyMapType.Button:
					gamepad.SetButton(keyMapItem.Button, value);
					break;

				case KeyMapType.LeftStick:
					gamepad.LeftStick = ComputeNewStickValue(gamepad.LeftStick, value, keyMapItem);
					break;

				case KeyMapType.RightStick:
					gamepad.RightStick = ComputeNewStickValue(gamepad.RightStick, value, keyMapItem);
					break;

				case KeyMapType.DirectionPad:
					gamepad.DirectionPad = ComputeNewDirectionPadValue(gamepad.DirectionPad, value, keyMapItem);
					break;

				case KeyMapType.LeftTrigger:
					gamepad.LeftTrigger = value ? keyMapItem.Value : 0;
					break;

				case KeyMapType.RightTrigger:
					gamepad.RightTrigger = value ? keyMapItem.Value : 0;
					break;

				default:
					throw new NotImplementedException();
			}

			return true;
		}

		private Point ComputeNewDirectionPadValue(Point dpad, bool value, KeyMapItem keyMapItem)
		{
			if (keyMapItem.Axis == Axis.X)
			{
				dpad.X = value ? Math.Sign(keyMapItem.Value) : 0;

				if (!value)
				{
					var otherDirectionMapping = FindPressedKeyMap(keyMapItem.GamepadIndex, keyMapItem.KeyMapType, keyMapItem.Axis);

					if (otherDirectionMapping != null)
						dpad.X = Math.Sign(otherDirectionMapping.Value);
				}
			}
			else
			{
				dpad.Y = value ? Math.Sign(keyMapItem.Value) : 0;

				if (!value)
				{
					var otherDirectionMapping = FindPressedKeyMap(keyMapItem.GamepadIndex, keyMapItem.KeyMapType, keyMapItem.Axis);

					if (otherDirectionMapping != null)
						dpad.Y = Math.Sign(otherDirectionMapping.Value);
				}
			}

			return dpad;
		}

		private Vector2 ComputeNewStickValue(Vector2 stickValue, bool value, KeyMapItem keyMapItem)
		{
			if (keyMapItem.Axis == Axis.X)
			{
				stickValue.X = value ? keyMapItem.Value : 0;

				if (!value)
				{
					var otherDirectionMapping = FindPressedKeyMap(keyMapItem.GamepadIndex, keyMapItem.KeyMapType, keyMapItem.Axis);

					if (otherDirectionMapping != null)
						stickValue.X = otherDirectionMapping.Value;
				}
			}
			else
			{
				stickValue.Y = value ? keyMapItem.Value : 0;

				if (!value)
				{
					var otherDirectionMapping = FindPressedKeyMap(keyMapItem.GamepadIndex, keyMapItem.KeyMapType, keyMapItem.Axis);

					if (otherDirectionMapping != null)
						stickValue.Y = otherDirectionMapping.Value;
				}
			}
			return stickValue;
		}

		private KeyMapItem FindPressedKeyMap(int gamepadIndex, KeyMapType keyMapType, Axis axis)
		{
			foreach (var key in keysPressed)
			{
				if (!KeyMap.ContainsKey(key))
					continue;

				var item = KeyMap[key];

				if (gamepadIndex != item.GamepadIndex)
					continue;

				if (item.KeyMapType != keyMapType)
					continue;

				if (item.Axis != axis)
					continue;

				return item;
			}

			return null;
		}

		private void OnJoystickAxis(IJoystick joystick, int axisIndex)
		{
			if (joypadMapping.ContainsKey(joystick))
			{
				var gamepad = joypadMapping[joystick];

				if (axisIndex < 2)
					gamepad.LeftStick = gamepad.ReadStickFromJoystick(0);
				else if (axisIndex < 4)
					gamepad.RightStick = gamepad.ReadStickFromJoystick(1);

				// For triggers, we transform the value because the axis value reported by the joystick is
				// in the range -1 to 1, but we want 0 to 1 for triggers.
				else if (axisIndex == 4)
					gamepad.LeftTrigger = (1 + joystick.AxisState(axisIndex)) * 0.5;
				else if (axisIndex == 5)
					gamepad.RightTrigger = (1 + joystick.AxisState(axisIndex)) * 0.5;
			}
		}

		private void OnJoystickHat(IJoystick joystick)
		{
			if (joypadMapping.ContainsKey(joystick))
			{
				var gamepad = joypadMapping[joystick];

				gamepad.DirectionPad = gamepad.ReadDirectionPadFromJoystick();
			}
		}

		private void OnJoystickButton(IJoystick joystick, int joystickButtonIndex, bool value)
		{
			if (joypadMapping.ContainsKey(joystick))
			{
				var gamepad = joypadMapping[joystick];

				gamepad.SetButton((GamepadButton)joystickButtonIndex, value);
			}
		}
	}
}
