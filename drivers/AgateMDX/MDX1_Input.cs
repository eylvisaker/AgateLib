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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.DirectInput;

using ERY.AgateLib.Drivers;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib.MDX
{
    public class MDX1_Input : InputImpl
    {
        public static void Register()
        {
            Registrar.RegisterInputDriver(new DriverInfo<InputTypeID>(
                typeof(MDX1_Input), InputTypeID.DirectInput, "Managed DirectInput", 100));
        }
        public override void Initialize()
        {
            System.Diagnostics.Trace.WriteLine("Using Managed DirectX implementation of InputImpl.");
        }

        public override void Dispose()
        {

        }

        public override int CountJoysticks()
        {
            int retval = 0;

            foreach (DeviceInstance i in Manager.Devices)
            {
                switch (i.DeviceType)
                {
                    case DeviceType.Gamepad:
                    case DeviceType.Joystick:
                        retval++;
                        break;
                }
            }

            return retval;
        }

        public override IEnumerable<JoystickImpl> CreateJoysticks()
        {
            List<JoystickImpl> retval = new List<JoystickImpl>();

            foreach (DeviceInstance i in Manager.Devices)
            {
                switch (i.DeviceType)
                {
                    case DeviceType.Gamepad:
                    case DeviceType.Joystick:

                        Device d = new Device(i.InstanceGuid);

                        retval.Add(new MDX1_Joystick(d));

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
    public class MDX1_Joystick : JoystickImpl
    {
        private Device mDevice;
        private bool[] mButtons;

        private int shiftX, shiftY, shiftZ;
        private double maxX, maxY, maxZ;

        private double mThreshold;

        public MDX1_Joystick(Device d)
        {
            mDevice = d;
            mDevice.Acquire();

            Recalibrate();

            // joystick values in di seem to be from 0 (left) to 65536 (right).
            // seems to be the right value on my joystick.
            maxX = maxY = maxZ = 32768;

            mButtons = new bool[ButtonCount];
        }

        public override int AxisCount
        {
            get { return mDevice.Caps.NumberAxes; }
        }
        public override int ButtonCount
        {
            get { return mDevice.Caps.NumberButtons; }
        }

        public override bool[] Buttons
        {
            get
            {
                return mButtons;
            }
        }

        public override void Poll()
        {
            mDevice.Poll();
             
            byte[] di_buttons = mDevice.CurrentJoystickState.GetButtons();

            for (int i = 0; i < ButtonCount; i++)
                mButtons[i] = (di_buttons[i] != 0) ? true : false;
        }

        public override double GetAxisValue(int axisIndex)
        {
            if (axisIndex == 0)
                return Xaxis;
            else if (axisIndex == 1)
                return Yaxis;
            else if (axisIndex == 2)
                return Zaxis;

            else 
                return mDevice.CurrentJoystickState.GetSlider()[axisIndex - 3] / maxX;
        }
        public override double Xaxis
        {
            get
            {
                double retval = (mDevice.CurrentJoystickState.X - shiftX) / maxX;

                if (Math.Abs(retval) < mThreshold)
                    return 0;
                else
                    return retval;
            }
        }
        public override double Yaxis
        {
            get 
            { 
                double retval = (mDevice.CurrentJoystickState.Y - shiftY) / maxY;

                if (Math.Abs(retval) < mThreshold)
                    return 0;
                else
                    return retval;
            }
        }
        public double Zaxis
        {
            get
            {
                double retval = (mDevice.CurrentJoystickState.Z - shiftZ) / maxZ;

                if (Math.Abs(retval) < mThreshold)
                    return 0;
                else
                    return retval;
            }
        }


        public override void Recalibrate()
        {
            shiftX = mDevice.CurrentJoystickState.X;
            shiftY = mDevice.CurrentJoystickState.Y;
            shiftZ = mDevice.CurrentJoystickState.Z;
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
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Error polling joystick: " + e.Message);
                    return false;
                }
            }
        }

    }
}