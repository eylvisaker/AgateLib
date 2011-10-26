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
