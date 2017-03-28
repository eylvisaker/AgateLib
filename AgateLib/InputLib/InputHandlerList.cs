//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
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
