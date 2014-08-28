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
using AgateLib.Drivers;
using AgateLib.InputLib.ImplementationBase;
using SlimDX.DirectInput;

namespace AgateSDX
{
	public class SDX_Input : InputImpl
	{
		DirectInput mDIobject;

		public override void Initialize()
		{
			mDIobject = new DirectInput();
			System.Diagnostics.Trace.WriteLine("Using Managed DirectX implementation of InputImpl.");
		}

		public override void Dispose()
		{
			mDIobject.Dispose();
		}

		public override int JoystickCount
		{
			get
			{
				int retval = 0;

				
				foreach (DeviceInstance i in mDIobject.GetDevices())
				{
					switch (i.Type)
					{
						case DeviceType.Gamepad:
						case DeviceType.Joystick:
							retval++;
							break;
					}
				}

				return retval;
			}
		}

		public override IEnumerable<JoystickImpl> CreateJoysticks()
		{
			List<JoystickImpl> retval = new List<JoystickImpl>();

			foreach (DeviceInstance i in mDIobject.GetDevices())
			{
				switch (i.Type)
				{
					case DeviceType.Gamepad:
					case DeviceType.Joystick:

						Joystick d = new Joystick(mDIobject, i.InstanceGuid);

						retval.Add(new SDX_Joystick(d));

						break;
				}
			}

			return retval;
		}
	}

	/// <summary>
	/// MDX1_Joystick class
	/// Could be done with action maps?  would this be better?
	/// </summary>
	public class SDX_Joystick : JoystickImpl
	{
		private Joystick mDevice;
		private bool[] mButtons;

		private int[] shift = new int[8];
		private double max;

		private double mThreshold;

		public SDX_Joystick(Joystick d)
		{
			mDevice = d;
			mDevice.Acquire();

			Recalibrate();

			// joystick values in di seem to be from 0 (left) to 65536 (right).
			// seems to be the right value on my joystick.
			max = 32768;

			mButtons = new bool[ButtonCount];
			shift = new int[AxisCount];
		}

		public override string Name
		{
			get { return mDevice.Information.InstanceName; }
		}
		public override int AxisCount
		{
			get { return mDevice.Capabilities.AxesCount; }
		}
		public override int ButtonCount
		{
			get { return mDevice.Capabilities.ButtonCount; }
		}
		public override int HatCount
		{
			get { return mDevice.Capabilities.PovCount; }
		}

		public override bool GetButtonState(int buttonIndex)
		{
			return mButtons[buttonIndex];
		}

		public override void Poll()
		{
			mDevice.Poll();

			bool[] di_buttons = mDevice.GetCurrentState().GetButtons();

			for (int i = 0; i < ButtonCount; i++)
				mButtons[i] = di_buttons[i];
		}

		public override double GetAxisValue(int axisIndex)
		{
			return CorrectAxisValue(mDevice.GetCurrentState().GetSliders()[axisIndex], shift[axisIndex], max);

			//if (axisIndex == 0)
			//    return CorrectAxisValue(mDevice.GetCurrentState().X, shift[0], maxX);
			//else if (axisIndex == 1)
			//    return CorrectAxisValue(mDevice.GetCurrentState().Y, shift[1], maxY);
			//else if (axisIndex == 2)
			//    return CorrectAxisValue(mDevice.GetCurrentState().RotationX, shift[2], maxX);
			//else if (axisIndex == 3)
			//    return CorrectAxisValue(mDevice.GetCurrentState().RotationY, shift[3], maxY);
			//else if (axisIndex == 4) 
			//    return CorrectAxisValue(mDevice.GetCurrentState().Z, shift[2], maxZ);
			//else
			//    return mDevice.GetCurrentState().GetSliders()[axisIndex - 4] / maxX;
		}

		public override AgateLib.InputLib.HatState GetHatState(int hatIndex)
		{
			int value = mDevice.GetCurrentState().GetPointOfViewControllers()[hatIndex];

			return (AgateLib.InputLib.HatState)value;
		}
		private double CorrectAxisValue(int axisValue, int shiftValue, double maxX)
		{
			double retval = (axisValue - shiftValue) / (double)maxX;

			if (Math.Abs(retval) < mThreshold)
				return 0;
			else
				return retval;
		}

		public override void Recalibrate()
		{
			shift[0] = mDevice.GetCurrentState().X;
			shift[1] = mDevice.GetCurrentState().Y;
			shift[2] = mDevice.GetCurrentState().Z;
		}

		public override double AxisThreshold
		{
			get
			{
				return mThreshold;
			}
			set
			{
				mThreshold = value;
			}
		}

		public override bool PluggedIn
		{
			get
			{
				if (mDevice == null)
					throw new NullReferenceException("Device is null.  This indicates a bug in the MDX1_1 library.");

				try
				{
					mDevice.Poll();

					return true;
				}
				catch (Exception e)
				{
					System.Diagnostics.Debug.WriteLine("Error polling joystick: " + e.Message);
					return false;
				}
			}
		}

	}
}