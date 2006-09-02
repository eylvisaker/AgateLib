using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.ImplBase
{
    /// <summary>
    /// Class which implements a Joystick.
    /// </summary>
    public abstract class JoystickImpl
    {
        /// <summary>
        /// Gets how many axes are on this joystick.
        /// </summary>
        public abstract int AxisCount { get; }
        /// <summary>
        /// Gets how many buttons are on this joystick.
        /// </summary>
        public abstract int ButtonCount { get; }

        /// <summary>
        /// Gets a bool array indicating state of the buttons.
        /// </summary>
        public abstract bool[] Buttons { get; }
        /// <summary>
        /// Returns the value of the gamepad x-axis.
        /// Ranges are:
        /// -1 all the way to the left
        ///  0 centered
        ///  1 all the way to the right
        /// </summary>
        public abstract double Xaxis { get; }
        /// <summary>
        /// Returns the value of the gamepad y-axis.
        /// Ranges are:
        /// -1 all the way to the top
        ///  0 centered
        ///  1 all the way to the bottom
        /// </summary>
        public abstract double Yaxis { get; }

        /// <summary>
        /// Recalibrates the joystick.
        /// </summary>
        public abstract void Recalibrate();

        /// <summary>
        /// Need documentation.
        /// </summary>
        public abstract double AxisThreshold { get; set; }

        /// <summary>
        /// Gets whether or not this joystick is plugged in.
        /// </summary>
        public abstract bool PluggedIn { get; }

        /// <summary>
        /// Polls the joystick for input.
        /// </summary>
        public abstract void Poll();

        /// <summary>
        /// Gets the current value for the given axis.
        /// Axis 0 is always the x-axis, axis 1 is always the y-axis on
        /// controlers which have this capability.
        /// </summary>
        /// <param name="axisIndex"></param>
        /// <returns></returns>
        public abstract double GetAxisValue(int axisIndex);
    }

    /// <summary>
    /// Implements an imaginary joystick that does nothing.
    /// </summary>
    public class NullJoystickImpl : JoystickImpl
    {
        bool[] mButtons = new bool[128];
        /// <summary>
        /// 
        /// </summary>
        public override int AxisCount
        {
            get { return 2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public override int ButtonCount
        {
            get { return mButtons.Length; }
        }
        /// <summary>
        /// 
        /// </summary>
        public override bool[] Buttons
        {
            get { return mButtons; }
        }
        /// <summary>
        /// 
        /// </summary>
        public override double Xaxis
        {
            get { return 0; }
        }
        /// <summary>
        /// 
        /// </summary>
        public override double Yaxis
        {
            get { return 0; }
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Recalibrate()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public override double AxisThreshold
        {
            get
            {
                return 0;
            }
            set
            {

            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override bool PluggedIn
        {
            get { return false; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="axisIndex"></param>
        /// <returns></returns>
        public override double GetAxisValue(int axisIndex)
        {
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Poll()
        {

        }
    }
}