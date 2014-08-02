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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using AgateLib.Drivers;
using AgateLib.InputLib.ImplementationBase;

namespace AgateLib.SDL.Input
{
	public class SDL_Input : InputImpl
	{
		public SDL_Input()
		{
		}
		public override int JoystickCount
		{
			get { return SDL2.SDL.SDL_NumJoysticks(); }
		}

		public override IEnumerable<JoystickImpl> CreateJoysticks()
		{
			for (int i = 0; i < JoystickCount; i++)
			{
				Debug.Print(SDL2.SDL.SDL_JoystickName((IntPtr)i));
				yield return new Joystick_SDL(i);
			}
		}

		public override void Dispose()
		{
			SDL2.SDL.SDL_QuitSubSystem(SDL2.SDL.SDL_INIT_JOYSTICK);
		}

		public override void Initialize()
		{
			// apparently initializing the video has some side-effect 
			// that is required for joysticks to work on windows (at least).
			if (SDL2.SDL.SDL_InitSubSystem(SDL2.SDL.SDL_INIT_JOYSTICK | SDL2.SDL.SDL_INIT_VIDEO) != 0)
			{
				throw new AgateLib.AgateException("Failed to initialize SDL joysticks.");
			}

			SDL2.SDL.SDL_version version;
			SDL2.SDL.SDL_VERSION(out version);

			Report("SDL driver version " + version.ToString() + " instantiated for joystick input.");

		}
	}

	public class Joystick_SDL : JoystickImpl
	{
		IntPtr joystick;
		IntPtr joystickIndex;
		double axisTheshold = 0.04f;
		bool[] buttons;

		public Joystick_SDL(int index)
		{
			this.joystickIndex = (IntPtr) index;
			this.joystick = SDL2.SDL.SDL_JoystickOpen(index);
			buttons = new bool[ButtonCount];
		}

		public override string Name
		{
			get
			{
				string retval = SDL2.SDL.SDL_JoystickName(joystickIndex);

				return retval;
			}
		}

		public override int AxisCount
		{
			get { return SDL2.SDL.SDL_JoystickNumAxes(joystick); }
		}
		public override int HatCount
		{
			get { return SDL2.SDL.SDL_JoystickNumHats(joystick); }
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
			get { return SDL2.SDL.SDL_JoystickNumButtons(joystick); }
		}

		public override bool GetButtonState(int buttonIndex)
		{
			return buttons[buttonIndex];
		}
		public override AgateLib.InputLib.HatState GetHatState(int hatIndex)
		{
			switch(SDL2.SDL.SDL_JoystickGetHat(joystick, hatIndex))
			{
				case SDL2.SDL.SDL_HAT_RIGHTUP: return AgateLib.InputLib.HatState.UpRight;
				case SDL2.SDL.SDL_HAT_RIGHT: return AgateLib.InputLib.HatState.Right;
				case SDL2.SDL.SDL_HAT_RIGHTDOWN: return AgateLib.InputLib.HatState.DownRight;
				case SDL2.SDL.SDL_HAT_LEFTUP: return AgateLib.InputLib.HatState.UpLeft;
				case SDL2.SDL.SDL_HAT_LEFT: return AgateLib.InputLib.HatState.Left;
				case SDL2.SDL.SDL_HAT_LEFTDOWN: return AgateLib.InputLib.HatState.DownLeft;
				case SDL2.SDL.SDL_HAT_DOWN: return AgateLib.InputLib.HatState.Down;
				case SDL2.SDL.SDL_HAT_UP: return AgateLib.InputLib.HatState.Up;

				case SDL2.SDL.SDL_HAT_CENTERED: 
				default:
					return AgateLib.InputLib.HatState.None;
			}
		}
		public override double GetAxisValue(int axisIndex)
		{
			// Convert joystick coordinate to the agatelib coordinate system of -1..1.
			double value = SDL2.SDL.SDL_JoystickGetAxis(joystick, axisIndex) / 32767.0;

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
			SDL2.SDL.SDL_Event evt;
			SDL2.SDL.SDL_PollEvent(out evt);

			for (int i = 0; i < ButtonCount; i++)
			{
				buttons[i] = (SDL2.SDL.SDL_JoystickGetButton(joystick, i) != 0) ? true : false;
			}
		}

		public override void Recalibrate()
		{

		}

	}
}
