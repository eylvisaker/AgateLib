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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.AgateSDL.Sdl2
{
	static class SDLConstants
	{
		public const uint SDL_INIT_TIMER = 0x00000001;
		public const uint SDL_INIT_AUDIO = 0x00000010;
		public const uint SDL_INIT_VIDEO = 0x00000020;
		public const uint SDL_INIT_JOYSTICK = 0x00000200;
		public const uint SDL_INIT_HAPTIC = 0x00001000;
		public const uint SDL_INIT_GAMECONTROLLER = 0x00002000;
		public const uint SDL_INIT_NOPARACHUTE = 0x00100000;
		public const uint SDL_INIT_EVERYTHING = (
			SDL_INIT_TIMER | SDL_INIT_AUDIO | SDL_INIT_VIDEO |
			SDL_INIT_JOYSTICK | SDL_INIT_HAPTIC |
			SDL_INIT_GAMECONTROLLER
		);

		public const ushort AUDIO_U8 = 0x0008;
		public const ushort AUDIO_S8 = 0x8008;
		public const ushort AUDIO_U16LSB = 0x0010;
		public const ushort AUDIO_S16LSB = 0x8010;
		public const ushort AUDIO_U16MSB = 0x1010;
		public const ushort AUDIO_S16MSB = 0x9010;
		public const ushort AUDIO_U16 = AUDIO_U16LSB;
		public const ushort AUDIO_S16 = AUDIO_S16LSB;
		public const ushort AUDIO_S32LSB = 0x8020;
		public const ushort AUDIO_S32MSB = 0x9020;
		public const ushort AUDIO_S32 = AUDIO_S32LSB;
		public const ushort AUDIO_F32LSB = 0x8120;
		public const ushort AUDIO_F32MSB = 0x9120;
		public const ushort AUDIO_F32 = AUDIO_F32LSB;

		public static readonly ushort AUDIO_U16SYS =
			BitConverter.IsLittleEndian ? AUDIO_U16LSB : AUDIO_U16MSB;
		public static readonly ushort AUDIO_S16SYS =
			BitConverter.IsLittleEndian ? AUDIO_S16LSB : AUDIO_S16MSB;
		public static readonly ushort AUDIO_S32SYS =
			BitConverter.IsLittleEndian ? AUDIO_S32LSB : AUDIO_S32MSB;
		public static readonly ushort AUDIO_F32SYS =
			BitConverter.IsLittleEndian ? AUDIO_F32LSB : AUDIO_F32MSB;

		public const uint SDL_AUDIO_ALLOW_FREQUENCY_CHANGE = 0x00000001;
		public const uint SDL_AUDIO_ALLOW_FORMAT_CHANGE = 0x00000001;
		public const uint SDL_AUDIO_ALLOW_CHANNELS_CHANGE = 0x00000001;
		public const uint SDL_AUDIO_ALLOW_ANY_CHANGE = (
			SDL_AUDIO_ALLOW_FREQUENCY_CHANGE |
			SDL_AUDIO_ALLOW_FORMAT_CHANGE |
			SDL_AUDIO_ALLOW_CHANNELS_CHANGE
		);

		public const int SDL_MIX_MAXVOLUME = 128;


		public static readonly int MIX_DEFAULT_FREQUENCY = 22050;
		public static readonly ushort MIX_DEFAULT_FORMAT =
			BitConverter.IsLittleEndian ? SDLConstants.AUDIO_S16LSB : SDLConstants.AUDIO_S16MSB;
		public static readonly int MIX_DEFAULT_CHANNELS = 2;
		public static readonly byte MIX_MAX_VOLUME = 128;

		public const byte SDL_HAT_CENTERED = 0x00;
		public const byte SDL_HAT_UP = 0x01;
		public const byte SDL_HAT_RIGHT = 0x02;
		public const byte SDL_HAT_DOWN = 0x04;
		public const byte SDL_HAT_LEFT = 0x08;
		public const byte SDL_HAT_RIGHTUP = SDL_HAT_RIGHT | SDL_HAT_UP;
		public const byte SDL_HAT_RIGHTDOWN = SDL_HAT_RIGHT | SDL_HAT_DOWN;
		public const byte SDL_HAT_LEFTUP = SDL_HAT_LEFT | SDL_HAT_UP;
		public const byte SDL_HAT_LEFTDOWN = SDL_HAT_LEFT | SDL_HAT_DOWN;
	}
	
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void SDL_mixer_MixFuncDelegate(
		IntPtr udata, // void*
		IntPtr stream, // Uint8*
		int len
	);
}
