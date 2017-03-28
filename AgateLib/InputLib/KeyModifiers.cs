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

namespace AgateLib.InputLib
{
	/// <summary>
	/// Structure which keeps track of modifier keys applied to
	/// other keys.
	/// </summary>
	public struct KeyModifiers
	{
		/// <summary>
		/// Constructs a KeyModifiers structure with the given
		/// state of the alt, control and shift keys.
		/// </summary>
		/// <param name="alt"></param>
		/// <param name="control"></param>
		/// <param name="shift"></param>
		public KeyModifiers(bool alt, bool control, bool shift)
		{
			Alt = alt;
			Control = control;
			Shift = shift;
		}
		/// <summary>
		/// Gets or sets the state of the Alt key.
		/// </summary>
		public bool Alt { get; set; }

		/// <summary>
		/// Gets or sets the state of the Control key.
		/// </summary>
		public bool Control { get; set; }

		/// <summary>
		/// Gets or sets the state of the Shift key.
		/// </summary>
		public bool Shift { get; set; }
	}
}
