using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.AudioLib;
using AgateLib.AudioLib.ImplementationBase;

namespace AgateLib.Configuration.State
{
	class AudioState
	{
		internal AudioImpl Impl;
		internal readonly AudioCapsInfo Caps = new AudioCapsInfo();

	}
}
