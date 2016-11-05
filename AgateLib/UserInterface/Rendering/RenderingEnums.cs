using System;
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
		Border_Box,
		Padding_Box,
		Content_Box,
	}

	public enum BackgroundRepeat
	{
		Repeat,
		Repeat_X,
		Repeat_Y,
		Space,
		Round,
		No_Repeat,
	}
}
