using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.InputLib.GamepadModel
{
	/// <summary>
	/// Buttons used in the game pad model.
	/// </summary>
	public enum GamepadButton
	{
		A,
		B,
		X,
		Y,
		LeftBumper,
		RightBumper,
		Back,
		Start,
		LeftStickButton,
		RightStickButton,
	}

	/// <summary>
	/// Enum for mapping keyboard keys to gamepad input.
	/// </summary>
	public enum KeyMapType
	{
		LeftStick,
		RightStick,
		Button,
		DPad,
	}

	/// <summary>
	/// Axes for the gamepad sticks.
	/// </summary>
	public enum StickAxis
	{
		X,
		Y,
	}
}
