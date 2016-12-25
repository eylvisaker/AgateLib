using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Configuration.State
{
	class AgateLibState
	{
		public AgateLibState()
		{
			Input.FirstHandler = Console.Instance;
		}

		public AppModelState AppModel { get; private set; } = new AppModelState();
		public CoreState Core { get; private set; } = new CoreState();
		public ConsoleState Console { get; private set; } = new ConsoleState();

		public AudioState Audio { get; private set; } = new AudioState();
		public DisplayState Display { get; private set; } = new DisplayState();
		public InputState Input { get; private set; } = new InputState();

		public IOState IO { get; private set; } = new IOState();
		public bool Debug { get; internal set; }
	}
}
