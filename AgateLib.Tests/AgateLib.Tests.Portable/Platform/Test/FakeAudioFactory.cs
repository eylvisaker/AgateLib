using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgateLib.Drivers;
using AgateLib.AudioLib.ImplementationBase;
using AgateLib.Platform.Test.Audio;

namespace AgateLib.Platform.Test
{
    public class FakeAudioFactory : IAudioFactory
    {
        public FakeAudioFactory()
        {
            AudioImpl = new FakeAudioImpl();
        }

        public FakeAudioImpl AudioImpl { get; private set; }



        public SoundBufferImpl CreateSoundBuffer(string filename)
        {
            throw new NotImplementedException();
        }

        public MusicImpl CreateMusic(string filename)
        {
            throw new NotImplementedException();
        }

        public MusicImpl CreateMusic(System.IO.Stream musicStream)
        {
            throw new NotImplementedException();
        }

        public SoundBufferSessionImpl CreateSoundBufferSession(AudioLib.SoundBufferSession owner, SoundBufferImpl buffer)
        {
            throw new NotImplementedException();
        }

        public SoundBufferImpl CreateSoundBuffer(System.IO.Stream inStream)
        {
            throw new NotImplementedException();
        }

        public StreamingSoundBufferImpl CreateStreamingSoundBuffer(System.IO.Stream input, AudioLib.SoundFormat format)
        {
            throw new NotImplementedException();
        }

        AudioImpl IAudioFactory.AudioImpl
        {
            get { return AudioImpl; }
        }

    }
}
