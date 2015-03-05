using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgateLib.Drivers;

namespace AgateLib.UnitTests.Fakes
{
    public class FakeAudioFactory : IAudioFactory
    {
        public AudioLib.ImplementationBase.AudioImpl AudioImpl
        {
            get { throw new NotImplementedException(); }
        }

        public AudioLib.ImplementationBase.SoundBufferImpl CreateSoundBuffer(string filename)
        {
            throw new NotImplementedException();
        }

        public AudioLib.ImplementationBase.MusicImpl CreateMusic(string filename)
        {
            throw new NotImplementedException();
        }

        public AudioLib.ImplementationBase.MusicImpl CreateMusic(System.IO.Stream musicStream)
        {
            throw new NotImplementedException();
        }

        public AudioLib.ImplementationBase.SoundBufferSessionImpl CreateSoundBufferSession(AudioLib.SoundBufferSession owner, AudioLib.ImplementationBase.SoundBufferImpl buffer)
        {
            throw new NotImplementedException();
        }

        public AudioLib.ImplementationBase.SoundBufferImpl CreateSoundBuffer(System.IO.Stream inStream)
        {
            throw new NotImplementedException();
        }

        public AudioLib.ImplementationBase.StreamingSoundBufferImpl CreateStreamingSoundBuffer(System.IO.Stream input, AudioLib.SoundFormat format)
        {
            throw new NotImplementedException();
        }
    }
}
