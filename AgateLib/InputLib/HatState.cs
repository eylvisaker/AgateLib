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
