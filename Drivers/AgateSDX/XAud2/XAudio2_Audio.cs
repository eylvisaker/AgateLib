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
using System.Threading;
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
		Thread xaudThread;
		bool stopThread;

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

			xaudThread = new Thread(Run);
			xaudThread.Start();
		}

		public bool InvokeRequired
		{
			get
			{
				if (Thread.CurrentThread == xaudThread)
					return false;
				else
					return true;
			}
		}

		struct invk
		{
			public Delegate method;
			public object[] args;
		}

		public void BeginInvoke(Delegate method, params object[] args)
		{
			invk k = new invk { method = method, args = args };

			lock (invokeMethod)
			{
				invokeMethod.Add(k);
			}
		}

		List<invk> invokeMethod = new List<invk>();

		void Run(object unused)
		{
			mDevice = new XAudio2();
			masteringVoice = new MasteringVoice(mDevice);

			for (; ; )
			{
				lock (invokeMethod)
				{
					while (invokeMethod.Count > 0)
					{
						if (stopThread)
							break;

						invk k = invokeMethod[0];
						invokeMethod.RemoveAt(0);

						k.method.DynamicInvoke(k.args);

					}
				}

				if (stopThread)
					break;

				Thread.Sleep(1);

			}

			Dispose();
		}

		
		public override void Dispose()
		{
			if (InvokeRequired)
			{
				stopThread = true;

				int count = 0;

				while (xaudThread.ThreadState == ThreadState.Running)
				{
					Thread.Sleep(10);
					count++;

					if (count > 20)
					{
						xaudThread.Abort();
					}
				}

				return;
	 		}

			// hack because there is access violation when XAudio2 shuts down?
			masteringVoice.Dispose();
			mDevice.Dispose();
		}

		public override SoundBufferImpl CreateSoundBuffer(Stream inStream)
		{
			return new XAudio2_SoundBuffer(this, inStream);
		}

		public override MusicImpl CreateMusic(System.IO.Stream musicStream)
		{
			return new XAudio2_Music(this, musicStream);
		}
		public override MusicImpl CreateMusic(string filename)
		{
			return new XAudio2_Music(this, filename);
		}
		public override SoundBufferImpl CreateSoundBuffer(string filename)
		{
			return new XAudio2_SoundBuffer(this, filename);
		}
		public override SoundBufferSessionImpl CreateSoundBufferSession(SoundBufferImpl buffer)
		{
			return new XAudio2_SoundBufferSession(this, buffer as XAudio2_SoundBuffer);
		}
		public override StreamingSoundBufferImpl CreateStreamingSoundBuffer(Stream input, SoundFormat format)
		{
			return new XAudio2_StreamingSoundBuffer(this, input, format);
		}
	}

	public delegate void DisposeDelegate();

}
