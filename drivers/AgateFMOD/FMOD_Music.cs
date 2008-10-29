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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

using AgateLib;
using AgateLib.ImplBase;

namespace AgateLib.AgateFMOD
{
    class FMOD_Music : MusicImpl 
    {
        FMOD_Audio mAudio;
        FMOD.System mSystem;
        FMOD.Sound mSound;
        FMOD.Channel mChannel;

        bool mIsDisposed = false;


        public FMOD_Music(FMOD_Audio audio, string filename)
        {
            mAudio = audio;
            mSystem = mAudio.FMODSystem;

            FMOD.RESULT result = mSystem.createStream(filename, 
                FMOD.MODE.LOOP_NORMAL | FMOD.MODE.ACCURATETIME, ref mSound);

            CreateChannel();

        }
        ~FMOD_Music()
        {
            Dispose(false);
        }
        public override void Dispose()
        {
            Dispose(true); 
        }
        void Dispose(bool disposing)
        {
            if (mIsDisposed)
                return;

            if (disposing)
            {
                GC.SuppressFinalize(this);

                mSystem = null;
            }
            
            if (mAudio.IsDisposed == false)
                mSound.release();

            mAudio = null;
            mSound = null;

            mIsDisposed = true;
        }

        void CreateChannel()
        {
            FMOD_Audio.CheckFMODResult(
                mSystem.playSound(FMOD.CHANNELINDEX.FREE, mSound, true, ref mChannel));
        }

        public override void Play()
        {
            CreateChannel();
            FMOD_Audio.CheckFMODResult(mChannel.setPaused(false));
        }

        public override void Stop()
        {
            FMOD_Audio.CheckFMODResult(mChannel.setPaused(true));
            FMOD_Audio.CheckFMODResult(mChannel.setPosition(0, FMOD.TIMEUNIT.MS));
        }

        public override double Volume
        {
            get
            {
                float vol = 0.0f;
                FMOD_Audio.CheckFMODResult(mChannel.getVolume(ref vol));

                return vol;
            }
            set
            {
                FMOD_Audio.CheckFMODResult(mChannel.setVolume((float)value));
            }
        }

        public override double Pan
        {
            get
            {
                float pan = 0.0f;
                FMOD_Audio.CheckFMODResult(mChannel.getPan(ref pan));

                return pan;
            }
            set
            {
                FMOD_Audio.CheckFMODResult(
                    mChannel.setPan((float)value));
            }
        }

        public override bool IsPlaying
        {
            get
            {
                return mAudio.IsChannelPlaying(mChannel);
            }
        }


        protected override void OnSetLoop(bool value)
        {
            mChannel.setLoopCount(-1);
        }

    }
}
