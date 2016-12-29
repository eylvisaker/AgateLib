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
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using AgateLib;
using AgateLib.AudioLib.ImplementationBase;
using AgateLib.AgateSDL.Sdl2;

namespace AgateLib.AgateSDL.Audio
{
	class SDL_SoundBuffer : SoundBufferImpl
	{
		ISDL sdl;
		IntPtr sound;
		string tempfile;
		double mVolume = 1.0;
		int samplesPerSec = 22050;
		string filename;

		public SDL_SoundBuffer(Stream stream)
		{
			throw new NotImplementedException();
			//tempfile = AgateFileProvider.SaveStreamToTempFile(stream);
			//this.filename = tempfile;

			//LoadFromFile(tempfile);

			//(AgateLib.AudioLib.Audio.Impl as SDL_Audio).RegisterTempFile(tempfile);

		}
		public SDL_SoundBuffer(string filename)
		{
			sdl = SdlFactory.CreateSDL();

			this.filename = IO.Assets.Sounds.ResolveFile(filename);
			LoadFromFile(this.filename);
		}


		~SDL_SoundBuffer()
		{
			Dispose(false);
		}

		public string Filename { get { return filename; } }

		public int SamplePerSec
		{
			get { return samplesPerSec; }
		}

        protected override void Dispose(bool disposing)
		{
			sdl.Mixer.Mix_FreeChunk(sound);
			//if (string.IsNullOrEmpty(tempfile) == false)
			//{
			//    File.Delete(tempfile);
			//    tempfile = "";
			//}

            base.Dispose(disposing);
		}
		private void LoadFromFile(string file)
		{
			sound = sdl.Mixer.Mix_LoadWAV(file);

			if (sound == IntPtr.Zero)
			{
				var error = sdl.Mixer.GetError();

				throw new AgateException("Could not load audio file:" + error);
			}
		}

		public override double Volume
		{
			get
			{
				return mVolume;
			}
			set
			{
				if (value < 0.0) mVolume = 0.0;
				else if (value > 1.0) mVolume = 1.0;
				else mVolume = value;
			}
		}

		public override bool Loop
		{
			get;
			set;
		}
		internal IntPtr SoundChunk
		{
			get { return sound; }
		}

	}
}
