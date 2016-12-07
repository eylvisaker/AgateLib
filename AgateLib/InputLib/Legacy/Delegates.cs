using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.InputLib.Legacy
{
	/// <summary>
	/// Input Event handler event type.
	/// </summary>
	/// <param name="e"></param>
	[Obsolete("Use AgateInputEventHandler instead.")]
	public delegate void InputEventHandler(InputEventArgs e);

	public delegate void JoystickEventHandler(object sender, JoystickEventArgs e);
}
