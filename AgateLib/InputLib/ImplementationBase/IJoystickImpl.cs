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
using System.Text;

namespace AgateLib.InputLib.ImplementationBase
{
	/// <summary>
	/// Class which implements a Joystick.
	/// </summary>
	public interface IJoystickImpl
	{
		/// <summary>
		/// Gets how many axes are on this joystick.
		/// </summary>
		int AxisCount { get; }
		/// <summary>
		/// Gets how many buttons are on this joystick.
		/// </summary>
		int ButtonCount { get; }
		/// <summary>
		/// Gets how many POV hats are on this joystick.
		/// </summary>
		int HatCount { get; }

		/// <summary>
		/// Gets the reported name of the joystick.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the GUID identifying the hardware of this joystick
		/// </summary>
		Guid Guid { get; }

		/// <summary>
		/// Gets the state of the specified button.  
		/// </summary>
		/// <param name="buttonIndex">Index of the button to check.  Valid values are
		/// from 0 to ButtonCount - 1.</param>
		/// <returns></returns>
		bool GetButtonState(int buttonIndex);

		/// <summary>
		/// Gets the currentFrame value for the given axis.
		/// Axis 0 is always the x-axis, axis 1 is always the y-axis on
		/// controllers which have this capability.
		/// </summary>
		/// <param name="axisIndex"></param>
		/// <returns></returns>
		double GetAxisValue(int axisIndex);

		/// <summary>
		/// Gets the state of the specified POV hat.
		/// </summary>
		/// <param name="hatIndex"></param>
		/// <returns></returns>
		HatState GetHatState(int hatIndex);

		/// <summary>
		/// Recalibrates the joystick.
		/// </summary>
		void Recalibrate();

		/// <summary>
		/// Need documentation.
		/// </summary>
		double AxisThreshold { get; set; }

		/// <summary>
		/// Gets whether or not this joystick is plugged in.
		/// </summary>
		bool PluggedIn { get; }

		/// <summary>
		/// Polls the joystick for input.
		/// </summary>
		void Poll();
	}
}