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

namespace AgateFMOD
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

			//CheckCreateChannel();
			CreateChannel();

			Volume = mBuffer.Volume;
		}

		private void CheckFMODResult(FMOD.RESULT result)
		{
			if (result == FMOD.RESULT.ERR_INVALID_HANDLE)
			{
				CreateChannel();
			}
		}

		private void CheckChannel()
		{
			bool p = false;
			FMOD.RESULT result = mChannel.isPlaying(ref p);

			if (result == FMOD.RESULT.ERR_INVALID_HANDLE || p == false)
			{
				CreateChannel();
			}
			else
				FMOD_Audio.CheckFMODResult(result);
		}
		private void CreateChannel()
		{
			FMOD_Audio.CheckFMODResult(
				mSystem.playSound(FMOD.CHANNELINDEX.FREE, mSound, true, ref mChannel));
		}

		public override void Dispose()
		{
		}

		public override void Play()
		{
			CheckChannel();

			//FMOD_Audio.CheckFMODResult();
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
				CheckChannel();

				float vol = 0.0f;
				FMOD_Audio.CheckFMODResult(mChannel.getVolume(ref vol));

				return vol;
			}
			set
			{
				CheckChannel();

				FMOD_Audio.CheckFMODResult(
					mChannel.setVolume((float)value));
			}
		}

		public override double Pan
		{
			get
			{
				CheckChannel();

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
	}
}
