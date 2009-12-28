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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
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
	class XAudio2_StreamingSoundBuffer : StreamingSoundBufferImpl
	{
		XAudio2_Audio mAudio;
		Stream mInput;
		SoundFormat mFormat;
		SourceVoice mVoice;
		WaveFormat xaudFormat;
		BufferData[] buffer;
		bool mPlaying;
		int mNextData;
		double mPan;

		int mChunkSize;

		const int bufferCount = 3;

		class BufferData
		{
			public AudioBuffer buffer;
			public MemoryStream ms;
			public byte[] backing;
		}

		public XAudio2_StreamingSoundBuffer(XAudio2_Audio audio, Stream input, SoundFormat format)
		{
			mAudio = audio;
			mInput = input;
			mFormat = format;

			xaudFormat = new WaveFormat();
			xaudFormat.BitsPerSample = (short) mFormat.BitsPerSample;
			xaudFormat.Channels = (short)mFormat.Channels;
			xaudFormat.BlockAlignment = (short)(mFormat.Channels * mFormat.BitsPerSample / 8);
			xaudFormat.FormatTag = WaveFormatTag.Pcm;
			xaudFormat.SamplesPerSecond = format.SamplingFrequency;
			xaudFormat.AverageBytesPerSecond = xaudFormat.BlockAlignment * xaudFormat.SamplesPerSecond;

			mVoice = new SourceVoice(audio.Device, xaudFormat);
			mVoice.BufferEnd += new EventHandler<ContextEventArgs>(mVoice_BufferEnd);

			buffer = new BufferData[bufferCount];
			for (int i = 0; i < bufferCount; i++)
			{
				buffer[i] = new BufferData();
				buffer[i].buffer = new AudioBuffer();
				buffer[i].ms = new MemoryStream();
			}
		}

		public override void Dispose()
		{
			try
			{
				mVoice.Stop();
				mVoice.Dispose();
			}
			catch
			{

			}
		}
		void mVoice_BufferEnd(object sender, ContextEventArgs e)
		{
			ReadAndSubmitNextData();
		}

		private void ReadAndSubmitNextData()
		{
			ReadData(buffer[mNextData]);
			SubmitData(buffer[mNextData]);

			mNextData++;

			if (mNextData >= bufferCount)
				mNextData = 0;
		}

		public override void Play()
		{
			mNextData = 0;
			ReadAndSubmitNextData();

			mVoice.Start();
			mPlaying = true;
		}



		public override void Stop()
		{
			mPlaying = false;

			mVoice.Stop();
		}


		private void ReadData(BufferData bufferData)
		{
			int count = mInput.Read(bufferData.backing, 0, bufferData.backing.Length);

			bufferData.ms.Position = 0;
			bufferData.buffer.AudioData = bufferData.ms;
			bufferData.buffer.AudioBytes = count;
		}

		private void SubmitData(BufferData bufferData)
		{
			mVoice.SubmitSourceBuffer(bufferData.buffer);
		}

		public override int ChunkSize
		{
			get
			{
				return mChunkSize;
			}
			set
			{
				if (value <= xaudFormat.BlockAlignment)
					throw new ArgumentOutOfRangeException(string.Format(
						"Chunk size is too small.  Must be multiple of {0} but was {1}.",
						xaudFormat.BlockAlignment, value));

				if (value % xaudFormat.BlockAlignment != 0)
					throw new ArgumentException(string.Format(
						"Chunk size {0} was not a multiple of the block alignment {1}.",
						value, xaudFormat.BlockAlignment));

				mChunkSize = value;

				for (int i = 0; i < buffer.Length; i++)
				{
					buffer[i].backing = new byte[mChunkSize];
					buffer[i].ms = new MemoryStream(buffer[i].backing);
				}
			}
		}

		public override bool IsPlaying
		{
			get { return mPlaying; }
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
