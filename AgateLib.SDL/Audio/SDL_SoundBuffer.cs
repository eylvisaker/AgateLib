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
using System.Runtime.InteropServices;
using System.Text;
using AgateLib;
using AgateLib.AudioLib.ImplementationBase;
using AgateLib.AgateSDL.Sdl2;
using AgateLib.IO;

namespace AgateLib.AgateSDL.Audio
{
	class SDL_SoundBuffer : SoundBufferImpl
	{
		ISDL sdl;
		IntPtr sound;
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
		public SDL_SoundBuffer(string filename, IReadFileProvider fileProvider)
		{
			sdl = SdlFactory.CreateSDL();

			this.filename = fileProvider.ResolveFile(filename);
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
