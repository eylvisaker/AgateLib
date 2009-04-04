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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AgateLib;
using AgateLib.Drivers;
using AgateLib.ImplementationBase;

namespace AgateFMOD
{
	public sealed class FMOD_Audio : AudioImpl
	{
		FMOD.System mSystem;
		bool mIsDisposed;

		public override SoundBufferImpl CreateSoundBuffer(string filename)
		{
			return new FMOD_SoundBuffer(this, filename);
		}
		public override MusicImpl CreateMusic(string filename)
		{
			return new FMOD_Music(this, filename);
		}
		public override SoundBufferSessionImpl CreateSoundBufferSession(SoundBufferImpl buffer)
		{
			return new FMOD_SoundBufferSession(this, buffer as FMOD_SoundBuffer);
		}

		public FMOD.System FMODSystem
		{
			get { return mSystem; }
		}
		public override void Initialize()
		{
			// create and initialize the FMOD system.
			CheckFMODResult(FMOD.Factory.System_Create(ref mSystem));
			CheckFMODResult(mSystem.init(1000, FMOD.INITFLAG._3D_RIGHTHANDED, IntPtr.Zero));

			Report("AgateFMOD driver instantiated for audio.");

		}
		public override void Dispose()
		{
			if (mSystem != null)
			{
				mSystem.release();
				mSystem = null;
			}

			mIsDisposed = true;
		}

		internal static void CheckFMODResult(FMOD.RESULT result)
		{
			if (result != FMOD.RESULT.OK)
			{
				System.Diagnostics.Debug.WriteLine
					("An error has occurred in the FMOD library: " + result.ToString());

				//throw new Exception("An error has occurred in the FMOD library: " + result.ToString());
			}
		}

		public bool IsDisposed
		{
			get { return mIsDisposed; }
			set { mIsDisposed = value; }
		}


		internal bool IsChannelPlaying(FMOD.Channel channel)
		{
			if (channel == null)
				return false;

			bool playing = false;
			bool paused = false;
			FMOD.RESULT result = channel.isPlaying(ref playing);

			if (result != FMOD.RESULT.OK)
			{
				return false;
			}

			if (playing == false)
				return playing;

			if (channel.getPaused(ref paused) == FMOD.RESULT.OK)
			{
				return !paused;
			}

			channel = null;
			return false;
		}

		public override void Update()
		{
			CheckFMODResult(mSystem.update());
		}

		public override MusicImpl CreateMusic(System.IO.Stream musicStream)
		{
			string filename = SaveStreamToTempFile(musicStream);
			return CreateMusic(filename);
		}
		public override SoundBufferImpl CreateSoundBuffer(System.IO.Stream inStream)
		{
			string filename = SaveStreamToTempFile(inStream);
			return CreateSoundBuffer(filename);
		}
		private string SaveStreamToTempFile(System.IO.Stream inStream)
		{
			string tempfile = System.IO.Path.GetTempFileName();
			byte[] data = new byte[inStream.Length];

			inStream.Read(data, 0, (int)inStream.Length);

			using (Stream file = File.OpenWrite(tempfile))
			{
				file.Write(data, 0, data.Length);
			}

			return tempfile;
		}

	}
}
