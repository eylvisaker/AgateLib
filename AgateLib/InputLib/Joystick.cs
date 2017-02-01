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
using System.Text;
using AgateLib.Geometry;
using AgateLib.InputLib.ImplementationBase;
using AgateLib.InputLib.Legacy;

namespace AgateLib.InputLib
{
	/// <summary>
	/// Represents a joystick connected to the system.
	/// </summary>
	public interface IJoystick
	{
		[Obsolete("Use input handler instead.", true)]
		event JoystickEventHandler AxisChanged;
		[Obsolete("Use input handler instead.", true)]
		event JoystickEventHandler ButtonPressed;
		[Obsolete("Use input handler instead.", true)]
		event JoystickEventHandler ButtonReleased;
		[Obsolete("Use input handler instead.", true)]
		event JoystickEventHandler HatStateChanged;

		/// <summary>
		/// Returns the name of the joystick.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Returns the GUID that identifies this joystick hardware.
		/// </summary>
		Guid Guid { get; }

		/// <summary>
		/// Gets how many axes are available on this joystick.
		/// </summary>
		int AxisCount { get; }

		/// <summary>
		/// Returns the number of buttons this joystick has.
		/// </summary>
		int ButtonCount { get; }

		/// <summary>
		/// Returns the number of POV hats this joystick has.
		/// </summary>
		int HatCount { get; }

		/// <summary>
		/// Values smaller than this value for axes will
		/// be truncated and returned as zero.
		/// </summary>
		double AxisThreshold { get; set; }

		/// <summary>
		/// Returns whether or not this joystick is plugged in.
		/// 
		/// If a joystick is removed, you must throw away the reference to 
		/// this object and get a new one.
		/// </summary>
		bool PluggedIn { get; }

		/// <summary>
		/// Gets an array indicating the state of the buttons.
		/// </summary>
		bool ButtonState(int buttonIndex);

		/// <summary>
		/// Gets the state of the specified hat.
		/// </summary>
		HatState HatState(int hatIndex);

		/// <summary>
		/// Returns the value of a joystick axis.
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
		/// Axis 0 is always the x-axis, axis 1 is always the y-axis on
		/// controllers which have this capability.
		/// </summary>
		/// <param name="axisIndex"></param>
		/// <returns></returns>
		double AxisState(int axisIndex);

		/// <summary>
		/// Recalibrates this joystick.
		/// 
		/// Behavior is driver-dependent, however this usually means taking
		/// the current position as the "zeroed" position for the joystick.
		/// </summary>
		void Recalibrate();
	}

	/// <summary>
	/// Class which encapsulates a single joystick.
	/// </summary>
	public class Joystick : IJoystick
	{
		IJoystickImpl impl;
		bool[] mButtonState;
		HatState[] mHatState;
		double[] mAxisState;

		internal Joystick(IJoystickImpl i)
		{
			impl = i;

			AxisThreshold = 0.02;

			mButtonState = new bool[ButtonCount];
			mHatState = new HatState[HatCount];
			mAxisState = new double[AxisCount];
		}

		[Obsolete("Use input handler instead.", true)]
		public event JoystickEventHandler AxisChanged;
		[Obsolete("Use input handler instead.", true)]
		public event JoystickEventHandler ButtonPressed;
		[Obsolete("Use input handler instead.", true)]
		public event JoystickEventHandler ButtonReleased;
		[Obsolete("Use input handler instead.", true)]
		public event JoystickEventHandler HatStateChanged;

		/// <summary>
		/// Returns the name of the joystick.
		/// </summary>
		public string Name => impl.Name;

		/// <summary>
		/// Returns the GUID that identifies this joystick hardware.
		/// </summary>
		public Guid Guid => impl.Guid;

		/// <summary>
		/// Gets how many axes are available on this joystick.
		/// </summary>
		public int AxisCount => impl.AxisCount;

		/// <summary>
		/// Returns the number of buttons this joystick has.
		/// </summary>
		public int ButtonCount => impl.ButtonCount;

		/// <summary>
		/// Returns the number of POV hats this joystick has.
		/// </summary>
		public int HatCount => impl.HatCount;

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
		/// Returns whether or not this joystick is plugged in.
		/// 
		/// If a joystick is removed, you must throw away the reference to 
		/// this object and get a new one.
		/// </summary>
		public bool PluggedIn => impl.PluggedIn;

		/// <summary>
		/// Gets an array indicating the state of the buttons.
		/// </summary>
		public bool ButtonState(int buttonIndex)
		{
			return mButtonState[buttonIndex];
		}

		/// <summary>
		/// Gets the state of the specified hat.
		/// </summary>
		public HatState HatState(int hatIndex)
		{
			return mHatState[hatIndex];
		}

		/// <summary>
		/// Returns the value of a joystick axis.
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
		/// Axis 0 is always the x-axis, axis 1 is always the y-axis on
		/// controllers which have this capability.
		/// </summary>
		/// <param name="axisIndex"></param>
		/// <returns></returns>
		public double AxisState(int axisIndex)
		{
			return impl.GetAxisValue(axisIndex);
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
		/// Polls the joystick for data.
		/// </summary>
		internal void Poll()
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

			for (int i = 0; i < mAxisState.Length; i++)
			{
				double newValue = AxisState(i);

				if (Math.Abs(newValue - mAxisState[i]) > 0.001)
				{
					mAxisState[i] = newValue;
					OnAxisChanged(i);
				}
			}
		}

		private void OnAxisChanged(int axisIndex)
		{
			Input.QueueInputEvent(AgateInputEventArgs
				.JoystickAxisChanged(this, axisIndex));
		}

		private void OnButtonPressed(int index)
		{
			Input.QueueInputEvent(AgateInputEventArgs
				.JoystickButtonPressed(this, index));
		}

		private void OnButtonReleased(int index)
		{
			Input.QueueInputEvent(AgateInputEventArgs
				.JoystickButtonReleased(this, index));
		}

		private void OnHatStateChanged(int index)
		{
			Input.QueueInputEvent(AgateInputEventArgs
				.JoystickHatStateChanged(this, index));
		}

		private HatState GetHatStateImpl(int hatIndex)
		{
			return impl.GetHatState(hatIndex);
		}

		private bool GetButtonStateImpl(int buttonIndex)
		{
			return impl.GetButtonState(buttonIndex);
		}

	}
}
