using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Configuration.State
{
	class AgateLibState
	{
		public AppModelState AppModel { get; private set; } = new AppModelState();
		public CoreState Core { get; private set; } = new CoreState();
		public DisplayState Display { get; private set; } = new DisplayState();
		public AudioState Audio { get; private set; } = new AudioState();
	}
}
