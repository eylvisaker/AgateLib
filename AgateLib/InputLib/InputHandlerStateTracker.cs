using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.InputLib
{
	internal class InputHandlerStateTracker
	{
		// These should not be reordered without also updating the ReleaseEventFor method.
		private const int keyOffset = 0;
		private const int mouseOffset = 0x1000000;
		private const int joystickButtonOffset = 0x2000000;
		private const int joystickHatOffset = 0x4000000;

		private const int joystickIndexMultiplier = 0x0010000;
		private const int joystickIndexMask = 0x0ff0000;
		private const int joystickButtonMask = 0x000ffff;

		private const int joystickHatIndexMultiplier = 0x10;
		private const int joystickHatIndexMask = 0xf0;

		private const int hatUp = joystickHatOffset + 0;
		private const int hatLeft = joystickHatOffset + 1;
		private const int hatDown = joystickHatOffset + 2;
		private const int hatRight = joystickHatOffset + 3;

		private int[] hatButtons = { hatUp, hatLeft, hatDown, hatRight };

		private Point mousePosition;

		private HashSet<int> ButtonsPressed = new HashSet<int>();

		private Dictionary<IInputHandler, HandlerState> HandlerStates = new Dictionary<IInputHandler, HandlerState>();

		public void Synchronize(IEnumerable<IInputHandler> handlers)
		{
			var missingHandlers = HandlerStates.Keys.Where(x => !handlers.Contains(x)).ToList();

			foreach (var handler in missingHandlers)
			{
				HandlerStates.Remove(handler);
			}
		}

		public void FixButtonState(IEnumerable<IInputHandler> inputHandlers, Action<AgateInputEventArgs> queueEvent)
		{
			foreach (var handler in inputHandlers)
			{
				var handlerState = GetOrCreateHandlerState(handler);

				if (!handlerState.ButtonsPressed.SetEquals(ButtonsPressed))
				{
					foreach (var key in handlerState.ButtonsPressed)
					{
						if (!ButtonsPressed.Contains(key))
						{
							queueEvent(ReleaseEventFor(key));
						}
					}
				}

				if (handler.ForwardUnhandledEvents == false)
					break;
			}
		}

		public void TrackHandlerButtonState(IInputHandler handler, AgateInputEventArgs evt)
		{
			var state = GetOrCreateHandlerState(handler);

			UpdateButtonPressedState(state.ButtonsPressed, evt);
		}

		public void TrackGlobalButtonState(AgateInputEventArgs evt)
		{
			if (evt.InputEventType == InputEventType.MouseMove)
				mousePosition = evt.MousePosition;
			
			UpdateButtonPressedState(ButtonsPressed, evt);
		}

		private void UpdateButtonPressedState(HashSet<int> buttonPressedState, AgateInputEventArgs evt)
		{
			if (evt.InputEventType == InputEventType.JoystickHatChanged)
			{
				var pressedButtons = HatButtons(evt);

				foreach (var button in pressedButtons)
					buttonPressedState.Add(button);

				foreach (var button in hatButtons.Except(pressedButtons))
					buttonPressedState.Remove(button);
			}

			var hash = EventButtonHash(evt);

			if (hash < 0)
				return;

			if (IsPressEvent(evt.InputEventType))
				buttonPressedState.Add(hash);
			else if (IsReleaseEvent(evt.InputEventType))
				buttonPressedState.Remove(hash);
		}

		private bool IsPressEvent(InputEventType eventType)
		{
			switch (eventType)
			{
				case InputEventType.JoystickButtonPressed:
				case InputEventType.KeyDown:
				case InputEventType.MouseDown:
					return true;

				default:
					return false;
			}
		}

		private bool IsReleaseEvent(InputEventType eventType)
		{
			switch (eventType)
			{
				case InputEventType.JoystickButtonReleased:
				case InputEventType.KeyUp:
				case InputEventType.MouseUp:
					return true;

				default:
					return false;
			}
		}

		private AgateInputEventArgs ReleaseEventFor(int key)
		{
			if (key >= joystickHatOffset)
			{
				var joystickIndex = (key & joystickIndexMask) / joystickIndexMultiplier;
				var joystick = Input.Joysticks[joystickIndex];

				return AgateInputEventArgs.JoystickHatStateChanged(joystick, key & joystickHatIndexMask - joystickHatIndexMultiplier);
			}
			if (key >= joystickButtonOffset)
			{
				var joystickIndex = (key & joystickIndexMask) / joystickIndexMultiplier;
				var joystick = Input.Joysticks[joystickIndex];

				return AgateInputEventArgs.JoystickButtonReleased(joystick, key & joystickButtonMask);
			}
			if (key >= mouseOffset)
				return AgateInputEventArgs.MouseUp(mousePosition, (MouseButton)(key - mouseOffset));
			if (key >= keyOffset)
				return AgateInputEventArgs.KeyUp((KeyCode)(key - keyOffset), new KeyModifiers());

			return null;
		}

		private HandlerState GetOrCreateHandlerState(IInputHandler handler)
		{
			if (!HandlerStates.ContainsKey(handler))
			{
				HandlerStates.Add(handler, new InputLib.HandlerState());
			}

			return HandlerStates[handler];
		}


		private int EventButtonHash(AgateInputEventArgs evt)
		{
			if (evt.IsKeyboardEvent)
				return keyOffset + (int)evt.KeyCode;
			if (evt.IsMouseEvent)
				return mouseOffset + (int)evt.MouseButton;
			if (evt.IsJoystickButtonEvent)
				return joystickButtonOffset + (int)evt.JoystickButtonIndex + joystickIndexMultiplier * evt.JoystickIndex;

			return -1;
		}

		private IEnumerable<int> HatButtons(AgateInputEventArgs evt)
		{
			var hatIndex = evt.JoystickHatIndex * joystickHatIndexMultiplier;

			switch (Input.Joysticks[evt.JoystickIndex].HatState(evt.JoystickHatIndex))
			{
				case HatState.Down:
					yield return hatIndex + hatDown;
					break;
				case HatState.DownLeft:
					yield return hatIndex + hatLeft;
					yield return hatIndex + hatDown;
					break;
				case HatState.DownRight:
					yield return hatIndex + hatRight;
					yield return hatIndex + hatDown;
					break;
				case HatState.Left:
					yield return hatIndex + hatLeft;
					break;
				case HatState.Right:
					yield return hatIndex + hatRight;
					break;
				case HatState.Up:
					yield return hatIndex + hatUp;
					break;
				case HatState.UpLeft:
					yield return hatIndex + hatUp;
					yield return hatIndex + hatLeft;
					break;
				case HatState.UpRight:
					yield return hatIndex + hatUp;
					yield return hatIndex + hatRight;
					break;
				default:
					yield break;
			}
		}
	}
}
