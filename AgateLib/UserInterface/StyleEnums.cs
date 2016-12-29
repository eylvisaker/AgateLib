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
