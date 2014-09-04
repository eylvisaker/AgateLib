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
using System.Threading;
using System.Text;
using AgateLib.AudioLib;
using AgateLib.AudioLib.ImplementationBase;
using AgateLib.Drivers;
using SharpDX.XAudio2;
using SharpDX.Multimedia;

namespace AgateLib.Platform.WindowsStore.AudioImplementation

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
		//BinaryWriter w;
		bool mIsDisposed;
		bool mReadingData;
		int thisBufferIndex;

		int mChunkSize;

		const int bufferCount = 3;

		class BufferData
		{
			public AudioBuffer buffer;
			public MemoryStream ms;
			public byte[] backing;
		}

		static int count = 0;

		public XAudio2_StreamingSoundBuffer(XAudio2_Audio audio, Stream input, SoundFormat format)
		{
			mAudio = audio;
			mInput = input;
			mFormat = format;

			Initialize();
		}

		#region --- Thread safety ---

		public bool InvokeRequired
		{
			get { return mAudio.InvokeRequired; }
		}
		public void BeginInvoke(Delegate method, params object[] args)
		{
			mAudio.BeginInvoke(method, args);
		}
		void Invoke(Delegate method, params object[] args)
		{
			mAudio.Invoke(method, args);
		}

		#endregion

		private void Initialize()
		{
			if (InvokeRequired)
			{
				Invoke(new DisposeDelegate(Initialize));
				return;
			}

			throw new NotImplementedException();
			/*
			xaudFormat = new WaveFormat();
			xaudFormat.BitsPerSample = (short)mFormat.BitsPerSample;
			xaudFormat.Channels = (short)mFormat.Channels;
			xaudFormat.BlockAlignment = (short)(mFormat.Channels * mFormat.BitsPerSample / 8);
			xaudFormat.FormatTag = WaveFormatTag.Pcm;
			xaudFormat.SamplesPerSecond = mFormat.SamplingFrequency;
			xaudFormat.AverageBytesPerSecond = xaudFormat.BlockAlignment * xaudFormat.SamplesPerSecond;

			mVoice = new SourceVoice(mAudio.Device, xaudFormat);
			mVoice.BufferEnd += mVoice_BufferEnd;

			buffer = new BufferData[bufferCount];
			for (int i = 0; i < bufferCount; i++)
			{
				buffer[i] = new BufferData();
				buffer[i].ms = new MemoryStream();
				buffer[i].buffer = new AudioBuffer();
				buffer[i].buffer.Flags = BufferFlags.EndOfStream;
			}

			thisBufferIndex = count;

			string tempFileName = string.Format("xaudio2_buffer{0}.pcm", count);
			count++;

			//w = new BinaryWriter(File.Open(tempFileName, FileMode.Create));
			*/
			Pan = 0;
		}

		public override void Dispose()
		{/*
			if (InvokeRequired)
			{
				Invoke(new DisposeDelegate(Dispose));
				return;
			}

			while (mReadingData)
				Thread.Sleep(1);

			mIsDisposed = true;

			mVoice.FlushSourceBuffers();
			Thread.Sleep(1);
			mVoice.Stop();

			//w.BaseStream.Dispose();

			try
			{
				mVoice.Dispose();
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.Print("Caught exception {0}.\n{1}", e.GetType(), e.Message);
			}

			foreach (var b in buffer)
			{
				b.buffer.Dispose();
			}


			System.Diagnostics.Debug.Print("Disposed streaming buffer {0}.", thisBufferIndex);
		  * */
		}


		void mVoice_BufferEnd(IntPtr obj)
		{
			if (IsPlaying == false)
				return;

			ReadAndSubmitNextData();
		}

		public override void Play()
		{
			if (InvokeRequired)
			{
				BeginInvoke(new DisposeDelegate(Play));
				return;
			}

			mNextData = 0;
			ReadAndSubmitNextData();
			ReadAndSubmitNextData();

			mVoice.Start();
			mPlaying = true;
		}
		public override void Stop()
		{
			if (InvokeRequired)
			{
				BeginInvoke(new DisposeDelegate(Stop));
				return;
			}

			mPlaying = false;

			mVoice.Stop();
		}

		#region --- Reading Data ---

		int bytesRead = 0;

		private void ReadAndSubmitNextData()
		{
			if (mIsDisposed)
				return;

			if (InvokeRequired)
			{
				Invoke(new DisposeDelegate(ReadAndSubmitNextData));
				return;
			}

			try
			{
				mReadingData = true;

				ReadData(buffer[mNextData]);
				SubmitData(buffer[mNextData]);

				mNextData++;

				if (mNextData >= bufferCount)
					mNextData = 0;
			}
			finally
			{
				mReadingData = false;
			}
		}
		private void ReadData(BufferData bufferData)
		{
			/*
			if (bufferData.backing == null)
			{
				ResizeBacking();
			}

			int count = mInput.Read(bufferData.backing, 0, bufferData.backing.Length);
			bytesRead += count;

			bufferData.ms.Position = 0;
			bufferData.buffer.AudioData = bufferData.ms;
			bufferData.buffer.AudioBytes = count;

			//w.Write(bufferData.backing, 0, count);
			 * */
		}

		private void SubmitData(BufferData bufferData)
		{/*
			mVoice.SubmitSourceBuffer(bufferData.buffer);*/
		}

		#endregion

		public override int ChunkSize
		{
			get
			{
				return mChunkSize;
			}
			set
			{
				mChunkSize = value;

				ResizeBacking();
			}
		}

		int ChunkByteSize
		{
			get
			{
				return ChunkSize * xaudFormat.BlockAlign;
			}
		}
		private void ResizeBacking()
		{
			if (InvokeRequired)
			{
				Invoke(new DisposeDelegate(ResizeBacking));
				return;
			}

			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i].backing = new byte[ChunkByteSize];
				buffer[i].ms = new MemoryStream(buffer[i].backing);
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
				ResetChannelVolumes();
			}
		}

		private void ResetChannelVolumes()
		{
			if (InvokeRequired)
			{
				BeginInvoke(new DisposeDelegate(ResetChannelVolumes));
				return;
			}

			mVoice.SetChannelVolumes(2, GetChannelVolumes((float)mPan));
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
