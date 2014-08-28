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

	public class XAudio2_Music : MusicImpl
	{
		XAudio2_Audio mAudio;

		public XAudio2_Music(XAudio2_Audio audio, string filename)
		{
			mAudio = audio;

			if (System.IO.Path.GetExtension(filename) == ".mp3")
				throw new Exception("MP3 files cannot be played due to license restrictions.");

			//LoadMusic(filename);
		}

		public XAudio2_Music(XAudio2_Audio audio, Stream infile)
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
