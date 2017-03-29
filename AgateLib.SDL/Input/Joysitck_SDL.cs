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
using AgateLib.AgateSDL.Sdl2;
using AgateLib.InputLib;
using AgateLib.InputLib.ImplementationBase;

namespace AgateLib.AgateSDL.Input
{
	public sealed class Joystick_SDL : IJoystickImpl
	{
		private readonly ISDL sdl;

		private readonly IntPtr joystick;
		private readonly int joystickIndex;
		private readonly bool[] buttons;

		private int buttonCount = -1;

		public Joystick_SDL(int index)
		{
			sdl = SdlFactory.CreateSDL();

			joystickIndex = index;
			joystick = sdl.SDL_JoystickOpen(index);
			buttons = new bool[ButtonCount];
		}

		public string Name
		{
			get
			{
				var result = sdl.SDL_JoystickNameForIndex(joystickIndex);

				return result ?? "";
			}
		}

		public Guid Guid => sdl.SDL_JoystickGetDeviceGUID(joystickIndex);

		public int AxisCount => sdl.SDL_JoystickNumAxes(joystick);

		public int HatCount => sdl.SDL_JoystickNumHats(joystick);

		public double AxisThreshold { get; set; } = 0.04f;

		public int ButtonCount
		{
			get
			{
				if (buttonCount == -1)
					buttonCount = sdl.SDL_JoystickNumButtons(joystick);

				return buttonCount;
			}
		}

		public bool PluggedIn => true;

		public bool GetButtonState(int buttonIndex)
		{
			return buttons[buttonIndex];
		}

		public HatState GetHatState(int hatIndex)
		{
			switch (sdl.SDL_JoystickGetHat(joystick, hatIndex))
			{
				case SDLConstants.SDL_HAT_RIGHTUP:
					return HatState.UpRight;
				case SDLConstants.SDL_HAT_RIGHT:
					return HatState.Right;
				case SDLConstants.SDL_HAT_RIGHTDOWN:
					return HatState.DownRight;
				case SDLConstants.SDL_HAT_LEFTUP:
					return HatState.UpLeft;
				case SDLConstants.SDL_HAT_LEFT:
					return HatState.Left;
				case SDLConstants.SDL_HAT_LEFTDOWN:
					return HatState.DownLeft;
				case SDLConstants.SDL_HAT_DOWN:
					return HatState.Down;
				case SDLConstants.SDL_HAT_UP:
					return HatState.Up;

				case SDLConstants.SDL_HAT_CENTERED:
				default:
					return HatState.None;
			}
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

		public void Poll()
		{
			for (var i = 0; i < ButtonCount; i++)
				buttons[i] = sdl.SDL_JoystickGetButton(joystick, i) != 0;
		}

		public void Recalibrate()
		{
		}
	}
}