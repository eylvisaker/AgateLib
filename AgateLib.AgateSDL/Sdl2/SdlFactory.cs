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
			}

			return sdl;
		}
	}
}
