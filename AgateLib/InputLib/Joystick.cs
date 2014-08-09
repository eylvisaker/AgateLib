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
using System.Text;
using AgateLib.Geometry;
using AgateLib.InputLib.ImplementationBase;
using AgateLib.InputLib.Legacy;

namespace AgateLib.InputLib
{
	/// <summary>
	/// Class which encapsulates a single joystick.
	/// </summary>
	public class Joystick
	{
		JoystickImpl impl;
		bool[] mButtonState;
		HatState[] mHatState;

		internal Joystick(JoystickImpl i)
		{
			impl = i;

			AxisThreshold = 0.02;

			mButtonState = new bool[ButtonCount];
			mHatState = new HatState[HatCount];

		}

		/// <summary>
		/// Returns the name of the joystick.
		/// </summary>
		public string Name { get { return impl.Name; } }
		/// <summary>
		/// Returns the GUID that identifies this joystick hardware.
		/// </summary>
		public Guid Guid { get { return impl.Guid; } }

		/// <summary>
		/// Gets how many axes are available on this joystick.
		/// </summary>
		public int AxisCount { get { return impl.AxisCount; } }
		/// <summary>
		/// Returns the number of buttons this joystick has.
		/// </summary>
		public int ButtonCount { get { return impl.ButtonCount; } }
		/// <summary>
		/// Returns the number of POV hats this joystick has.
		/// </summary>
		public int HatCount { get { return impl.HatCount; } }


		/// <summary>
		/// Gets the state of the specified POV hat.
		/// </summary>
		/// <param name="hatIndex"></param>
		/// <returns></returns>
		[Obsolete("Use HatState property instead.")]
		public HatState GetHatState(int hatIndex)
		{
			return GetHatStateImpl(hatIndex);
		}
		private HatState GetHatStateImpl(int hatIndex)
		{
			return impl.GetHatState(hatIndex);
		}

		/// <summary>
		/// Gets an array indicating the state of the joystick hats.
		/// </summary>
		public HatState[] HatState
		{
			get { return mHatState; }
		}
		/// <summary>
		/// Gets the current value for the given axis.
		/// Axis 0 is always the x-axis, axis 1 is always the y-axis on
		/// controlers which have this capability.
		/// </summary>
		/// <param name="axisIndex"></param>
		/// <returns></returns>
		public double GetAxisValue(int axisIndex)
		{
			return impl.GetAxisValue(axisIndex);
		}
		/// <summary>
		/// Gets whether or not the specified button is pushed.
		/// </summary>
		/// <param name="buttonIndex"></param>
		/// <returns></returns>
		[Obsolete("Use ButtonState property instead.")]
		public bool GetButtonState(int buttonIndex)
		{
			return GetButtonStateImpl(buttonIndex);
		}
		private bool GetButtonStateImpl(int buttonIndex)
		{
			return impl.GetButtonState(buttonIndex);
		}

		/// <summary>
		/// Gets an array indicating the state of the buttons.
		/// </summary>
		public bool[] ButtonState
		{
			get { return mButtonState; }
		}

		/// <summary>
		/// Recalibrates this joystick.
		/// 
		/// Behavior is driver-dependent, however this usually means taking
		/// the current position as the "zeroed" position for the joystick.
		/// </summary>
		public void Recalibrate()
		{
			impl.Recalibrate();
		}
		/// <summary>
		/// Values smaller than this value for axes will
		/// be truncated and returned as zero.
		/// </summary>
		public double AxisThreshold
		{
			get { return impl.AxisThreshold; }
			set { impl.AxisThreshold = value; }
		}

		/// <summary>
		/// Returns the value of the joystick x-axis.
		/// Ranges are:
		/// -1 all the way to the left
		///  0 centered
		///  1 all the way to the right
		/// 
		/// Values outside this range may be returned.
		/// Never do tests which expect exact return values.
		/// Even digital gamepads will sometimes return values close to 1
		/// when pushed down, instead of exactly 1.
		/// 
		/// </summary>
		public double Xaxis
		{
			get { return impl.GetAxisValue(0); }
		}
		/// <summary>
		/// Returns the value of the joystick y-axis.
		/// Ranges are:
		/// -1 all the way to the top
		///  0 centered
		///  1 all the way to the bottom
		/// 
		/// Values outside this range may be returned.
		/// Never do tests which expect exact return values.
		/// Even digital gamepads will sometimes return values close to 1
		/// when pushed down, instead of exactly 1.
		/// 
		/// </summary>
		public double Yaxis
		{
			get { return impl.GetAxisValue(1); }
		}

		/// <summary>
		/// Returns whether or not this joystick is plugged in.
		/// 
		/// If a joystick is removed, you must throw away the reference to 
		/// this object and get a new one.
		/// </summary>
		public bool PluggedIn
		{
			get { return impl.PluggedIn; }
		}

		/// <summary>
		/// Polls the joystick for data.
		/// </summary>
		public void Poll()
		{
			impl.Poll();

			for (int i = 0; i < mButtonState.Length; i++)
			{
				bool newValue = GetButtonStateImpl(i);

				if (newValue != mButtonState[i])
				{
					mButtonState[i] = newValue;

					if (newValue)
						OnButtonPressed(i);
					else
						OnButtonReleased(i);
				}
			}
			for (int i = 0; i < mHatState.Length; i++)
			{
				HatState newValue = GetHatStateImpl(i);

				if (newValue != mHatState[i])
				{
					mHatState[i] = newValue;

					OnHatStateChanged(i);
				}
			}
		}


		public event JoystickEventHandler ButtonPressed;
		public event JoystickEventHandler ButtonReleased;
		public event JoystickEventHandler HatStateChanged;

		private void OnButtonPressed(int index)
		{
			if (ButtonPressed != null)
				ButtonPressed(this, 
					new JoystickEventArgs(JoystickEventType.Button, index));
		}
		private void OnButtonReleased(int index)
		{
			if (ButtonReleased != null)
				ButtonReleased(this,
					new JoystickEventArgs(JoystickEventType.Button, index));
		}
		private void OnHatStateChanged(int index)
		{
			if (HatStateChanged != null)
				HatStateChanged(this,
					new JoystickEventArgs(JoystickEventType.Hat, index));
		}
	}
}
