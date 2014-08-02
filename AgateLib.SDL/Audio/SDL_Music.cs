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
using System.IO;
using System.Text;
using AgateLib;
using AgateLib.AudioLib.ImplementationBase;
using SDL2;

namespace AgateLib.SDL.Audio
{
	public class SDL_Music : MusicImpl
	{
		SDL_Audio audio;
		IntPtr music;
		string tempfile;
		double mVolume;

		public SDL_Music(SDL_Audio audio, Stream stream)
		{
			this.audio = audio;

			throw new NotImplementedException();
			//tempfile = audio.FileProvider.SaveStreamToTempFile(stream);

			LoadFromFile(tempfile);

			audio.RegisterTempFile(tempfile);
		}

		public SDL_Music(SDL_Audio audio, string filename)
		{
			this.audio = audio;

			LoadFromFile(filename);
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
			SDL_mixer.Mix_FreeMusic(music);

			//if (string.IsNullOrEmpty(tempfile) == false)
			//{
			//    File.Delete(tempfile);
			//    tempfile = "";
			//}
		}

		private void LoadFromFile(string file)
		{
			music = SDL_mixer.Mix_LoadMUS(file);

			if (music == IntPtr.Zero)
				throw new AgateException("Could not load music file.");
		}

		public override bool IsPlaying
		{
			get
			{
				if (SDL_mixer.Mix_PlayingMusic() == 0)
					return false;

				if (SDL_mixer.Mix_PausedMusic() != 0)
					return false;

				return true;
			}
		}

		protected override void OnSetLoop(bool value)
		{
			SDL_mixer.Mix_PlayMusic(music, IsLooping ? -1 : 1);
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
			SDL_mixer.Mix_PlayMusic(music, IsLooping ? -1 : 1);
			mVolume = SDL_mixer.Mix_VolumeMusic(-1) / (double)SDL_mixer.MIX_MAX_VOLUME;
		}

		public override void Stop()
		{
			SDL_mixer.Mix_PauseMusic();
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

				int v = (int)(mVolume * SDL_mixer.MIX_MAX_VOLUME);

				SDL_mixer.Mix_VolumeMusic(v);
			}
		}
	}
}
