using System;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.Venus
{
	public class TransitionStyle : ITransitionStyle
	{
		public TransitionDirection Direction { get; set; }

		public WindowTransitionType Type { get; set; }

		public double Time { get; set; }
	}
}