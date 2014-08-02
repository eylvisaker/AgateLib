using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.SDL.Sdl2
{
	interface ISDL
	{
		ISDLMixer Mixer { get; }

		void SDL_QuitSubSystem(uint p);
	}
}
