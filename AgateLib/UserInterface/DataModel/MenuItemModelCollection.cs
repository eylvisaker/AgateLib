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
	public class MenuItemModelCollection : IList<MenuItemProperties>
	{
		List<MenuItemProperties> menuItems = new List<MenuItemProperties>();

		public MenuItemProperties this[int index]
		{
			get
			{
				return ((IList<MenuItemProperties>)menuItems)[index];
			}

			set
			{
				((IList<MenuItemProperties>)menuItems)[index] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IList<MenuItemProperties>)menuItems).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IList<MenuItemProperties>)menuItems).IsReadOnly;
			}
		}

		public void Add(MenuItemProperties item)
		{
			((IList<MenuItemProperties>)menuItems).Add(item);
		}

		public void Clear()
		{
			((IList<MenuItemProperties>)menuItems).Clear();
		}

		public bool Contains(MenuItemProperties item)
		{
			return ((IList<MenuItemProperties>)menuItems).Contains(item);
		}

		public void CopyTo(MenuItemProperties[] array, int arrayIndex)
		{
			((IList<MenuItemProperties>)menuItems).CopyTo(array, arrayIndex);
		}

		public IEnumerator<MenuItemProperties> GetEnumerator()
		{
			return ((IList<MenuItemProperties>)menuItems).GetEnumerator();
		}

		public int IndexOf(MenuItemProperties item)
		{
			return ((IList<MenuItemProperties>)menuItems).IndexOf(item);
		}

		public void Insert(int index, MenuItemProperties item)
		{
			((IList<MenuItemProperties>)menuItems).Insert(index, item);
		}

		public bool Remove(MenuItemProperties item)
		{
			return ((IList<MenuItemProperties>)menuItems).Remove(item);
		}

		public void RemoveAt(int index)
		{
			((IList<MenuItemProperties>)menuItems).RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IList<MenuItemProperties>)menuItems).GetEnumerator();
		}

		public override string ToString()
		{
			return $"Count = {Count}";
		}
	}
}