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