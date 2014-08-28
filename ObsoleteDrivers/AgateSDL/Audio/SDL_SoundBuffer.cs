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
using System.Runtime.InteropServices;
using System.Text;
using AgateLib;
using AgateLib.AudioLib.ImplementationBase;
using Tao.Sdl;

namespace AgateSDL.Audio
{
	class SDL_SoundBuffer : SoundBufferImpl
	{
		IntPtr sound;
		string tempfile;
		double mVolume = 1.0;
		int samplesPerSec = 22050;
		string filename;

		public SDL_SoundBuffer(Stream stream)
		{
			tempfile = AgateFileProvider.SaveStreamToTempFile(stream);
			this.filename = tempfile;

			LoadFromFile(tempfile);

			(AgateLib.AudioLib.Audio.Impl as SDL_Audio).RegisterTempFile(tempfile);

		}
		public SDL_SoundBuffer(string filename)
		{
			this.filename = filename;
			LoadFromFile(filename);
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

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			SdlMixer.Mix_FreeChunk(sound);
			//if (string.IsNullOrEmpty(tempfile) == false)
			//{
			//    File.Delete(tempfile);
			//    tempfile = "";
			//}
		}
		private void LoadFromFile(string file)
		{
			sound = SdlMixer.Mix_LoadWAV(file);

			if (sound == IntPtr.Zero)
				throw new AgateException("Could not load audio file.");
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
