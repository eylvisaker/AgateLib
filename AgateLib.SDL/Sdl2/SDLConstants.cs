using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.SDL.Sdl2
{
	class SDLConstants
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

	}
}
