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
using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.InputLib
{
	public class Gamepad : IDisposable
	{
		static List<HatState> mHatUpStates = new List<HatState>();
		static List<HatState> mHatLeftStates = new List<HatState>();
		static List<HatState> mHatRightStates = new List<HatState>();
		static List<HatState> mHatDownStates = new List<HatState>();

		static Gamepad()
		{
			mHatUpStates.AddRange(new[] { HatState.Up, HatState.UpLeft, HatState.UpRight });
			mHatLeftStates.AddRange(new[] { HatState.Left, HatState.UpLeft, HatState.DownLeft });
			mHatRightStates.AddRange(new[] { HatState.Right, HatState.UpRight, HatState.DownRight });
			mHatDownStates.AddRange(new[] { HatState.Down, HatState.DownLeft, HatState.DownRight });
		}

		Joystick mJoystick;
		GamepadMap mMap;
		Dictionary<GamepadButton, bool> mButtonState = new Dictionary<GamepadButton, bool>();
		Vector2 mLeftStick;
		Vector2 mRightStick;
		float mLeftTrigger, mRightTrigger;

		public Gamepad(Joystick joystick)
			: this(joystick, GamepadMapDatabase.FindMap(joystick.Guid))
		{
		}

		private Gamepad(Joystick joystick, GamepadMap map)
		{
			mMap = map;

			mJoystick = joystick;

			mJoystick.AxisChanged += joystick_AxisChanged;
			mJoystick.ButtonPressed += joystick_ButtonPressed;
			mJoystick.ButtonReleased += joystick_ButtonReleased;
			mJoystick.HatStateChanged += joystick_HatStateChanged;
		}

		/// <summary>
		/// Disconnects the Gamepad instance from the hardware joystick.
		/// </summary>
		public void Dispose()
		{
			mJoystick.AxisChanged -= joystick_AxisChanged;
			mJoystick.ButtonPressed -= joystick_ButtonPressed;
			mJoystick.ButtonReleased -= joystick_ButtonReleased;
			mJoystick.HatStateChanged -= joystick_HatStateChanged;
		}


		public DirectionPad DPad { get; private set; }
		public Vector2 LeftStick { get { return mLeftStick; } }
		public Vector2 RightStick { get { return mRightStick; } }
		public float LeftTrigger { get { return mLeftTrigger; } }
		public float RightTrigger { get { return mRightTrigger; } }
		public Dictionary<GamepadButton, bool> Buttons { get { return mButtonState; } }

		void joystick_HatStateChanged(object sender, JoystickEventArgs e)
		{
			var hat = mJoystick.HatState[e.Index];

			DPad.Up = mHatUpStates.Contains(hat);
			DPad.Left = mHatLeftStates.Contains(hat);
			DPad.Right = mHatRightStates.Contains(hat);
			DPad.Down = mHatDownStates.Contains(hat);

			DPad.OnChanged(this);
		}

		void joystick_AxisChanged(object sender, JoystickEventArgs e)
		{
			var target = mMap.AxisMap[e.Index];

			if (target == GamepadMapTarget.None)
				return;

			var value = (float)mJoystick.GetAxisValue(e.Index);

			SetValue(target, value);
		}

		void joystick_ButtonReleased(object sender, JoystickEventArgs e)
		{
			MapButton(e.Index, 0);
		}
		void joystick_ButtonPressed(object sender, JoystickEventArgs e)
		{
			MapButton(e.Index, 1);
		}

		private void MapButton(int joystickButtonIndex, int value)
		{
			var target = mMap.ButtonMap[joystickButtonIndex];

			if (target == GamepadMapTarget.None)
				return;

			SetValue(target, 0);
		}

		private void SetValue(GamepadMapTarget target, float value)
		{
			switch (target)
			{
				case GamepadMapTarget.LeftX:
				case GamepadMapTarget.LeftY:
				case GamepadMapTarget.RightX:
				case GamepadMapTarget.RightY:
				case GamepadMapTarget.LeftTrigger:
				case GamepadMapTarget.RightTrigger:
					SetAxisValue(target, value);
					break;

				case GamepadMapTarget.A:
				case GamepadMapTarget.B:
				case GamepadMapTarget.X:
				case GamepadMapTarget.Y:
				case GamepadMapTarget.LeftBumper:
				case GamepadMapTarget.RightBumper:
				case GamepadMapTarget.Select:
				case GamepadMapTarget.Start:
				case GamepadMapTarget.LeftStickButton:
				case GamepadMapTarget.RightStickButton:
				case GamepadMapTarget.DPadUp:
				case GamepadMapTarget.DPadRight:
				case GamepadMapTarget.DPadLeft:
				case GamepadMapTarget.DPadDown:
					SetButtonValue(target, value);
					break;

				default:
					throw new NotImplementedException();
			}
		}

		void SetAxisValue(GamepadMapTarget target, float value)
		{
			var val = (float)value;

			switch (target)
			{
				case GamepadMapTarget.LeftX: mLeftStick.X = val; OnLeftStickMoved(); break;
				case GamepadMapTarget.LeftY: mLeftStick.Y = val; OnLeftStickMoved(); break;
				case GamepadMapTarget.RightX: mRightStick.X = val; OnRightStickMoved(); break;
				case GamepadMapTarget.RightY: mRightStick.Y = val; OnRightStickMoved(); break;
				case GamepadMapTarget.LeftTrigger: mLeftTrigger = val; OnLeftTriggerMoved(); break;
				case GamepadMapTarget.RightTrigger: mRightTrigger = val; OnRightTriggerMoved(); break;

				default:
					throw new NotImplementedException();
			}
		}


		void SetButtonValue(GamepadMapTarget target, float value)
		{
			if (value > 0.5)
				SetButtonValue(target, true);
			else
				SetButtonValue(target, false);
		}

		void SetButtonValue(GamepadMapTarget target, bool value)
		{
			GamepadButton mappedTarget;

			switch (target)
			{
				case GamepadMapTarget.A: mappedTarget = GamepadButton.A; break;
				case GamepadMapTarget.B: mappedTarget = GamepadButton.B; break;
				case GamepadMapTarget.X: mappedTarget = GamepadButton.X; break;
				case GamepadMapTarget.Y: mappedTarget = GamepadButton.Y; break;
				case GamepadMapTarget.LeftBumper: mappedTarget = GamepadButton.LeftBumper; break;
				case GamepadMapTarget.RightBumper: mappedTarget = GamepadButton.RightBumper; break;
				case GamepadMapTarget.Select: mappedTarget = GamepadButton.Select; break;
				case GamepadMapTarget.Start: mappedTarget = GamepadButton.Start; break;
				case GamepadMapTarget.LeftStickButton: mappedTarget = GamepadButton.LeftStickButton; break;
				case GamepadMapTarget.RightStickButton: mappedTarget = GamepadButton.RightStickButton; break;
				case GamepadMapTarget.Home: mappedTarget = GamepadButton.Home; break;

				case GamepadMapTarget.DPadUp: DPad.Up = value; DPad.OnChanged(this); return;
				case GamepadMapTarget.DPadRight: DPad.Right = value; DPad.OnChanged(this); return;
				case GamepadMapTarget.DPadLeft: DPad.Left = value; DPad.OnChanged(this); return;
				case GamepadMapTarget.DPadDown: DPad.Down = value; DPad.OnChanged(this); return;

				default:
					throw new NotImplementedException();
			}

			mButtonState[mappedTarget] = value;

			if (value)
				OnButtonPressed(mappedTarget);
			else
				OnButtonReleased(mappedTarget);
		}


		private void OnLeftStickMoved()
		{
			if (LeftStickMoved != null)
				LeftStickMoved(this, EventArgs.Empty);
		}
		private void OnRightStickMoved()
		{
			if (RightStickMoved != null)
				RightStickMoved(this, EventArgs.Empty);
		}
		private void OnLeftTriggerMoved()
		{
		}
		private void OnRightTriggerMoved()
		{
		}

		private void OnButtonPressed(GamepadButton mappedTarget)
		{
			if (ButtonPressed != null)
				ButtonPressed(this, new GamepadButtonEventArgs(mappedTarget));
		}
		private void OnButtonReleased(GamepadButton mappedTarget)
		{
			if (ButtonReleased != null)
				ButtonReleased(this, new GamepadButtonEventArgs(mappedTarget));
		}

		public event EventHandler LeftStickMoved;
		public event EventHandler RightStickMoved;

		public event EventHandler<GamepadButtonEventArgs> ButtonPressed;
		public event EventHandler<GamepadButtonEventArgs> ButtonReleased;

	}


	public enum GamepadButton
	{
		A,
		B,
		X,
		Y,
		LeftBumper,
		RightBumper,
		Select,
		Start,
		LeftStickButton,
		RightStickButton,
		Home
	}
}
