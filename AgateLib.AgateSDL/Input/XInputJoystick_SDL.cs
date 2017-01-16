using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib.AgateSDL.Sdl2;
using AgateLib.InputLib;
using AgateLib.InputLib.ImplementationBase;
using AgateLib.Quality;

namespace AgateLib.AgateSDL.Input
{
	public class XInputJoystick_SDL : JoystickImpl
	{
		private static Dictionary<int, int> xinputButtonMap = new Dictionary<int, int>
		{
			{0, 6},
			{1, 7},
			{2, 8}, {3, 9}, {4, 4}, {5, 5}, {6, 1}, {7, 0}, {8, 2}, {9, 3}
		};

		private readonly ISDL sdl;

		private IntPtr joystick;
		private readonly int joystickIndex;
		private bool[] buttons = new bool[14];

		public XInputJoystick_SDL(int index)
		{
			sdl = SdlFactory.CreateSDL();

			joystickIndex = index;
			joystick = sdl.SDL_JoystickOpen(index);
		}

		public override int AxisCount => 6;

		public override int ButtonCount => 10;

		public override int HatCount => 1;

		public override string Name
		{
			get
			{
				var result = sdl.SDL_JoystickNameForIndex(joystickIndex);

				return result ?? "";
			}
		}

		public override Guid Guid => sdl.SDL_JoystickGetDeviceGUID(joystickIndex);

		public override double AxisThreshold { get; set; } = 0.04f;

		public override bool PluggedIn => true;

		public override bool GetButtonState(int buttonIndex)
		{
			// We add 4 here because SDL reports the first four buttons of an XInput
			// controller as belonging to the d-pad.
			var targetIndex = xinputButtonMap[buttonIndex] + 4;

			return buttons[targetIndex];
		}

		public override double GetAxisValue(int axisIndex)
		{
			// Convert joystick coordinate to the agatelib coordinate system of -1..1.
			var value = sdl.SDL_JoystickGetAxis(joystick, axisIndex) / 32767.0;

			if (value < -1) value = -1;
			else if (value > 1) value = 1;

			if (Math.Abs(value) < AxisThreshold)
				value = 0;

			return value;
		}

		public override HatState GetHatState(int hatIndex)
		{
			Require.True<IndexOutOfRangeException>(hatIndex == 0, "XInput only has one dpad.");

			HatState result = HatState.None;

			if (buttons[0]) result |= HatState.Up;
			if (buttons[1]) result |= HatState.Down;
			if (buttons[2]) result |= HatState.Left;
			if (buttons[3]) result |= HatState.Right;

			return result;
		}

		public override void Recalibrate()
		{
		}

		public override void Poll()
		{
			for (var i = 0; i < buttons.Length; i++)
				buttons[i] = sdl.SDL_JoystickGetButton(joystick, i) != 0;
		}
	}
}