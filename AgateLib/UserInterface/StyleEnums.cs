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
using System.Threading.Tasks;

namespace AgateLib.UserInterface
{
	public enum WindowTransitionType
	{
		None,
		Slide,
	}

	public enum TransitionDirection
	{
		Left,
		Right,
		Top,
		Bottom,
	}

	public enum TextAlign
	{
		Left,
		Right,
		Center,
	}

	public enum Overflow
	{
		Visible,
		Hidden,
		Scroll,
	}

	public enum BackgroundClip
	{
		/// <summary>
		/// Specifies the background should be drawn out to and behind the border.
		/// </summary>
		Border,
		/// <summary>
		/// Specifies the background should be drawn within the padding box.
		/// </summary>
		Padding,
		/// <summary>
		/// Specifies the background should only be drawn within the content box.
		/// </summary>
		Content,
	}

	public enum BackgroundRepeat
	{
		Repeat,
		Both = Repeat,
		Repeat_X,
		Repeat_Y,
		Space,
		Round,
		No_Repeat,
		None = No_Repeat,
	}
}
