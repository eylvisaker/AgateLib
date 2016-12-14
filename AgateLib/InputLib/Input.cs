//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.InputLib
{
	public static class Input
	{
		static List<AgateInputEventArgs> eventQueue { get { return Core.State.Input.EventQueue; } }
		static InputHandlerList inputHandlers { get { return Core.State.Input.Handlers; } }

		public static void QueueInputEvent(AgateInputEventArgs args)
		{
			lock (eventQueue)
			{
				eventQueue.Add(args);
			}
		}

		public static void DispatchQueuedEvents()
		{
			while (eventQueue.Count > 0)
			{
				AgateInputEventArgs args;

				lock (eventQueue)
				{
					if (eventQueue.Count == 0)
						return;

					args = eventQueue[0];
					eventQueue.RemoveAt(0);
				}
			
				DispatchEvent(args);
			}
		}

		private static void DispatchEvent(AgateInputEventArgs args)
		{
			inputHandlers.Dispatch(args);
		}

		public static InputHandlerList InputHandlers { get { return inputHandlers; } }

		internal static void PollJoysticks()
		{
			JoystickInput.PollTimer();
		}
	}
}
