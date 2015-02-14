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
using System.Threading;
using AgateLib.AudioLib;
using AgateLib.AudioLib.ImplementationBase;
using AgateLib.Drivers;
using SharpDX.XAudio2;
using SharpDX.Multimedia;
using Windows.UI.Core;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsStore.AudioImplementation
{
	public class XAudio2_Audio : AudioImpl
	{
		XAudio2 mDevice;
		MasteringVoice mMasteringVoice;
		bool stopThread;

		List<InvokeData> methodsToInvoke = new List<InvokeData>();

		struct InvokeData
		{
			public Delegate method;
			public object[] args;
		}

		public XAudio2 Device
		{
			get { return mDevice; }
		}

		public XAudio2_Audio()
		{

		}

		public CoreDispatcher Dispatcher { get; private set; }
		public MasteringVoice MasteringVoice { get { return mMasteringVoice; } }

		public override void Initialize()
		{
			Dispatcher = DisplayImplementation.SDX_Display.MainThreadCoreWindow.Dispatcher;

			mDevice = new XAudio2();
			mMasteringVoice = new MasteringVoice(mDevice);

			Report("SharpDX XAudio2 driver instantiated for audio.");

		}

		#region --- Threading ---

		public bool InvokeRequired
		{
			get
			{
				return false;
				/*
				if (Thread.CurrentThread == xaudThread)
					return false;
				else
					return true;
				 * */
			}
		}


		public void BeginInvoke(Delegate method, params object[] args)
		{
			InvokeData k = new InvokeData { method = method, args = args };

			lock (methodsToInvoke)
			{
				methodsToInvoke.Add(k);
			}
		}

		public void Invoke(Delegate method, object[] args)
		{
			BeginInvoke(method, args);

			throw new NotImplementedException();
			/*
			while (methodsToInvoke.Count > 0)
				Thread.Sleep(1);
			 * */
		}
		
		void Run(object unused)
		{
			mDevice = new XAudio2();
			mMasteringVoice = new MasteringVoice(mDevice);

			for (; ; )
			{
				int count = methodsToInvoke.Count;

				for (int i = 0; i < count; i++)
				{
					if (stopThread)
						break;

					InvokeData k = methodsToInvoke[i];

					k.method.DynamicInvoke(k.args);
				}

				lock (methodsToInvoke)
				{
					methodsToInvoke.RemoveRange(0, count);
				}

				if (stopThread)
					break;

				//Thread.Sleep(1);
			}

			Dispose();
		}

		#endregion

        protected override void Dispose(bool disposing)
        {
            if (InvokeRequired)
            {
                stopThread = true;

                /*
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
                */
                return;
            }

            if (disposing)
            {
                mMasteringVoice.Dispose();
                mDevice.Dispose();
            }

            base.Dispose(disposing);
        }

		protected override bool CapsBool(AgateLib.AudioLib.AudioBoolCaps audioBoolCaps)
		{
			switch (audioBoolCaps)
			{
				case AudioBoolCaps.StreamingSoundBuffer:
					return true;
				default:
					return false;
			}
		}
	}

	public delegate void DisposeDelegate();
}
