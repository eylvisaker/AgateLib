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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
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
	/// <summary>
	/// Static API for processing user input events. This works by having a stack of 
	/// IInputHandler objects which are called in a last first fashion. Each input handler
	/// has the opporunity to process an event or pass it to the next input handler.
	/// </summary>
	public static class Input
	{
		private static List<AgateInputEventArgs> EventQueue { get { return Core.State.Input.EventQueue; } }

		/// <summary>
		/// Enumerates the input handlers in the order they should process an event.
		/// </summary>
		private static IEnumerable<IInputHandler> AllInputHandlers
		{
			get
			{
				if (FirstInputHandler != null)
					yield return FirstInputHandler;

				for (int i = Handlers.Count - 1; i >= 0; i--)
				{
					yield return Handlers[i];
				}

				yield return Unhandled;
			}
		}

		private static IInputHandler FirstInputHandler { get { return Core.State.Input.FirstHandler; } }

		/// <summary>
		/// Last chance input handler for events which are not handled by any of the handlers on the 
		/// input stack. This can also be useful for small applications that don't need input handlers.
		/// </summary>
		public static SimpleInputHandler Unhandled { get { return Core.State.Input.Unhandled; } }

		/// <summary>
		/// A stack of handlers for processing user input events. The handlers are applied in order
		/// from last to first in the list, ie. Handlers[0] is the last one to process an
		/// event.
		/// </summary>
		public static IList<IInputHandler> Handlers { get { return Core.State.Input.Handlers; } }

		/// <summary>
		/// Adds an input event to the list of queued events that will be processed 
		/// at the next Core.KeepAlive call.
		/// </summary>
		/// <param name="args"></param>
		public static void QueueInputEvent(AgateInputEventArgs args)
		{
			lock (EventQueue)
			{
				EventQueue.Add(args);
			}
		}

		/// <summary>
		/// Dispatches every event in the queue to the event handlers.
		/// </summary>
		internal static void DispatchQueuedEvents()
		{
			while (EventQueue.Count > 0)
			{
				AgateInputEventArgs args;

				lock (EventQueue)
				{
					if (EventQueue.Count == 0)
						return;

					args = EventQueue[0];
					EventQueue.RemoveAt(0);
				}

				DispatchEvent(args);
			}
		}

		internal static void PollJoysticks()
		{
			JoystickInput.PollTimer();
		}

		private static void DispatchEvent(AgateInputEventArgs args)
		{
			foreach (var handler in AllInputHandlers)
			{
				handler.ProcessEvent(args);

				if (args.Handled)
					break;

				if (handler.ForwardUnhandledEvents == false)
					break;
			}
		}
	}
}
