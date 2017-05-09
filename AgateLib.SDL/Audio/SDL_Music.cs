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
using System.IO;
using System.Text;
using AgateLib;
using AgateLib.AudioLib.ImplementationBase;
using AgateLib.AgateSDL.Sdl2;
using AgateLib.IO;

namespace AgateLib.AgateSDL.Audio
{
	public class SDL_Music : MusicImpl
	{
		ISDL sdl;
		SDL_Audio audio;
		IntPtr music;
		string filename;
		double mVolume = 1;

		public SDL_Music(SDL_Audio audio, Stream stream)
		{
			this.audio = audio;

			throw new NotImplementedException();
			//tempfile = audio.FileProvider.SaveStreamToTempFile(stream);

			//LoadFromFile(tempfile);

			//audio.RegisterTempFile(tempfile);
		}

		public SDL_Music(SDL_Audio audio, string filename, IReadFileProvider fileProvider)
		{
			sdl = SdlFactory.CreateSDL();

			this.audio = audio;
			this.filename = fileProvider.ResolveFile(filename);
			LoadFromFile(this.filename);
		}

		~SDL_Music()
		{
			Dispose(false);
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			sdl.Mixer.Mix_FreeMusic(music);

			//if (string.IsNullOrEmpty(tempfile) == false)
			//{
			//    File.Delete(tempfile);
			//    tempfile = "";
			//}
		}

		private void LoadFromFile(string file)
		{
			music = sdl.Mixer.Mix_LoadMUS(file);

			if (music == IntPtr.Zero)
				throw new AgateException("Could not load music file.");
		}

		public override bool IsPlaying
		{
			get
			{
				if (sdl.Mixer.Mix_PlayingMusic() == 0)
					return false;

				if (sdl.Mixer.Mix_PausedMusic() != 0)
					return false;

				return true;
			}
		}

		protected override void OnSetLoop(bool value)
		{
			sdl.Mixer.Mix_PlayMusic(music, IsLooping ? -1 : 1);
		}

		public override double Pan
		{
			get
			{
				return 0;
			}
			set
			{

			}
		}

		public override void Play()
		{
			sdl.Mixer.Mix_PlayMusic(music, IsLooping ? -1 : 1);

			SetMixerVolume();
		}

		public override void Stop()
		{
			sdl.Mixer.Mix_PauseMusic();
		}

		public override double Volume
		{
			get
			{
				return mVolume;
			}
			set
			{
				mVolume = value;
				if (mVolume < 0) mVolume = 0;
				if (mVolume > 1) mVolume = 1;

				SetMixerVolume();
			}
		}

		private void SetMixerVolume()
		{
			int v = (int)(mVolume * AudioLib.Audio.Configuration.MusicVolume * SDLConstants.MIX_MAX_VOLUME);

			sdl.Mixer.Mix_VolumeMusic(v);
		}
	}
}
