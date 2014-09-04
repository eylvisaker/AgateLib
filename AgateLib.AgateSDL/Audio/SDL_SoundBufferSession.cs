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
using SDL2;
using AgateLib.AgateSDL.Sdl2;
using AgateLib.AudioLib;

namespace AgateLib.AgateSDL.Audio
{
	class SDL_SoundBufferSession : SoundBufferSessionImpl
	{
		ISDL sdl;
		IntPtr sound;
		int channel = -1;
		double volume;
		double pan;
		bool loop;
		Stopwatch watch = new Stopwatch();
		SDL_SoundBuffer buffer;
		SDL_Audio audio;

		public bool mIsPlaying;

		public SDL_SoundBufferSession(SoundBufferSession owner, SDL_SoundBuffer buffer)
		{
			sdl = SdlFactory.CreateSDL();
			Owner = owner;
			
			this.buffer = buffer;
			loop = buffer.Loop;

			sound = buffer.SoundChunk;
			volume = buffer.Volume;

			audio = (SDL_Audio)AgateLib.AudioLib.Audio.Impl;

			Debug.Print("Playing " + buffer.Filename);

			Play();
		}
		public override void Dispose()
		{
			Stop();
		}
		public SoundBufferSession Owner { get; private set; }

		protected override void Initialize()
		{

		}

		public override bool IsPlaying
		{
			get
			{
				if (channel == -1)
					return false;

				return sdl.Mixer.Mix_Playing(channel) != 0;
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

			sdl.Mixer.Mix_SetPanning(channel, leftVol, rightVol);
		}

		public override void Play()
		{
			if (IsPlaying == false)
			{
				channel = sdl.Mixer.Mix_PlayChannel(-1, sound, LoopCount);

				if (channel == -1)
					Trace.WriteLine(string.Format("Error: {0}", "unknown" /*SDL_mixer.Mix_GetError() */));
			}
			else
			{
				sdl.Mixer.Mix_PlayChannel(channel, sound, LoopCount);
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
			sdl.Mixer.Mix_HaltChannel(channel);

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
				sdl.Mixer.Mix_Volume(channel, (int)(volume * 128));
			}
		}

		public override bool IsPaused
		{
			get
			{
				if (channel == -1)
					return false;
				else
					return sdl.Mixer.Mix_Paused(channel) != 0;
			}
			set
			{
				if (channel == -1)
					return;

				if (IsPaused)
				{
					sdl.Mixer.Mix_Resume(channel);
				}
				else
				{
					sdl.Mixer.Mix_Pause(channel);
				}
			}
		}
	}
}
