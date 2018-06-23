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
using Tao.Sdl;

namespace AgateSDL.Audio
{
	public class SDL_Music : MusicImpl
	{
		IntPtr music;
		string tempfile;
		double mVolume;

		public SDL_Music(Stream stream)
		{
			tempfile = AgateFileProvider.SaveStreamToTempFile(stream);

			LoadFromFile(tempfile);

			(AgateLib.AudioLib.Audio.Impl as SDL_Audio).RegisterTempFile(tempfile);
		}

		public SDL_Music(string filename)
		{
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
			SdlMixer.Mix_FreeMusic(music);

			//if (string.IsNullOrEmpty(tempfile) == false)
			//{
			//    File.Delete(tempfile);
			//    tempfile = "";
			//}
		}

		private void LoadFromFile(string file)
		{
			music = SdlMixer.Mix_LoadMUS(file);

			if (music == IntPtr.Zero)
				throw new AgateException("Could not load music file.");
		}

		public override bool IsPlaying
		{
			get
			{
				if (SdlMixer.Mix_PlayingMusic() == 0)
					return false;

				if (SdlMixer.Mix_PausedMusic() != 0)
					return false;

				return true;
			}
		}

		protected override void OnSetLoop(bool value)
		{
			SdlMixer.Mix_PlayMusic(music, IsLooping ? -1 : 1);
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
			SdlMixer.Mix_PlayMusic(music, IsLooping ? -1 : 1);
			mVolume = SdlMixer.Mix_VolumeMusic(-1) / (double)SdlMixer.MIX_MAX_VOLUME;
		}

		public override void Stop()
		{
			SdlMixer.Mix_PauseMusic();
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

				int v = (int)(mVolume * SdlMixer.MIX_MAX_VOLUME);

				SdlMixer.Mix_VolumeMusic(v);
			}
		}
	}
}
