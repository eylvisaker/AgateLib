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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//

using System;
using AgateLib.AgateSDL.Sdl2;
using AgateLib.InputLib;
using AgateLib.InputLib.ImplementationBase;

namespace AgateLib.AgateSDL.Input
{
	public sealed class Joystick_SDL : JoystickImpl
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

		public override string Name
		{
			get
			{
				var result = sdl.SDL_JoystickNameForIndex(joystickIndex);

				return result ?? "";
			}
		}

		public override Guid Guid => sdl.SDL_JoystickGetDeviceGUID(joystickIndex);

		public override int AxisCount => sdl.SDL_JoystickNumAxes(joystick);

		public override int HatCount => sdl.SDL_JoystickNumHats(joystick);

		public override double AxisThreshold { get; set; } = 0.04f;

		public override int ButtonCount
		{
			get
			{
				if (buttonCount == -1)
					buttonCount = sdl.SDL_JoystickNumButtons(joystick);

				return buttonCount;
			}
		}

		public override bool PluggedIn => true;

		public override bool GetButtonState(int buttonIndex)
		{
			return buttons[buttonIndex];
		}

		public override HatState GetHatState(int hatIndex)
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

		public override void Poll()
		{
			for (var i = 0; i < ButtonCount; i++)
				buttons[i] = sdl.SDL_JoystickGetButton(joystick, i) != 0;
		}

		public override void Recalibrate()
		{
		}
	}
}