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
