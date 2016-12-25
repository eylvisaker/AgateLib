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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.InputLib
{
	internal class InputHandlerList : IList<IInputHandler>
	{
		List<IInputHandler> mHandlers = new List<IInputHandler>();

		public int Count
		{
			get
			{
				return ((IList<IInputHandler>)mHandlers).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IList<IInputHandler>)mHandlers).IsReadOnly;
			}
		}

		public IInputHandler this[int index]
		{
			get
			{
				return ((IList<IInputHandler>)mHandlers)[index];
			}

			set
			{
				((IList<IInputHandler>)mHandlers)[index] = value;
			}
		}

		public InputHandlerList()
		{
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

		public int IndexOf(IInputHandler item)
		{
			return ((IList<IInputHandler>)mHandlers).IndexOf(item);
		}

		public void Insert(int index, IInputHandler item)
		{
			((IList<IInputHandler>)mHandlers).Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			((IList<IInputHandler>)mHandlers).RemoveAt(index);
		}

		public void Clear()
		{
			((IList<IInputHandler>)mHandlers).Clear();
		}

		public bool Contains(IInputHandler item)
		{
			return ((IList<IInputHandler>)mHandlers).Contains(item);
		}

		public void CopyTo(IInputHandler[] array, int arrayIndex)
		{
			((IList<IInputHandler>)mHandlers).CopyTo(array, arrayIndex);
		}

		public IEnumerator<IInputHandler> GetEnumerator()
		{
			return ((IList<IInputHandler>)mHandlers).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IList<IInputHandler>)mHandlers).GetEnumerator();
		}
	}
}
