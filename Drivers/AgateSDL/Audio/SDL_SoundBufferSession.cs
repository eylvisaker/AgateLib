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
using System.Runtime.InteropServices;
using System.Text;

using Tao.Sdl;

using AgateLib;
using AgateLib.ImplementationBase;

namespace AgateSDL.Audio
{
	class SDL_SoundBufferSession : SoundBufferSessionImpl
	{
		IntPtr sound;
		int channel;
		double volume;
		double pan;
		bool loop;

		public SDL_SoundBufferSession(SDL_SoundBuffer buffer)
		{
			loop = buffer.Loop;

			sound = buffer.SoundChunk;
			channel = SdlMixer.Mix_PlayChannel(-1, sound, LoopCount);
			volume = buffer.Volume;

		}
		public override void Dispose()
		{
			Stop();
		}

		public override bool IsPlaying
		{
			get { return SdlMixer.Mix_Playing(channel) != 0; }
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

		private void SetPanning()
		{
			byte leftVol = (byte)(pan <= 0 ? 255 : (int)((1.0 - pan) * 255));
			byte rightVol = (byte)(pan >= 0 ? 255 : (int)((pan + 1.0) * 255));

			SdlMixer.Mix_SetPanning(channel, leftVol, rightVol);
		}

		public override void Play()
		{
			SdlMixer.Mix_PlayChannel(channel, sound, LoopCount);
			SetPanning();
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
			SdlMixer.Mix_Pause(channel);
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

				SdlMixer.Mix_Volume(channel, (int)(volume * 128));
			}
		}
	}
}
