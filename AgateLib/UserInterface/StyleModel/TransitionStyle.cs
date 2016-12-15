using System;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.StyleModel
{
	public class TransitionStyle
	{
		public TransitionDirection Direction { get; set; }

		public WindowTransitionType Type { get; set; }

		public double Time { get; set; }
	}
}