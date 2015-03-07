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
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Documents
{
	public enum CssDisplay
	{
		Initial,
		None,
		Block,
	}

	public enum CssTransitionType
	{
		None,
		Slide,
	}
	public enum CssTransitionDirection
	{
		Left,
		Right,
		Top,
		Bottom,
	}

	public enum CssLayoutKind
	{
		Flow,
		Column,
		Row,
		Grid,
	}

	public enum DistanceUnit
	{
		Pixels,
		Percent,
		FontHeight,
		FontAverageWidth,
		FontNumericWidth,
		ViewportWidthFrac,
		ViewportHeightFrac,
		ViewportMinFrac,
		ViewportMaxFrac,
	}

	public enum CssBorderImageRepeat
	{
		Initial,
		Inherit,
		Stretch,
		Repeat,
		Round,
	}

	public enum CssBorderStyle
	{
		None,
		Single,
		Double,
	}
	public enum CssBackgroundRepeat
	{
		Repeat,
		Repeat_X,
		Repeat_Y,
		Space,
		Round,
		No_Repeat,
	}
	public enum CssBackgroundClip
	{
		Border_Box,
		Padding_Box,
		Content_Box,
	}

	public enum CssPosition
	{
		Initial,
		Static = Initial,
		Relative,
		Absolute,
		Fixed,
	}

	public enum CssTextAlign
	{
		Inherit,
		Initial,
		Left,
		Right,
		Center,
	}

	public enum CssOverflow
	{
		Visible,
		Initial = Visible,
		Hidden,
		Scroll,
		Auto,
		Inherit,
        Disallow,
	}
}
