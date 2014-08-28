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
using SlimDX.XAudio2;
using SlimDX.Multimedia;

namespace AgateSDX.XAud2
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

		public XAudio2_SoundBufferSession(XAudio2_Audio audio, XAudio2_SoundBuffer source)
		{
			mAudio = audio;
			mSource = source;
			mBuffer = source.Buffer;
			mVolume = source.Volume;
			mLoop = source.Loop;

			mVoice = new SourceVoice(mAudio.Device, mSource.Format);
			mVoice.BufferEnd += new EventHandler<ContextEventArgs>(mVoice_BufferEnd);

		}

		void mVoice_BufferEnd(object sender, ContextEventArgs e)
		{
			if (mDisposing)
			{
				mVoice.Dispose();
				return;
			}

			if (mLoop)
			{
				mVoice.SubmitSourceBuffer(mBuffer);
			}
			else
			{
				mIsPlaying = false;
			}
		}
		public override void Dispose()
		{
			mLoop = false;
			mVoice.Stop();

			mDisposing = true;
		}

		protected override void Initialize()
		{
			mVoice.Stop();
			mVoice.SubmitSourceBuffer(mBuffer);
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
				mVoice.Volume = (float)value;
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

		float[] channelVolumes = new float[2];
		public override double Pan
		{
			get { return mPan; }
			set
			{
				mPan = value;
				mVoice.SetChannelVolumes(2, GetChannelVolumes((float)value));
			}
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
}
