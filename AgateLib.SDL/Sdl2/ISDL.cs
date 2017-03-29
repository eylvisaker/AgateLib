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
