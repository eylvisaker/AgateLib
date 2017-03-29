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
using AgateLib.AgateSDL.Sdl2;
using AgateLib.InputLib;
using AgateLib.InputLib.ImplementationBase;
using AgateLib.Quality;

namespace AgateLib.AgateSDL.Input
{
	public class XInputJoystick_SDL : IJoystickImpl
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

		public int AxisCount => 6;

		public int ButtonCount => 10;

		public int HatCount => 1;

		public string Name
		{
			get
			{
				var result = sdl.SDL_JoystickNameForIndex(joystickIndex);

				return result ?? "";
			}
		}

		public Guid Guid => sdl.SDL_JoystickGetDeviceGUID(joystickIndex);

		public double AxisThreshold { get; set; } = 0.04f;

		public bool PluggedIn => true;

		public bool GetButtonState(int buttonIndex)
		{
			// We add 4 here because SDL reports the first four buttons of an XInput
			// controller as belonging to the d-pad.
			var targetIndex = xinputButtonMap[buttonIndex] + 4;

			return buttons[targetIndex];
		}

		public double GetAxisValue(int axisIndex)
		{
			// Convert joystick coordinate to the agatelib coordinate system of -1..1.
			var value = sdl.SDL_JoystickGetAxis(joystick, axisIndex) / 32767.0;

			if (value < -1) value = -1;
			else if (value > 1) value = 1;

			if (Math.Abs(value) < AxisThreshold)
				value = 0;

			return value;
		}

		public HatState GetHatState(int hatIndex)
		{
			Require.True<IndexOutOfRangeException>(hatIndex == 0, "XInput only has one dpad.");

			HatState result = HatState.None;

			if (buttons[0]) result |= HatState.Up;
			if (buttons[1]) result |= HatState.Down;
			if (buttons[2]) result |= HatState.Left;
			if (buttons[3]) result |= HatState.Right;

			return result;
		}

		public void Recalibrate()
		{
		}

		public void Poll()
		{
			for (var i = 0; i < buttons.Length; i++)
				buttons[i] = sdl.SDL_JoystickGetButton(joystick, i) != 0;
		}
	}
}