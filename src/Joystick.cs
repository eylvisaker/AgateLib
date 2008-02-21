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
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib
{
    /// <summary>
    /// Class which encapsulates a single joystick.
    /// </summary>
    public class Joystick
    {
        /// <summary>
        /// AxisList is a class which simplifies access to the axes
        /// of a joystick, by providing an list syntax.
        /// </summary>
        public class AxisList
        {
            Joystick owner;

            internal AxisList(Joystick owner)
            {
                this.owner = owner;
            }

            /// <summary>
            /// The total number of axes for the joystick.
            /// </summary>
            public int Count
            {
                get { return owner.AxisCount; }
            }
            /// <summary>
            /// Gets the current value for the given axis.
            /// Axis 0 is the x-axis, axis 1 is the y-axis.
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public double this[int index]
            {
                get { return owner.GetAxisValue(index); }
            }
        }

        JoystickImpl impl;
        AxisList axes;

        internal Joystick(JoystickImpl i)
        {
            impl = i;
            axes = new AxisList(this);
    
            AxisThreshold = 0.02;
        }

        /// <summary>
        /// Gets how many axes are available on this joystick.
        /// </summary>
        public int AxisCount { get { return impl.AxisCount; } }
        /// <summary>
        /// Returns the number of buttons this joystick has.
        /// </summary>
        public int ButtonCount { get { return impl.ButtonCount; } }

        /// <summary>
        /// Returns a list of the axes that are available on this joystick.
        /// </summary>
        public AxisList Axes { get { return axes; } }

        /// <summary>
        /// Returns an array indicating whether or not the joystick buttons
        /// are pushed.
        /// </summary>
        public bool[] Buttons { get { return impl.Buttons; } }

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
        /// Returns the value of the gamepad x-axis.
        /// Ranges are:
        /// -1 all the way to the left
        ///  0 centered
        ///  1 all the way to the right
        /// 
        /// Values outside this range may be returned.
        /// Never do tests which expect exact return values.
        /// Even gamepads will sometimes return values close to 1
        /// when pushed down, instead of exactly 1.
        /// 
        /// </summary>
        public double Xaxis
        {
            get { return impl.Xaxis; }
        }
        /// <summary>
        /// Returns the value of the gamepad y-axis.
        /// Ranges are:
        /// -1 all the way to the top
        ///  0 centered
        ///  1 all the way to the bottom
        /// 
        /// Values outside this range may be returned.
        /// Never do tests which expect exact return values.
        /// Even gamepads will sometimes return values close to 1
        /// when pushed down, instead of exactly 1.
        /// 
        /// </summary>
        public double Yaxis
        {
            get { return impl.Yaxis; }
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
        }
    }
}
