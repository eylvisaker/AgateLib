using System;
using System.Collections.Generic;
using System.Text;
using SDL = SdlDotNet;

namespace ERY.GameLibrary
{
    /*
    class SDL_Audio : Audio 
    {
        public override bool Initialize(System.Windows.Forms.Control parent)
        {
            return true;
        }

        public override Sound CreateSound(string filename)
        {
            return new SDL_Sound(this, filename);
        }

        public override Music CreateMusic(string filename)
        {
            return new SDL_Music(this, filename);
        }

    }

    class SDL_Sound : Sound
    {
        SDL_Audio mOwner;
        SDL.Sound mSound;
        SDL.Channel mSoundChannel;

        string mFilename;

        public SDL_Sound(SDL_Audio owner, string filename)
        {
            mOwner = owner;
            mFilename = filename;

            mSound = new SdlDotNet.Sound(filename);


        }

        void mOwner_StopAllSounds(object sender, EventArgs e)
        {
            Stop();
        }

        public override void Play()
        {
            mSoundChannel = mSound.Play();

            mOwner.EventStopAllSounds += new EventHandler(mOwner_StopAllSounds);
        }

        public override void Stop()
        {
            mSound.Stop();

            mOwner.EventStopAllSounds -= mOwner_StopAllSounds;
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
    class SDL_Music : Music
    {
        SDL_Audio mOwner;
        SDL.Music mMusic;

        
        public SDL_Music(SDL_Audio owner, string filename)
        {
            mOwner = owner;

            mMusic = new SdlDotNet.Music(filename);

            
        }

        void mOwner_StopAllSounds(object sender, EventArgs e)
        { 

        }
        protected override void OnSetLoop(bool value)
        {
            
        }

        public override void Play()
        {
            mMusic.Play(true);

            mOwner.EventStopAllMusic += new EventHandler(mOwner_StopAllSounds);
        }

        public override void Stop()
        {
            SDL.Music old = mMusic;

            mMusic = new SdlDotNet.Music(Filename);
            
            old.Dispose();

            mOwner.EventStopAllMusic -= mOwner_StopAllSounds;
        }

        
        public override string Filename
        {
            get { return mMusic.FileName; }
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
