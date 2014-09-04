using AgateLib.Drivers;
using AgateLib.AudioLib.ImplementationBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Platform.WindowsStore.AudioImplementation;
using System.IO;
using AgateLib.AudioLib;

namespace AgateLib.Platform.WindowsStore.Factories
{
	public class AudioFactoryDX : IAudioFactory
	{
		XAudio2_Audio mImpl;

		public AudioImpl CreateAudioImpl()
		{
			return AudioImpl;
		}

		public AudioImpl AudioImpl
		{
			get
			{
				if (mImpl == null)
					mImpl = new XAudio2_Audio();

				return mImpl;
			}
		}

		public SoundBufferImpl CreateSoundBuffer(Stream inStream)
		{
			return new XAudio2_SoundBuffer(mImpl, inStream);
		}

		public MusicImpl CreateMusic(System.IO.Stream musicStream)
		{
			return new XAudio2_Music(mImpl, musicStream);
		}
		public MusicImpl CreateMusic(string filename)
		{
			return new XAudio2_Music(mImpl, filename);
		}
		public SoundBufferImpl CreateSoundBuffer(string filename)
		{
			return new XAudio2_SoundBuffer(mImpl, filename);
		}
		public SoundBufferSessionImpl CreateSoundBufferSession(SoundBufferSession owner, SoundBufferImpl buffer)
		{
			return new XAudio2_SoundBufferSession(mImpl, buffer as XAudio2_SoundBuffer);
		}
		public StreamingSoundBufferImpl CreateStreamingSoundBuffer(Stream input, SoundFormat format)
		{
			return new XAudio2_StreamingSoundBuffer(mImpl, input, format);
		}
	}
}
