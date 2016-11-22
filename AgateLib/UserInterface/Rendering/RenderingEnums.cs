﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Rendering
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
		Inherit,
		Initial,
		Left,
		Right,
		Center,
	}

	public enum Overflow
	{
		Visible,
		Initial = Visible,
		Hidden,
		Scroll,
		Auto,
		Inherit,
		Disallow,
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
		
		Border_Box = Border,
		Padding_Box = Padding,
		Content_Box = Content,
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
