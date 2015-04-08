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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using AgateLib.AudioLib;
using AgateLib.AudioLib.ImplementationBase;
using AgateLib.Drivers;
using SharpDX.XAudio2;
using SharpDX.Multimedia;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsStore.AudioImplementation
{
	public class XAudio2_SoundBufferSession : SoundBufferSessionImpl
	{
		XAudio2_SoundBuffer mSource;
		XAudio2_Audio mAudio;
		AudioBuffer mBuffer;
		SourceVoice mVoice;
		double mVolume;
		double mPan;
		bool mIsPlaying;
		bool mIsPaused;
		bool mLoop = false;
		bool mDisposing = false;

		float[] channelVolumes = new float[2];
		float[] outputMatrix = new float[8];

		public XAudio2_SoundBufferSession(XAudio2_Audio audio, XAudio2_SoundBuffer source)
		{
			mAudio = audio;
			mSource = source;
			mBuffer = source.Buffer;
			mVolume = source.Volume;
			mLoop = source.Loop;

			mVoice = new SourceVoice(mAudio.Device, mSource.Format);
			mVoice.BufferEnd += mVoice_BufferEnd;
		}

		void mVoice_BufferEnd(IntPtr obj)
		{
			if (mDisposing)
			{
				mVoice.Dispose();
				return;
			}

			if (mLoop)
			{
				mVoice.SubmitSourceBuffer(mBuffer, null);
			}
			else
			{
				mIsPlaying = false;
			}
		}

		protected override void Dispose(bool disposing)
		{
			mLoop = false;

            if (disposing)
            {
                mVoice.Stop();
            }

			mDisposing = true;
		}

		protected override void Initialize()
		{
			mVoice.Stop();
			mVoice.SubmitSourceBuffer(mBuffer, null);
		}

		public override int CurrentLocation
		{
			get
			{
				return (int)mVoice.State.SamplesPlayed;
			}
		}
		public override void Play()
		{
			mVoice.Stop();
			mVoice.Start();
			mIsPlaying = true;
		}

		public override void Stop()
		{
			mVoice.Stop();
		}

		public override bool IsPaused
		{
			get
			{
				return mIsPaused;
			}
			set
			{
				mIsPaused = value;

				if (mIsPaused)
					mVoice.Stop();
				else
					mVoice.Start();
			}
		}

		public override double Volume
		{
			get { return mVolume; }
			set
			{
				mVoice.SetVolume((float)value);
				mVolume = value;
			}
		}

		public override bool IsPlaying
		{
			get
			{
				return mIsPlaying;
			}
		}

		public override double Pan
		{
			get { return mPan; }
			set
			{
				SetPan(value);
			}
		}

		private void SetPan(double value)
		{
			mPan = value;

			// The description of this approach is found here:
			// http://msdn.microsoft.com/en-us/library/windows/desktop/hh405043%28v=vs.85%29.aspx

			var outputChannels = (SpeakerConfigurations)mAudio.MasteringVoice.ChannelMask;
			GetChannelVolumes((float)mPan);

			float left = channelVolumes[0];
			float right = channelVolumes[1];

			switch (outputChannels)
			{
				case SpeakerConfigurations.SPEAKER_MONO:
					outputMatrix[0] = 1.0f;
					break;
				case SpeakerConfigurations.SPEAKER_STEREO:
				case SpeakerConfigurations.SPEAKER_2POINT1:
				case SpeakerConfigurations.SPEAKER_SURROUND:
					outputMatrix[0] = left;
					outputMatrix[1] = right;
					break;
				case SpeakerConfigurations.SPEAKER_QUAD:
					outputMatrix[0] = outputMatrix[2] = left;
					outputMatrix[1] = outputMatrix[3] = right;
					break;
				case SpeakerConfigurations.SPEAKER_4POINT1:
					outputMatrix[0] = outputMatrix[3] = left;
					outputMatrix[1] = outputMatrix[4] = right;
					break;
				case SpeakerConfigurations.SPEAKER_5POINT1:
				case SpeakerConfigurations.SPEAKER_7POINT1:
				case SpeakerConfigurations.SPEAKER_5POINT1_SURROUND:
					outputMatrix[0] = outputMatrix[4] = left;
					outputMatrix[1] = outputMatrix[5] = right;
					break;
				case SpeakerConfigurations.SPEAKER_7POINT1_SURROUND:
					outputMatrix[0] = outputMatrix[4] = outputMatrix[6] = left;
					outputMatrix[1] = outputMatrix[5] = outputMatrix[7] = right;
					break;
			}

			mVoice.SetOutputMatrix(mVoice.VoiceDetails.InputChannelCount,
				mAudio.MasteringVoice.VoiceDetails.InputChannelCount,
				outputMatrix);

			//await mAudio.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			//	{
			//		mVoice.SetChannelVolumes(2, GetChannelVolumes((float)value));
			//	});
		}

		private float[] GetChannelVolumes(float pan)
		{
			if (pan < 0)
			{
				channelVolumes[0] = 1;
				channelVolumes[1] = 1 + pan;
			}
			else
			{
				channelVolumes[0] = 1 - pan;
				channelVolumes[1] = 1;
			}

			return channelVolumes;
		}
	}

	enum SpeakerConfigurations : uint
	{
		SPEAKER_FRONT_LEFT = 0x00000001,
		SPEAKER_FRONT_RIGHT = 0x00000002,
		SPEAKER_FRONT_CENTER = 0x00000004,
		SPEAKER_LOW_FREQUENCY = 0x00000008,
		SPEAKER_BACK_LEFT = 0x00000010,
		SPEAKER_BACK_RIGHT = 0x00000020,
		SPEAKER_FRONT_LEFT_OF_CENTER = 0x00000040,
		SPEAKER_FRONT_RIGHT_OF_CENTER = 0x00000080,
		SPEAKER_BACK_CENTER = 0x00000100,
		SPEAKER_SIDE_LEFT = 0x00000200,
		SPEAKER_SIDE_RIGHT = 0x00000400,
		SPEAKER_TOP_CENTER = 0x00000800,
		SPEAKER_TOP_FRONT_LEFT = 0x00001000,
		SPEAKER_TOP_FRONT_CENTER = 0x00002000,
		SPEAKER_TOP_FRONT_RIGHT = 0x00004000,
		SPEAKER_TOP_BACK_LEFT = 0x00008000,
		SPEAKER_TOP_BACK_CENTER = 0x00010000,
		SPEAKER_TOP_BACK_RIGHT = 0x00020000,
		SPEAKER_RESERVED = 0x7FFC0000,// bit mask locations reserved for future use
		SPEAKER_ALL = 0x80000000,// used to specify that any possible permutation of speaker configurations
		SPEAKER_MONO = SPEAKER_FRONT_CENTER,
		SPEAKER_STEREO = (SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT),
		SPEAKER_2POINT1 = (SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT | SPEAKER_LOW_FREQUENCY),
		SPEAKER_SURROUND = (SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT | SPEAKER_FRONT_CENTER | SPEAKER_BACK_CENTER),
		SPEAKER_QUAD = (SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT | SPEAKER_BACK_LEFT | SPEAKER_BACK_RIGHT),
		SPEAKER_4POINT1 = (SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT | SPEAKER_LOW_FREQUENCY | SPEAKER_BACK_LEFT | SPEAKER_BACK_RIGHT),
		SPEAKER_5POINT1 = (SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT | SPEAKER_FRONT_CENTER | SPEAKER_LOW_FREQUENCY | SPEAKER_BACK_LEFT | SPEAKER_BACK_RIGHT),
		SPEAKER_7POINT1 = (SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT | SPEAKER_FRONT_CENTER | SPEAKER_LOW_FREQUENCY | SPEAKER_BACK_LEFT | SPEAKER_BACK_RIGHT | SPEAKER_FRONT_LEFT_OF_CENTER | SPEAKER_FRONT_RIGHT_OF_CENTER),
		SPEAKER_5POINT1_SURROUND = (SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT | SPEAKER_FRONT_CENTER | SPEAKER_LOW_FREQUENCY | SPEAKER_SIDE_LEFT | SPEAKER_SIDE_RIGHT),
		SPEAKER_7POINT1_SURROUND = (SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT | SPEAKER_FRONT_CENTER | SPEAKER_LOW_FREQUENCY | SPEAKER_BACK_LEFT | SPEAKER_BACK_RIGHT | SPEAKER_SIDE_LEFT | SPEAKER_SIDE_RIGHT),
		SPEAKER_XBOX = SPEAKER_5POINT1,
	}

}
