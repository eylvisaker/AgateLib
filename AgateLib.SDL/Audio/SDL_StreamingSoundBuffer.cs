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
