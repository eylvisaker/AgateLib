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

		string SDL_JoystickNameForIndex(int device_index);

		Guid SDL_JoystickGetDeviceGUID(int device_index);

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

		void PreloadLibrary(string name);
	}
}
