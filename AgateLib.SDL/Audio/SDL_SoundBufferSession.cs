//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using AgateLib.AudioLib.ImplementationBase;
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

		public SDL_SoundBufferSession(SoundBufferSession owner, SDL_SoundBuffer buffer)
		{
			sdl = SdlFactory.CreateSDL();
			Owner = owner;

			this.buffer = buffer;
			loop = buffer.Loop;

			sound = buffer.SoundChunk;
			volume = buffer.Volume;

			audio = (SDL_Audio)AgateLib.AudioLib.Audio.Impl;
		}

		protected override void Dispose(bool disposing)
		{
			HaltChannel();

			base.Dispose(disposing);
		}

		private int LoopCount
		{
			get
			{
				int loops = 0;
				if (loop)
					loops = -1;

				return loops;
			}
		}

		public SoundBufferSession Owner { get; private set; }

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
		}

		public override void Stop()
		{
			HaltChannel();

			watch.Stop();
		}

		internal void OnPlaybackFinished()
		{
			channel = -1;
		}

		protected override void Initialize()
		{

		}

		private void HaltChannel()
		{
			sdl.Mixer.Mix_HaltChannel(channel);
		}

		private void SetVolume()
		{
			if (channel != -1)
			{
				sdl.Mixer.Mix_Volume(channel, (int)(volume * AudioLib.Audio.Configuration.SoundVolume * 128));
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

	}
}
