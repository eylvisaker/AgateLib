using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2.ThirtyTwo;

namespace AgateLib.SDL.Sdl2
{
	class SDL32 : ISDL
	{
		SDLMixer32 mixer = new SDLMixer32();

		ISDLMixer ISDL.Mixer
		{
			get { return mixer; }
		}


		public void SDL_QuitSubSystem(uint p)
		{
			SDL2.ThirtyTwo.SDL.SDL_QuitSubSystem(p);
		}
	}

	class SDLMixer32 : ISDLMixer
	{

		public void Mix_CloseAudio()
		{
			SDL_mixer.Mix_CloseAudio();
		}
	}
}
