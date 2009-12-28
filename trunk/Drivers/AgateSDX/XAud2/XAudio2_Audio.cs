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
			return new XAudio2_SoundBuffer(this, inStream);
		}

		public override MusicImpl CreateMusic(System.IO.Stream musicStream)
		{
			CheckCoop();

			return new XAudio2_Music(this, musicStream);
		}
		public override MusicImpl CreateMusic(string filename)
		{
			CheckCoop();

			return new XAudio2_Music(this, filename);
		}
		public override SoundBufferImpl CreateSoundBuffer(string filename)
		{
			CheckCoop();

			return new XAudio2_SoundBuffer(this, filename);
		}
		public override SoundBufferSessionImpl CreateSoundBufferSession(SoundBufferImpl buffer)
		{
			CheckCoop();

			return new XAudio2_SoundBufferSession(this, buffer as XAudio2_SoundBuffer);
		}
		public override StreamingSoundBufferImpl CreateStreamingSoundBuffer(Stream input, SoundFormat format)
		{
			return new XAudio2_StreamingSoundBuffer(this, input, format);
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

}
