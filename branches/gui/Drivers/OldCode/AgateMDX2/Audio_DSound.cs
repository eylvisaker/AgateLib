using System;
using System.Collections.Generic;
using System.Text;
using DirectSound = Microsoft.DirectX.DirectSound;
using AV = Microsoft.DirectX.AudioVideoPlayback;

namespace ERY.GameLibrary
{
    /*
    class DSound_Audio : Audio 
    {
        DirectSound.Device mDevice;

        
        public override bool Initialize(System.Windows.Forms.Control parent)
        {
            mDevice = new Microsoft.DirectX.DirectSound.Device();

            mDevice.SetCooperativeLevel(parent.Handle,
                 Microsoft.DirectX.DirectSound.CooperativeLevel.Normal);

            return true;
        }
        public override Sound CreateSound(string filename)
        {
            return new DSound_Sound(this, filename);
        }

        public override Music CreateMusic(string filename)
        {
            return new DSound_Music(this, filename);
        }

        public DirectSound.Device Device
        {
            get { return mDevice; }
        }

    }

    class DSound_Sound : Sound
    {
        DSound_Audio mOwner;
        DirectSound.Buffer mBuffer;

        string mFilename;

        public DSound_Sound(DSound_Audio owner, string filename)
        {
            mOwner = owner;
            mFilename = filename;

            mBuffer = new Microsoft.DirectX.DirectSound.Buffer(mOwner.Device, filename);
        }
        public override void Play()
        {
            mBuffer.Play();

            mOwner.EventStopAllSounds += new EventHandler(mOwner_EventStopAllSounds);
        }

        void mOwner_EventStopAllSounds(object sender, EventArgs e)
        {
            Stop();
        }

        public override void Stop()
        {
            mBuffer.Stop();

            mOwner.EventStopAllSounds -= mOwner_EventStopAllSounds;
        }

        public override string Filename
        {
            get { return mFilename; }
        }

        public override double Volume
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
    }
    class DSound_Music : Music
    {
        DSound_Audio mOwner;
        AV.Audio mAudio;

        string mFilename;

        public DSound_Music(DSound_Audio owner, string filename)
        {
            mOwner = owner;
            mFilename = filename;

            mAudio = new Microsoft.DirectX.AudioVideoPlayback.Audio(filename);
        }

        protected override void OnSetLoop(bool value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Play()
        {
            mAudio.Play();

            mOwner.EventStopAllMusic += new EventHandler(mOwner_EventStopAllMusic);
        }

        void mOwner_EventStopAllMusic(object sender, EventArgs e)
        {
            Stop();
        }

        public override void Stop()
        {
            mAudio.Stop();

            mOwner.EventStopAllMusic -= mOwner_EventStopAllMusic;
        }


        public override string Filename
        {
            get { return mFilename; }
        }

        public override double Volume
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
    }
    */
}