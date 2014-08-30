using AgateLib.Drivers;
using AgateLib.Drivers.NullDrivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.WinForms.Factories
{
	class SdlFactory : IAudioFactory, IInputFactory
	{
		public InputLib.ImplementationBase.InputImpl CreateJoystickInputImpl()
		{
			return new AgateLib.AgateSDL.Input.SDL_Input();
		}

		public AudioLib.ImplementationBase.AudioImpl CreateAudioImpl()
		{
			return new AgateLib.AgateSDL.Audio.SDL_Audio();
		}
	}
}
