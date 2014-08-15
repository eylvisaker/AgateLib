using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Drivers.NullDrivers
{
	public class NullSoundFactory : IAudioFactory
	{
		public AudioLib.ImplementationBase.AudioImpl CreateAudioImpl()
		{
			return new NullSoundImpl();
		}
	}
}
