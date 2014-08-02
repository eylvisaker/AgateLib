using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.SDL.Sdl2
{

	class SDL64: ISDL
	{
		SDLMixer64 mixer = new SDLMixer64();

		ISDLMixer ISDL.Mixer
		{
			get { return mixer; }
		}
	}

	class SDLMixer64 : ISDLMixer
	{

		public void Mix_CloseAudio()
		{
			SDL2.SixtyFour.SDL_mixer.Mix_CloseAudio();
		}
	}
}
