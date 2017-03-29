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
using System.Diagnostics;
using System.Text;
using AgateLib.Drivers;
using AgateLib.InputLib.ImplementationBase;
using AgateLib.AgateSDL.Sdl2;
using AgateLib.Diagnostics;

namespace AgateLib.AgateSDL.Input
{
	public class SDL_Input : InputImpl
	{
		private readonly ISDL sdl;

		public SDL_Input()
		{
			sdl = SdlFactory.CreateSDL();
		}

		~SDL_Input()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		
		public int JoystickCount => sdl.SDL_NumJoysticks();

		public IEnumerable<IJoystickImpl> CreateJoysticks()
		{
			for (int i = 0; i < JoystickCount; i++)
			{
				var name = sdl.SDL_JoystickNameForIndex(i);

				IJoystickImpl result;

				if (name.StartsWith("XInput"))
				{
					result = new XInputJoystick_SDL(i);
				}
				else
				{
					result = new Joystick_SDL(i);
				}

				Debug.Print($"Created joystick: {result.Guid} : {result.Name}");

				yield return result;
			}
		}

        protected void Dispose(bool disposing)
		{
			sdl.SDL_QuitSubSystem(SDLConstants.SDL_INIT_JOYSTICK);
		}

		public void Initialize()
		{
			// apparently initializing the video has some side-effect 
			// that is required for joysticks to work on windows (at least).
			if (sdl.SDL_InitSubSystem(SDLConstants.SDL_INIT_JOYSTICK | SDLConstants.SDL_INIT_VIDEO) != 0)
			{
				throw new AgateException("Failed to initialize SDL joysticks.");
			}
			
			sdl.SDL_SetHint("SDL_JOYSTICK_ALLOW_BACKGROUND_EVENTS", "1"); 

			Log.WriteLine("SDL driver version 2.0.3 instantiated for joystick input.");
		}

		public void Poll()
		{
			sdl.CallPollEvent();
		}
	}
}
