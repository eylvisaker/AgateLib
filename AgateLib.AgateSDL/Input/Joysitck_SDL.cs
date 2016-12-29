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
using AgateLib.AgateSDL.Sdl2;
using AgateLib.InputLib.ImplementationBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.AgateSDL.Input
{
	public class Joystick_SDL : JoystickImpl
	{
		ISDL sdl;

		IntPtr joystick;
		int joystickIndex;
		double axisTheshold = 0.04f;
		bool[] buttons;
		int buttonCount = -1;

		public Joystick_SDL(int index)
		{
			sdl = SdlFactory.CreateSDL();

			this.joystickIndex = index;
			this.joystick = sdl.SDL_JoystickOpen(index);
			buttons = new bool[ButtonCount];
		}

		public override string Name
		{
			get
			{
				string result = sdl.SDL_JoystickNameForIndex(joystickIndex);

				if (result == null)
					return "";

				return result;
			}
		}
		public override Guid Guid
		{
			get { return sdl.SDL_JoystickGetDeviceGUID(joystickIndex); }
		}

		public override int AxisCount
		{
			get { return sdl.SDL_JoystickNumAxes(joystick); }
		}
		public override int HatCount
		{
			get { return sdl.SDL_JoystickNumHats(joystick); }
		}

		public override double AxisThreshold
		{
			get
			{
				return axisTheshold;
			}
			set
			{
				axisTheshold = value;
			}
		}

		public override int ButtonCount
		{
			get
			{
				if (buttonCount == -1)
					buttonCount = sdl.SDL_JoystickNumButtons(joystick);

				return buttonCount;
			}
		}

		public override bool GetButtonState(int buttonIndex)
		{
			return buttons[buttonIndex];
		}
		public override AgateLib.InputLib.HatState GetHatState(int hatIndex)
		{
			switch (sdl.SDL_JoystickGetHat(joystick, hatIndex))
			{
				case SDLConstants.SDL_HAT_RIGHTUP: return AgateLib.InputLib.HatState.UpRight;
				case SDLConstants.SDL_HAT_RIGHT: return AgateLib.InputLib.HatState.Right;
				case SDLConstants.SDL_HAT_RIGHTDOWN: return AgateLib.InputLib.HatState.DownRight;
				case SDLConstants.SDL_HAT_LEFTUP: return AgateLib.InputLib.HatState.UpLeft;
				case SDLConstants.SDL_HAT_LEFT: return AgateLib.InputLib.HatState.Left;
				case SDLConstants.SDL_HAT_LEFTDOWN: return AgateLib.InputLib.HatState.DownLeft;
				case SDLConstants.SDL_HAT_DOWN: return AgateLib.InputLib.HatState.Down;
				case SDLConstants.SDL_HAT_UP: return AgateLib.InputLib.HatState.Up;

				case SDLConstants.SDL_HAT_CENTERED:
				default:
					return AgateLib.InputLib.HatState.None;
			}
		}
		public override double GetAxisValue(int axisIndex)
		{
			// Convert joystick coordinate to the agatelib coordinate system of -1..1.
			double value = sdl.SDL_JoystickGetAxis(joystick, axisIndex) / 32767.0;

			if (value < -1) value = -1;
			else if (value > 1) value = 1;

			if (Math.Abs(value) < AxisThreshold)
				value = 0;

			return value;
		}

		public override bool PluggedIn
		{
			get { return true; }
		}

		public override void Poll()
		{
			for (int i = 0; i < ButtonCount; i++)
			{
				buttons[i] = (sdl.SDL_JoystickGetButton(joystick, i) != 0) ? true : false;
			}
		}

		public override void Recalibrate()
		{

		}
	}
}
