using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.StaticState
{
	class InternalState
	{
		public AppModelState AppModel { get; private set; } = new AppModelState();
		public CoreState Core { get; private set; } = new CoreState();
	}
}
