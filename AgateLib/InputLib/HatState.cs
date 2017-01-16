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
using System.Linq;
using System.Text;

namespace AgateLib.InputLib
{
	/// <summary>
	/// An enum containing the possible states of a POV hat on a joystick.
	/// </summary>
	[Flags]
	public enum HatState
	{
		/// <summary>
		/// A value indicating the POV hat is not depressed.
		/// </summary>
		None,
		/// <summary>
		/// A value indicating the POV hat is pressed to the right.
		/// </summary>
		Right = 1,
		/// <summary>
		/// A value indicating the POV hat is pressed to the upper right.
		/// </summary>
		UpRight = 3,
		/// <summary>
		/// A value indicating the POV hat is pressed upwards.
		/// </summary>
		Up = 2,
		/// <summary>
		/// A value indicating the POV hat is pressed up left.
		/// </summary>
		UpLeft = 6,
		/// <summary>
		/// A value indicating the POV hat is pressed to the left.
		/// </summary>
		Left = 4,
		/// <summary>
		/// A value indicating the POV hat is pressed down left.
		/// </summary>
		DownLeft = 12,
		/// <summary>
		/// A value indicating the POV hat is pressed downwards.
		/// </summary>
		Down = 8,
		/// <summary>
		/// A value indicating the POV hat is pressed down-right.
		/// </summary>
		DownRight = 9,
	}
}
