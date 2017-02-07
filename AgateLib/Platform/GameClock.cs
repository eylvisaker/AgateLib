using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Configuration;

namespace AgateLib.Platform
{
	public class GameClock : IStopClock
	{
		private IStopClock parent;

		public GameClock(IStopClock parent)
		{
			this.parent = parent;
		}

		public TimeSpan DeltaTime { get; private set; }
	}

	public interface IStopClock
	{
	}
}
