using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Configuration;

namespace AgateLib.Platform
{
	public class MasterClock : IStopClock
	{
		private IClock root;

		public MasterClock(IClock rootClock)
		{
			this.root = rootClock;
		}

		public void Advance()
		{
			throw new NotImplementedException();
		}
	}
}
