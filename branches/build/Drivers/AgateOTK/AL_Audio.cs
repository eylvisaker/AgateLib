﻿using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.ImplementationBase;
using AgateLib.Drivers;
using OpenTK.Audio;

namespace AgateLib.DisplayLib.OpenGL
{
    class AL_Audio : AudioImpl 
    {
        AudioContext context;

        public static void Register()
        {
            Registrar.RegisterAudioDriver(new DriverInfo<AudioTypeID>(
                typeof(AL_Audio), AudioTypeID.OpenAL, "OpenAL with OpenTK 0.9.1", 300));
        }

        public override void Initialize()
        {
            context = new AudioContext();
        }
        public override void Dispose()
        {
            context.Dispose();
        }

        public override SoundBufferImpl CreateSoundBuffer(string filename)
        {
            return new AL_SoundBuffer(filename);
        }
        public override SoundBufferSessionImpl CreateSoundBufferSession(SoundBufferImpl buffer)
        {
            return new AL_SoundBufferSession((AL_SoundBuffer)buffer);
        }   
        public override MusicImpl CreateMusic(string filename)
        {
            return new AL_Music(filename);
        }

    }

    class AL_SoundBuffer : SoundBufferImpl
    {
        int buffer;
        double volume = 1.0;

        public AL_SoundBuffer(string filename)
        {
            using (AudioReader reader = new AudioReader(filename))
            {
                buffer = AL.GenBuffer();
                AL.BufferData(buffer, reader.ReadToEnd());
            }
        }
        public override void Dispose()
        {
            AL.DeleteBuffer(buffer);
        }

        public override double Volume
        {
            get
            {
                return volume;   
            }
            set
            {
                volume = value;
            }
        }

        public int Buffer
        {
            get { return buffer; }
        }
    }

    class AL_SoundBufferSession : SoundBufferSessionImpl
    {
        AL_SoundBuffer buffer;
        int source;
        double volume;
        double pan = 0;

        public AL_SoundBufferSession(AL_SoundBuffer buffer)
        {
            this.buffer = buffer;

            source = AL.GenSource();
            AL.Source(source, ALSourcei.Buffer, buffer.Buffer);

            Volume = buffer.Volume;
        }

        public override void Dispose()
        {
            AL.DeleteSource(source);
        }

        public override void Play()
        {
            AL.SourcePlay(source);
        }

        public override void Stop()
        {
            AL.SourceStop(source);
        }

        public override double Volume
        {
            get
            {
                return volume;
            }
            set
            {
                AL.Source(source, ALSourcef.Gain, (float)value);

                volume = value;
            }
        }

        public override double Pan
        {
            get
            {
                return pan;
            }
            set
            {
                // 2D pan is unsupported in OpenAL.   
            }
        }

        public override bool IsPlaying
        {
            get 
            { 
                int value;
                
                AL.GetSource(source, ALGetSourcei.SourceState, out value);

                if (value == (int)ALSourceState.Playing)
                    return true;
                else
                    return false;
            }
        }
    }

    class AL_Music : MusicImpl
    {
        int buffer;
        int source;
        double volume = 1.0;

        public AL_Music(string filename)
        {
            using (AudioReader reader = new AudioReader(filename))
            {
                buffer = AL.GenBuffer();
                source = AL.GenSource();

                AL.BufferData(buffer, reader.ReadToEnd());
                AL.Source(source, ALSourcei.Buffer, buffer);
            }
         
            OnSetLoop(true);
        }

        protected override void OnSetLoop(bool value)
        {
            AL.Source(source, ALSourceb.Looping, value);
        }

        public override void Dispose()
        {
            AL.DeleteSource(source);
            AL.DeleteBuffer(buffer);
        }

        public override void Play()
        {
            AL.SourcePlay(source);
        }

        public override void Stop()
        {
            AL.SourceStop(source);
        }

        public override double Volume
        {
            get
            {
                return volume;
            }
            set
            {
                AL.Source(source, ALSourcef.Gain, (float)value);
                volume = value;
            }
        }

        public override double Pan
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public override bool IsPlaying
        {
            get 
            {
                int state;
                AL.GetSource(source, ALGetSourcei.SourceState, out state);

                if (state == (int)ALSourceState.Playing)
                    return true;
                else
                    return false;
            }
        }
    }
}