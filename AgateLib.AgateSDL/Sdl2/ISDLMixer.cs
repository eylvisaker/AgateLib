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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.AgateSDL.Sdl2
{
	interface ISDLMixer
	{
		void Mix_CloseAudio();

		void Mix_ChannelFinished(Action<int> channelFinished);

		void Mix_AllocateChannels(int numchans);

		int Mix_OpenAudio(int frequency, ushort format, int channels, int chunksize);

		void Mix_FreeMusic(IntPtr music);

		IntPtr Mix_LoadMUS(string file);

		int Mix_PlayingMusic();

		int Mix_PausedMusic();

		void Mix_PlayMusic(IntPtr music, int p);

		double Mix_VolumeMusic(int p);

		void Mix_PauseMusic();

		void Mix_FreeChunk(IntPtr sound);

		IntPtr Mix_LoadWAV(string file);

		int Mix_Playing(int channel);

		void Mix_SetPanning(int channel, byte leftVol, byte rightVol);

		int Mix_PlayChannel(int p, IntPtr sound, int LoopCount);

		void Mix_HaltChannel(int channel);

		void Mix_Volume(int channel, int p);

		int Mix_Paused(int channel);

		void Mix_Resume(int channel);

		void Mix_Pause(int channel);

		string GetError();

		void Mix_HookMusic(SDL_mixer_MixFuncDelegate mix_func, IntPtr arg);
	}
}
