using AgateLib.AgateSDL.Audio;
using AgateLib.AudioLib;
using AgateLib.AudioLib.ImplementationBase;
using AgateLib.Drivers;
using AgateLib.Drivers.NullDrivers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AgateLib.AgateSDL.Input;
using AgateLib.InputLib.ImplementationBase;
using AgateLib.IO;

namespace AgateLib.AgateSDL
{
	public class AgateSdlFactory : IAudioFactory, IInputFactory
	{
		private SDL_Audio mAudioImpl;
		private SDL_Input inputImpl;

		#region --- Audio Factory ---


		public AudioImpl AudioCore
		{
			get
			{
				if (mAudioImpl == null)
					mAudioImpl = new SDL_Audio();
				return mAudioImpl;
			}
		}

		public MusicImpl CreateMusic(string filename, IReadFileProvider fileProvider)
		{
			return new SDL_Music(mAudioImpl, filename, fileProvider);
		}
		public MusicImpl CreateMusic(System.IO.Stream musicStream)
		{
			return new SDL_Music(mAudioImpl, musicStream);
		}

		public SoundBufferImpl CreateSoundBuffer(string filename, IReadFileProvider fileProvider)
		{
			return new SDL_SoundBuffer(filename, fileProvider);
		}
		public SoundBufferImpl CreateSoundBuffer(System.IO.Stream inStream)
		{
			return new SDL_SoundBuffer(inStream);
		}
		public SoundBufferSessionImpl CreateSoundBufferSession(SoundBufferSession owner, SoundBufferImpl buffer)
		{
			return new SDL_SoundBufferSession(owner, (SDL_SoundBuffer)buffer);
		}
		public StreamingSoundBufferImpl CreateStreamingSoundBuffer(Stream input, SoundFormat format)
		{
			return new SDL_StreamingSoundBuffer(mAudioImpl, input, format);
		}

		#endregion
		#region --- Input Factory ---


		public InputImpl InputCore
		{
			get
			{
				// Don't let SDL get initialized before the display object gets initialized!! 
				return inputImpl ?? (inputImpl = new SDL_Input());
			}
		}

		#endregion
	}
}
