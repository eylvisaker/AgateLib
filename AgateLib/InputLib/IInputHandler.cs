using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.InputLib
{
	public interface IInputHandler
	{
		void ProcessEvent(AgateInputEventArgs args);

		bool ForwardUnhandledEvents { get; }
	}
}
