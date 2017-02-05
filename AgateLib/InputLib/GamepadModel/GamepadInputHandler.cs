using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;

namespace AgateLib.InputLib.GamepadModel
{
	public class GamepadInputHandler : IInputHandler, IDisposable
	{
		private readonly List<Gamepad> gamepads = new List<Gamepad>();
		private readonly Dictionary<IJoystick, Gamepad> joypadMapping = new Dictionary<IJoystick, Gamepad>();
		private readonly HashSet<KeyCode> keysPressed = new HashSet<KeyCode>();

		public GamepadInputHandler()
		{
			GamepadCount = 1;
		}

		public void Dispose()
		{
			Input.Handlers.Remove(this);
		}

		public KeyboardGamepadMap KeyMap { get; } = new KeyboardGamepadMap();

		/// <summary>
		/// If set to true (the default), unbound keyboard and mouse events can 
		/// be captured by lower input handlers.
		/// </summary>
		public bool ForwardUnhandledEvents { get; set; } = true;

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

						var gamepad = new Gamepad(joystick);

						gamepads.Add(gamepad);

						joypadMapping[joystick] = gamepad;
					}
					else
					{
						gamepads.Add(new Gamepad(null));
					}
				}
			}
		}

		public IReadOnlyList<IGamepad> Gamepads => gamepads;

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

			var item = KeyMap[key];

			switch (item.KeyMapType)
			{
				case KeyMapType.Button:
					item.Gamepad.SetButton(item.Button, value);
					break;

				case KeyMapType.LeftStick:

					Vector2 stickValue = item.Gamepad.LeftStick;

					if (item.StickAxis == StickAxis.X)
					{
						stickValue.X = value ? item.StickValue : 0;

						if (!value)
						{
							var otherDirectionMapping = FindPressedKeyMap(item.Gamepad, KeyMapType.LeftStick, item.StickAxis);

							if (otherDirectionMapping != null)
								stickValue.X = otherDirectionMapping.StickValue;
						}
					}
					else
					{
						stickValue.Y = value ? item.StickValue : 0;

						if (!value)
						{
							var otherDirectionMapping = FindPressedKeyMap(item.Gamepad, KeyMapType.LeftStick, item.StickAxis);

							if (otherDirectionMapping != null)
								stickValue.Y = otherDirectionMapping.StickValue;
						}
					}

					item.Gamepad.LeftStick = stickValue;

					break;

				default:
					throw new NotImplementedException();
			}

			return true;
		}

		private KeyMapItem FindPressedKeyMap(IGamepad pad, KeyMapType keyMapType, StickAxis axis)
		{
			foreach (var key in keysPressed)
			{
				if (!KeyMap.ContainsKey(key))
					continue;

				var item = KeyMap[key];

				if (item.Gamepad != pad)
					continue;

				if (item.KeyMapType != keyMapType)
					continue;

				if (item.StickAxis != axis)
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
