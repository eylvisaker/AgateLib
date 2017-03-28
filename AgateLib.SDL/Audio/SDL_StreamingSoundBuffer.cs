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

using AgateLib.AgateSDL.Sdl2;
using AgateLib.AudioLib;
using AgateLib.AudioLib.ImplementationBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.AgateSDL.Audio
{
	class SDL_StreamingSoundBuffer : StreamingSoundBufferImpl
	{
		SDL_Audio mAudio;
		ISDL mSdl;

		Stream mInput;
		SoundFormat mFormat;
		bool mIsPlaying;

		public SDL_StreamingSoundBuffer(SDL_Audio audio, Stream input, SoundFormat format)
		{
			mAudio = audio;
			mInput = input;
			mFormat = format;
			mSdl = SdlFactory.CreateSDL();

			mSdl.Mixer.Mix_HookMusic(MixFunction, IntPtr.Zero);
			Play();
			ChunkSize = 2048;
		}
		public override void Dispose()
		{
			mSdl.Mixer.Mix_HookMusic(null, IntPtr.Zero);
		}

		public override void Play()
		{
			mIsPlaying = true;
		}

		public override void Stop()
		{
			mIsPlaying = false;
		}


		byte[] buffer = new byte[100];

		void MixFunction(IntPtr udata, IntPtr stream, int len)
		{
			if (buffer.Length < len)
				buffer = new byte[len];

			if (IsPlaying)
				mInput.Read(buffer, 0, len);
			else
				Array.Clear(buffer, 0, len);

			Marshal.Copy(buffer, 0, stream, len);
		}

		public override int ChunkSize { get; set; }

		public override bool IsPlaying
		{
			get { return mIsPlaying; }
		}

		public override double Pan { get; set; }

	}
}
