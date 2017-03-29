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
