using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.InputLib
{
	public static class Input
	{
		static List<AgateInputEventArgs> mEvents = new List<AgateInputEventArgs>();
		static InputHandlerList mInputHandlers = new InputHandlerList();

		public static void QueueInputEvent(AgateInputEventArgs args)
		{
			lock(mEvents)
			{
				mEvents.Add(args);
			}
		}

		public static void DispatchEvents()
		{
			while (mEvents.Count > 0)
			{
				AgateInputEventArgs args;

				lock(mEvents)
				{
					args = mEvents[0];
					mEvents.RemoveAt(0);
				}

				DispatchEvents(args);
			}
		}

		private static void DispatchEvents(AgateInputEventArgs args)
		{
			mInputHandlers.Dispatch(args);
		}

		public static InputHandlerList InputHandlers { get { return mInputHandlers; } }
	}
}
