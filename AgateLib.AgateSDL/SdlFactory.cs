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

namespace AgateLib.AgateSDL
{
	public class AgateSdlFactory : IAudioFactory, IInputFactory
	{
		#region --- Audio Factory ---

		SDL_Audio mAudioImpl;


		public AudioImpl AudioImpl
		{
			get
			{
				if (mAudioImpl == null)
					mAudioImpl = new SDL_Audio();
				return mAudioImpl;
			}
		}

		public MusicImpl CreateMusic(string filename)
		{
			return new SDL_Music(mAudioImpl, filename);
		}
		public MusicImpl CreateMusic(System.IO.Stream musicStream)
		{
			return new SDL_Music(mAudioImpl, musicStream);
		}

		public SoundBufferImpl CreateSoundBuffer(string filename)
		{
			return new SDL_SoundBuffer(filename);
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

		public InputLib.ImplementationBase.InputImpl CreateJoystickInputImpl()
		{
			return new AgateLib.AgateSDL.Input.SDL_Input();
		}

		#endregion
	}
}
