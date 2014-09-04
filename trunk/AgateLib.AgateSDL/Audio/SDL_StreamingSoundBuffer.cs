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
