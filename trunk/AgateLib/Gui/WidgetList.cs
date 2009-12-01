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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Gui
{
	/// <summary>
	/// WidgetList implements IList&lt;Widget&rt; for keeping track 
	/// of children of a container.
	/// </summary>
	public sealed class WidgetList : IList<Widget>
	{
		List<Widget> mChildren = new List<Widget>();
		Container mOwner;

		/// <summary>
		/// Constructs a WidgetList object.
		/// </summary>
		/// <param name="owner"></param>
		public WidgetList(Container owner)
		{
			mOwner = owner;
		}
		private void AddChild(Widget child)
		{
			if (child.Parent != null)
				throw new AgateGuiException("The passed widget already has a parent.");

			child.Parent = mOwner;
			mChildren.Add(child);

			OnListUpdated();
		}
		private void RemoveAllChildren()
		{
			if (mChildren.Count == 0)
				return;

			foreach (Widget child in mChildren)
				child.Parent = null;

			mChildren.Clear();

			OnListUpdated();
		}
		private void RemoveChild(Widget child)
		{
			if (mChildren.Remove(child))
			{
				child.Parent = null;
				OnListUpdated();
			}
		}

		void OnListUpdated()
		{
			if (ListUpdated != null)
				ListUpdated(this, EventArgs.Empty);
		}
		/// <summary>
		/// Event raised when something is added or removed from the list.
		/// </summary>
		public event EventHandler ListUpdated;

		/// <summary>
		/// Enumerates the widgets in the list which have their
		/// <c>Widget.Visible</c>
		/// property
		/// set to true.
		/// </summary>
		public IEnumerable<Widget> VisibleItems
		{
			get
			{
				return mChildren.Where(x => x.Visible);
			}
		}

		#region IList<Widget> Members

		/// <summary>
		/// Returns the index of the specified widget.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(Widget item)
		{
			return mChildren.IndexOf(item);
		}
		/// <summary>
		/// Inserts the widget into the list.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, Widget item)
		{
			if (item == null)
				throw new ArgumentNullException("item");
			if (item.Parent != null)
				throw new AgateGuiException("The passed widget already has a parent.");

			item.Parent = mOwner;
			mChildren.Insert(index, item);

			OnListUpdated();
		}
		/// <summary>
		/// Removes the specified item from the list.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= mChildren.Count)
				throw new IndexOutOfRangeException();

			mChildren[index].Parent = null;
			mChildren.RemoveAt(index);

			OnListUpdated();
		}
		/// <summary>
		/// Gets or sets the widget at the specified index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Widget this[int index]
		{
			get
			{
				return mChildren[index];
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException();
				if (index < 0 || index >= mChildren.Count)
					throw new IndexOutOfRangeException();

				if (value == mChildren[index])
					return;

				value.Parent = mOwner;
				mChildren[index].Parent = null;
				mChildren[index] = value;

				OnListUpdated();
			}
		}

		#endregion
		#region ICollection<Widget> Members

		/// <summary>
		/// Adds an item to the list.
		/// </summary>
		/// <param name="item"></param>
		public void Add(Widget item)
		{
			AddChild(item);
		}
		/// <summary>
		/// Removes all widgets from the list.
		/// </summary>
		public void Clear()
		{
			RemoveAllChildren();
		}
		/// <summary>
		/// Returns true of the specified widget is in this list.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(Widget item)
		{
			return mChildren.Contains(item);
		}
		/// <summary>
		/// Copies the list of widgets to an array.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		void ICollection<Widget>.CopyTo(Widget[] array, int arrayIndex)
		{
			mChildren.CopyTo(array, arrayIndex);
		}
		/// <summary>
		/// Returns the number of widgets in the list.
		/// </summary>
		public int Count
		{
			get { return mChildren.Count; }
		}

		bool ICollection<Widget>.IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Removes an item from the list.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(Widget item)
		{
			if (mChildren.Contains(item))
			{
				RemoveChild(item);
				return true;
			}
			else
				return false;
		}

		#endregion
		#region IEnumerable<Widget> Members

		/// <summary>
		/// Enumerates all items in the list.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<Widget> GetEnumerator()
		{
			return mChildren.GetEnumerator();
		}

		#endregion
		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
