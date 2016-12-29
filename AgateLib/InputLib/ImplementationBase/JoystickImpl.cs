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

namespace AgateLib.InputLib.ImplementationBase
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
		/// Gets how many POV hats are on this joystick.
		/// </summary>
		public abstract int HatCount { get; }

		/// <summary>
		/// Gets the reported name of the joystick.
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// Gets the GUID identifying the hardware of this joystick
		/// </summary>
		public abstract Guid Guid { get; }

		/// <summary>
		/// Gets the state of the specified button.  
		/// </summary>
		/// <param name="buttonIndex">Index of the button to check.  Valid values are
		/// from 0 to ButtonCount - 1.</param>
		/// <returns></returns>
		public abstract bool GetButtonState(int buttonIndex);

		/// <summary>
		/// Gets the currentFrame value for the given axis.
		/// Axis 0 is always the x-axis, axis 1 is always the y-axis on
		/// controllers which have this capability.
		/// </summary>
		/// <param name="axisIndex"></param>
		/// <returns></returns>
		public abstract double GetAxisValue(int axisIndex);
		
		/// <summary>
		/// Gets the state of the specified POV hat.
		/// </summary>
		/// <param name="hatIndex"></param>
		/// <returns></returns>
		public abstract HatState GetHatState(int hatIndex);

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


	}
}