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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.InputLib
{
	internal class InputHandlerList : IList<IInputHandler>
	{
		List<IInputHandler> handlers = new List<IInputHandler>();

		public event EventHandler<InputHandlerEventArgs> HandlerAdded;
		public event EventHandler<InputHandlerEventArgs> HandlerRemoved;

		public int Count => handlers.Count;

		public bool IsReadOnly => false;

		public IInputHandler this[int index]
		{
			get { return handlers[index]; }
			set
			{
				HandlerRemoved?.Invoke(this, new InputHandlerEventArgs(handlers[index]));

				handlers[index] = value;

				HandlerAdded?.Invoke(this, new InputHandlerEventArgs(handlers[index]));
			}
		}

		public void Add(IInputHandler handler)
		{
			if (handler == null)
				throw new ArgumentNullException("Cannot add a null input handler.");
			if (handlers.Contains(handler))
				throw new InvalidOperationException("Cannot add the same input handler twice.");

			handlers.Add(handler);
			HandlerAdded?.Invoke(this, new InputHandlerEventArgs(handler));
		}

		public bool Remove(IInputHandler handler)
		{
			var result = handlers.Remove(handler);

			if (result)
				HandlerRemoved?.Invoke(this, new InputHandlerEventArgs(handler));

			return result;
		}

		internal void BringToTop(IInputHandler handler)
		{
			if (handlers.Contains(handler) == false)
				throw new InvalidOperationException("Cannot move a handler which is not registered.");

			handlers.Remove(handler);
			handlers.Add(handler);
		}

		internal void SendToBack(IInputHandler handler)
		{
			if (handlers.Contains(handler) == false)
				throw new InvalidOperationException("Cannot move a handler which is not registered.");

			handlers.Remove(handler);
			handlers.Insert(0, handler);
		}

		public int IndexOf(IInputHandler item)
		{
			return handlers.IndexOf(item);
		}

		public void Insert(int index, IInputHandler item)
		{
			handlers.Insert(index, item);

			HandlerAdded?.Invoke(this, new InputHandlerEventArgs(item));
		}

		public void RemoveAt(int index)
		{
			HandlerRemoved?.Invoke(this, new InputHandlerEventArgs(this[index]));

			handlers.RemoveAt(index);
		}

		public void Clear()
		{
			foreach(var handler in handlers)
			{
				HandlerRemoved?.Invoke(this, new InputHandlerEventArgs(handler));
			}

			handlers.Clear();
		}

		public bool Contains(IInputHandler item)
		{
			return handlers.Contains(item);
		}

		public void CopyTo(IInputHandler[] array, int arrayIndex)
		{
			handlers.CopyTo(array, arrayIndex);
		}

		public IEnumerator<IInputHandler> GetEnumerator()
		{
			return handlers.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return handlers.GetEnumerator();
		}
	}

	internal class InputHandlerEventArgs : EventArgs
	{
		public InputHandlerEventArgs(IInputHandler handler)
		{
			this.Handler = handler;
		}

		public IInputHandler Handler { get; private set; }
	}
}
