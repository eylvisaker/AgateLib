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
using System.Linq;
using System.Text;

namespace AgateLib.InputLib
{
	/// <summary>
	/// An enum containing the possible states of a POV hat on a joystick.
	/// </summary>
	public enum HatState
	{
		/// <summary>
		/// A value indicating the POV hat is not depressed.
		/// </summary>
		None,
		/// <summary>
		/// A value indicating the POV hat is pressed to the right.
		/// </summary>
		Right,
		/// <summary>
		/// A value indicating the POV hat is pressed to the upper right.
		/// </summary>
		UpRight,
		/// <summary>
		/// A value indicating the POV hat is pressed upwards.
		/// </summary>
		Up,
		/// <summary>
		/// A value indicating the POV hat is pressed up left.
		/// </summary>
		UpLeft,
		/// <summary>
		/// A value indicating the POV hat is pressed to the left.
		/// </summary>
		Left,
		/// <summary>
		/// A value indicating the POV hat is pressed down left.
		/// </summary>
		DownLeft,
		/// <summary>
		/// A value indicating the POV hat is pressed downwards.
		/// </summary>
		Down,
		/// <summary>
		/// A value indicating the POV hat is pressed down-right.
		/// </summary>
		DownRight,
	}
}
