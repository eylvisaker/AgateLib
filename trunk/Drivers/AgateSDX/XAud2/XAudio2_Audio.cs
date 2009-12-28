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
	public class XAudio2_Audio : AudioImpl
	{
		XAudio2 mDevice;
		MasteringVoice masteringVoice;

		public XAudio2 Device
		{
			get { return mDevice; }
		}

		public XAudio2_Audio()
		{

		}

		public override void Initialize()
		{
			Report("SlimDX XAudio2 driver instantiated for audio.");


			mDevice = new XAudio2();//XAudio2Flags.DebugEngine, ProcessorSpecifier.AnyProcessor);
			masteringVoice = new MasteringVoice(mDevice);

		}
		public override void Dispose()
		{
			// hack because there is access violation when XAudio2 shuts down?
			try
			{
				mDevice.Dispose();
			}
			catch
			{ }
		}

		public override SoundBufferImpl CreateSoundBuffer(Stream inStream)
		{
			return new SDX_SoundBuffer(this, inStream);
		}

		public override MusicImpl CreateMusic(System.IO.Stream musicStream)
		{
			CheckCoop();

			return new SDX_Music(this, musicStream);
		}
		public override MusicImpl CreateMusic(string filename)
		{
			CheckCoop();

			return new SDX_Music(this, filename);
		}
		public override SoundBufferImpl CreateSoundBuffer(string filename)
		{
			CheckCoop();

			return new SDX_SoundBuffer(this, filename);
		}
		public override SoundBufferSessionImpl CreateSoundBufferSession(SoundBufferImpl buffer)
		{
			CheckCoop();

			return new SDX_SoundBufferSession(this, buffer as SDX_SoundBuffer);
		}


		/// <summary>
		/// hack to make sure the cooperative level is set after a window is created. 
		/// Is this necessary with XAudio2?
		/// </summary>
		private void CheckCoop()
		{
			if (System.Windows.Forms.Form.ActiveForm != null)
			{
				//mDSobject.SetCooperativeLevel(System.Windows.Forms.Form.ActiveForm.Handle,
				//   CooperativeLevel.Priority);
			}
		}
	}

	public class SDX_SoundBuffer : SoundBufferImpl
	{
		XAudio2_Audio mAudio;
		AudioBuffer mBuffer;
		double mVolume;
		WaveFormat mFormat;
		MemoryStream mem;
		byte[] buffer;

		public SDX_SoundBuffer(XAudio2_Audio audio, Stream inStream)
		{
			mAudio = audio;

			WaveStream stream = new WaveStream(inStream);

			mBuffer = new AudioBuffer();
			mBuffer.AudioData = stream;
			mBuffer.AudioBytes = (int)stream.Length;
			mBuffer.Flags = BufferFlags.EndOfStream;

			mFormat = stream.Format;
		}

		public SDX_SoundBuffer(XAudio2_Audio audio, string filename)
			: this(audio, File.OpenRead(filename))
		{

		}

		public override bool Loop { get; set; }

		public override void Dispose()
		{
			mBuffer.Dispose();
		}

		public AudioBuffer Buffer
		{
			get { return mBuffer; }
		}
		public WaveFormat Format
		{
			get { return mFormat; }
		}

		public override double Volume
		{
			get { return mVolume; } 
			set { mVolume = value; }
		}
	}
	public class SDX_SoundBufferSession : SoundBufferSessionImpl
	{
		SDX_SoundBuffer mSource;
		XAudio2_Audio mAudio;
		AudioBuffer mBuffer;
		SourceVoice mVoice;
		double mVolume;
		double mPan;
		bool mIsPlaying;
		bool mLoop = false;
		bool mDisposing = false;

		public SDX_SoundBufferSession(XAudio2_Audio audio, SDX_SoundBuffer source)
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
	public class SDX_Music : MusicImpl
	{
		XAudio2_Audio mAudio;

		public SDX_Music(XAudio2_Audio audio, string filename)
		{
			mAudio = audio;

			if (System.IO.Path.GetExtension(filename) == ".mp3")
				throw new Exception("MP3 files cannot be played due to license restrictions.");

			//LoadMusic(filename);
		}
		
		public SDX_Music(XAudio2_Audio audio, Stream infile)
		{
			mAudio = audio;

			//string tempfile = Path.GetTempFileName();
			//using (FileStream writer = File.OpenWrite(tempfile))
			//{
			//    ReadWriteStream(infile, writer);
			//}

			//try
			//{
			//    LoadMusic(tempfile);
			//}
			//catch (Microsoft.DirectX.DirectXException e)
			//{
			//    throw new AgateLib.AgateException(
			//        "Could not load the music file.  The file format may be unsupported by DirectX.", e);
			//}
			//finally
			//{
			//    File.Delete(tempfile);
			//}
		}
		/*
		private void LoadMusic(string filename)
		{
			mAVAudio = new Microsoft.DirectX.AudioVideoPlayback.Audio(filename);
			mAVAudio.Ending += new EventHandler(mAVAudio_Ending);
		}

		private void ReadWriteStream(Stream readStream, Stream writeStream)
		{
			int Length = 256;
			Byte[] buffer = new Byte[Length];
			int bytesRead = readStream.Read(buffer, 0, Length);
			// write the required bytes
			while (bytesRead > 0)
			{
				writeStream.Write(buffer, 0, bytesRead);
				bytesRead = readStream.Read(buffer, 0, Length);
			}
			readStream.Close();
			writeStream.Close();
		}

		public override void Dispose()
		{
			mAVAudio.Dispose();
		}


		protected override void OnSetLoop(bool value)
		{
			if (value == true)
				mAVAudio.Ending += mAVAudio_Ending;
			else
				mAVAudio.Ending -= mAVAudio_Ending;
		}

		public override void Play()
		{
			mAVAudio.Play();
		}

		public override void Stop()
		{
			mAVAudio.Stop();
		}

		/// <summary>
		/// </summary>
		public override double Volume
		{
			get
			{
				try
				{
					/// The DirectX AudioVideoPlayback object takes volume in the range of
					/// -10000 to 0, indicating the number of hundredths of decibels the volume
					/// is attenuated by, so we convert to zero to 1.

					double vol = (double)(mAVAudio.Volume + 10000) / 10000;
					// logarithmic volume control
					return Audio.TransformByExp(vol);
				}
				catch (Microsoft.DirectX.DirectXException e)
				{
					System.Diagnostics.Debug.WriteLine("Failed to read volume.");
					System.Diagnostics.Debug.WriteLine(e.Message);
					return 1.0;
				}
			}
			set
			{
				// do a logarithmic volume control
				try
				{
					mAVAudio.Volume = (int)(Audio.TransformByLog(value) * 10000.0 - 10000.0);
				}
				catch (Microsoft.DirectX.DirectXException e)
				{
					System.Diagnostics.Debug.WriteLine("Failed to set volume.");
					System.Diagnostics.Debug.WriteLine(e.Message);
				}
			}
		}


		void mAVAudio_Ending(object sender, EventArgs e)
		{
			if (IsLooping)
			{
				mAVAudio.CurrentPosition = 0;
			}
		}

		public override bool IsPlaying
		{
			get { return mAVAudio.Playing; }
		}

		public override double Pan
		{
			get
			{
				return mAVAudio.Balance / (double)10000.0;
			}
			set
			{
				try
				{
					mAVAudio.Balance = (int)(value * 10000.0);
				}
				catch (Microsoft.DirectX.DirectXException e)
				{
					if (e.ErrorCode != -2147220909)
						throw e;
				}
			}
		}
		 * */
		public override void Dispose()
		{
			throw new NotImplementedException();
		}

		public override bool IsPlaying
		{
			get { throw new NotImplementedException(); }
		}

		protected override void OnSetLoop(bool value)
		{
			throw new NotImplementedException();
		}

		public override double Pan
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override void Play()
		{
			throw new NotImplementedException();
		}

		public override void Stop()
		{
			throw new NotImplementedException();
		}

		public override double Volume
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
	}
}
