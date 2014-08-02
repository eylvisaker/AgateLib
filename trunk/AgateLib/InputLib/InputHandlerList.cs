using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.InputLib
{
	public class InputHandlerList
	{
		List<IInputHandler> mHandlers = new List<IInputHandler>();

		public InputHandlerList()
		{
			Add(new AgateLib.InputLib.Legacy.LegacyInputHandler());
		}
		public void Add(IInputHandler handler)
		{
			if (mHandlers.Contains(handler))
				throw new InvalidOperationException("Cannot add the same input handler twice.");

			mHandlers.Add(handler);
		}
		public bool Remove(IInputHandler handler)
		{
			return mHandlers.Remove(handler);
		}

		public void Dispatch(AgateInputEventArgs args)
		{
			for (int i = mHandlers.Count - 1; i >= 0; i--)
			{
				var handler = mHandlers[i];

				handler.ProcessEvent(args);

				if (args.Handled)
					break;

				if (handler.ForwardUnhandledEvents == false)
					break;
			}
		}
	}
}
