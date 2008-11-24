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
using AgateLib.ImplementationBase;

namespace AgateLib.AgateFMOD
{
    class FMOD_SoundBuffer : SoundBufferImpl 
    {
        FMOD_Audio mAudio;
        FMOD.System mSystem;
        FMOD.Sound mSound;
        double mVolume = 1.0;
        bool mIsDisposed = false;

        public FMOD_SoundBuffer(FMOD_Audio audio, string filename)
        {
            mAudio = audio;
            mSystem = mAudio.FMODSystem;

            FMOD_Audio.CheckFMODResult(mSystem.createSound(filename, FMOD.MODE.DEFAULT, ref mSound));

        }
        ~FMOD_SoundBuffer()
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

        public FMOD.Sound FMODSound
        {
            get { return mSound; }
        }

        public override double Volume
        {
            get
            {
                return mVolume;
            }
            set
            {
                if (value < 0.0) mVolume = 0.0;
                else if (value > 1.0) mVolume = 1.0;
                else mVolume = value;
            }
        }
    }
}
