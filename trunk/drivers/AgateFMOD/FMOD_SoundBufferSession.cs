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

using ERY.AgateLib;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib.AgateFMOD
{
    class FMOD_SoundBufferSession : SoundBufferSessionImpl
    {
        FMOD_Audio mAudio;
        FMOD_SoundBuffer mBuffer;
        FMOD.System mSystem;
        FMOD.Sound mSound;
        FMOD.Channel mChannel;

        public FMOD_SoundBufferSession(FMOD_Audio audio, FMOD_SoundBuffer buffer)
        {
            if (buffer == null)
                throw new NullReferenceException();

            mAudio = audio;
            mBuffer = buffer;

            mSystem = mAudio.FMODSystem;
            mSound = mBuffer.FMODSound;

            CheckCreateChannel();

            Volume = mBuffer.Volume;
        }

        private void CheckCreateChannel()
        {
            if (mChannel == null)
            {
                FMOD_Audio.CheckFMODResult(
                    mSystem.playSound(FMOD.CHANNELINDEX.FREE, mSound, true, ref mChannel));

            }
        }

        public override void Dispose()
        {
        }

        public override void Play()
        {
            if (IsPlaying == false)
                mChannel = null;

            CheckCreateChannel();

            FMOD_Audio.CheckFMODResult(mChannel.setPaused(false));
           
        }

        public override void Stop()
        {
            if (mChannel == null)
                return;

            mChannel.stop();   
        }

        public override double Volume
        {
            get
            {
                CheckCreateChannel();

                float vol = 0.0f;
                FMOD_Audio.CheckFMODResult(mChannel.getVolume(ref vol));

                return vol;
            }
            set
            {
                CheckCreateChannel();

                FMOD_Audio.CheckFMODResult(
                    mChannel.setVolume((float)value));
            }
        }

        public override double Pan
        {
            get
            {
                CheckCreateChannel();

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
                if (mChannel == null)
                    return false;

                bool playing = false;
                FMOD.RESULT result = mChannel.isPlaying(ref playing);

                if (result != FMOD.RESULT.OK)
                {
                    mChannel = null;
                    return false;
                }

                return playing;
            }
        }
    }
}
