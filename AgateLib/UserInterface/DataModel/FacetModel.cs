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