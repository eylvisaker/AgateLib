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
	public class XAudio2_SoundBuffer : SoundBufferImpl
	{
		XAudio2_Audio mAudio;
		AudioBuffer mBuffer;
		double mVolume;
		WaveFormat mFormat;

		public XAudio2_SoundBuffer(XAudio2_Audio audio, Stream inStream)
		{
			mAudio = audio;

			WaveStream stream = new WaveStream(inStream);

			mBuffer = new AudioBuffer();
			mBuffer.AudioData = stream;
			mBuffer.AudioBytes = (int)stream.Length;

			mFormat = stream.Format;
		}

		public XAudio2_SoundBuffer(XAudio2_Audio audio, string filename)
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
}
