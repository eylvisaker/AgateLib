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
	public class InputHandlerList
	{
		List<IInputHandler> mHandlers = new List<IInputHandler>();

		public InputHandlerList()
		{
			Add(new AgateLib.InputLib.Legacy.LegacyInputHandler());
		}

		public void Add(IInputHandler handler)
		{
			if (handler == null)
				throw new ArgumentNullException("Cannot add a null input handler.");
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

		internal void BringToTop(IInputHandler handler)
		{
			if (mHandlers.Contains(handler) == false)
				throw new InvalidOperationException("Cannot move a handler which is not registered.");

			mHandlers.Remove(handler);
			mHandlers.Add(handler);
		}

		internal void SendToBack(IInputHandler handler)
		{
			if (mHandlers.Contains(handler) == false)
				throw new InvalidOperationException("Cannot move a handler which is not registered.");

			mHandlers.Remove(handler);
			mHandlers.Insert(0, handler);
		}
	}
}
