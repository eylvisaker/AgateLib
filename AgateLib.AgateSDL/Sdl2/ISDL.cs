using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.AgateSDL.Sdl2
{
	interface ISDL
	{
		ISDLMixer Mixer { get; }

		void SDL_QuitSubSystem(uint flags);

		int SDL_InitSubSystem(uint flags);

		int SDL_NumJoysticks();

		string SDL_JoystickName(IntPtr joystick);

		IntPtr SDL_JoystickOpen(int index);

		int SDL_JoystickNumAxes(IntPtr joystick);

		int SDL_JoystickNumHats(IntPtr joystick);

		int SDL_JoystickNumButtons(IntPtr joystick);

		int SDL_JoystickGetHat(IntPtr joystick, int hatIndex);

		double SDL_JoystickGetAxis(IntPtr joystick, int axisIndex);

		int SDL_JoystickGetButton(IntPtr joystick, int i);

		void CallPollEvent();

		void SDL_SetHint(string p1, string p2);

		string GetError();

		void SDL_Init(uint flags);
	}
}
