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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using AgateLib;
using AgateLib.AudioLib.ImplementationBase;
using Tao.Sdl;

namespace AgateSDL.Audio
{
	class SDL_SoundBufferSession : SoundBufferSessionImpl
	{
		IntPtr sound;
		int channel = -1;
		double volume;
		double pan;
		bool loop;
		Stopwatch watch = new Stopwatch();
		SDL_SoundBuffer buffer;
		SDL_Audio audio;

		public bool mIsPlaying;

		public SDL_SoundBufferSession(SDL_SoundBuffer buffer)
		{
			this.buffer = buffer;
			loop = buffer.Loop;

			sound = buffer.SoundChunk;
			volume = buffer.Volume;

			audio = (SDL_Audio)AgateLib.AudioLib.Audio.Impl;
		}
		public override void Dispose()
		{
			Stop();
		}

		protected override void Initialize()
		{

		}

		public override bool IsPlaying
		{
			get
			{
				if (channel == -1)
					return false;

				return SdlMixer.Mix_Playing(channel) != 0;
			}
		}

		public override double Pan
		{
			get
			{
				return pan;
			}
			set
			{
				pan = value;
				SetPanning();
			}
		}

		public override int CurrentLocation
		{
			get
			{
				return (int)(watch.ElapsedMilliseconds / 1000.0 * buffer.SamplePerSec);
			}
		}

		private void SetPanning()
		{
			if (channel == -1)
				return;

			byte leftVol = (byte)(pan <= 0 ? 255 : (int)((1.0 - pan) * 255));
			byte rightVol = (byte)(pan >= 0 ? 255 : (int)((pan + 1.0) * 255));

			SdlMixer.Mix_SetPanning(channel, leftVol, rightVol);
		}

		public override void Play()
		{
			if (IsPlaying == false)
			{
				channel = SdlMixer.Mix_PlayChannel(-1, sound, LoopCount);

				if (channel == -1)
					Trace.WriteLine(string.Format("Error: {0}", SdlMixer.Mix_GetError()));
			}
			else
			{
				SdlMixer.Mix_PlayChannel(channel, sound, LoopCount);
			}

			SetPanning();
			SetVolume();

			watch.Reset();
			watch.Start();

			audio.RegisterChannel(channel, this);

			mIsPlaying = true;
		}

		int LoopCount
		{
			get
			{
				int loops = 0;
				if (loop)
					loops = -1;

				return loops;
			}
		}

		public override void Stop()
		{
			SdlMixer.Mix_HaltChannel(channel);

			watch.Stop();
		}

		public override double Volume
		{
			get
			{
				return volume;
			}
			set
			{
				volume = value;

				SetVolume();
			}
		}

		private void SetVolume()
		{
			if (channel != -1)
			{
				SdlMixer.Mix_Volume(channel, (int)(volume * 128));
			}
		}

		public override bool IsPaused
		{
			get
			{
				if (channel == -1)
					return false;
				else
					return SdlMixer.Mix_Paused(channel) != 0;
			}
			set
			{
				if (channel == -1)
					return;

				if (IsPaused)
				{
					SdlMixer.Mix_Resume(channel);
				}
				else
				{
					SdlMixer.Mix_Pause(channel);
				}
			}
		}
	}
}
