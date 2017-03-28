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

namespace AgateLib.UserInterface.DataModel
{
	public class FacetModel : IList<WidgetProperties>
	{
		List<WidgetProperties> widgets = new List<WidgetProperties>();

		public WidgetProperties this[int index]
		{
			get
			{
				return ((IList<WidgetProperties>)widgets)[index];
			}

			set
			{
				((IList<WidgetProperties>)widgets)[index] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IList<WidgetProperties>)widgets).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IList<WidgetProperties>)widgets).IsReadOnly;
			}
		}

		public void Add(WidgetProperties item)
		{
			((IList<WidgetProperties>)widgets).Add(item);
		}

		public void Clear()
		{
			((IList<WidgetProperties>)widgets).Clear();
		}

		public bool Contains(WidgetProperties item)
		{
			return ((IList<WidgetProperties>)widgets).Contains(item);
		}

		public void CopyTo(WidgetProperties[] array, int arrayIndex)
		{
			((IList<WidgetProperties>)widgets).CopyTo(array, arrayIndex);
		}

		public IEnumerator<WidgetProperties> GetEnumerator()
		{
			return ((IList<WidgetProperties>)widgets).GetEnumerator();
		}

		public int IndexOf(WidgetProperties item)
		{
			return ((IList<WidgetProperties>)widgets).IndexOf(item);
		}

		public void Insert(int index, WidgetProperties item)
		{
			((IList<WidgetProperties>)widgets).Insert(index, item);
		}

		public bool Remove(WidgetProperties item)
		{
			return ((IList<WidgetProperties>)widgets).Remove(item);
		}

		public void RemoveAt(int index)
		{
			((IList<WidgetProperties>)widgets).RemoveAt(index);
		}

		internal void Validate()
		{
			foreach(var wp in this)
			{
				wp.Validate();
			}
		}

		public override string ToString()
		{
			return $"Count: {Count}";
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IList<WidgetProperties>)widgets).GetEnumerator();
		}
	}
}