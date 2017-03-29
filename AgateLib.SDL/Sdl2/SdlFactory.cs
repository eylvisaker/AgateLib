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
	static class SdlFactory
	{
		static ISDL sdl;

		public static ISDL CreateSDL()
		{
			if (sdl == null)
			{
				if (Environment.Is64BitProcess)
					sdl = new SDL64();
				else
					sdl = new SDL32();

				sdl.PreloadLibrary("libogg-0.dll");
				sdl.PreloadLibrary("libvorbis-0.dll");
				sdl.PreloadLibrary("libvorbisfile-3.dll");
				sdl.PreloadLibrary("libFLAC-8.dll");
				sdl.PreloadLibrary("smepg2.dll");
				sdl.PreloadLibrary("libmikmod-2.dll");
				sdl.PreloadLibrary("libmodplug-1.dll");

				sdl.SDL_Init(SDLConstants.SDL_INIT_AUDIO | SDLConstants.SDL_INIT_GAMECONTROLLER | SDLConstants.SDL_INIT_JOYSTICK);
			}

			return sdl;
		}
	}
}
