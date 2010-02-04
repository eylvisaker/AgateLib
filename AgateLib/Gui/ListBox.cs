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
	/// A GUI widget which contains a list of items.
	/// </summary>
	public class ListBox : Widget 
	{
		List<object> mItems = new List<object>();
		ListBoxItemText mItemTextGetter;

		/// <summary>
		/// Constructs a ListBox object.
		/// </summary>
		public ListBox()
		{
			mItemTextGetter = ItemToString;
		}

		string ItemToString(object item)
		{
			return item.ToString();
		}

		/// <summary>
		/// Gets or sets the collection of objects in the list.
		/// </summary>
		public List<object> Items
		{
			get { return mItems; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("Cannot set the ListBox's item collection to null.");

				mItems = value;
			}
		}
		/// <summary>
		/// Gets or sets the delegate that produces strings to display for items in a list box.
		/// </summary>
		public ListBoxItemText ItemTextGetter
		{
			get { return mItemTextGetter; }
			set
			{
				if (value == null)
					mItemTextGetter = ItemToString;
				else
					mItemTextGetter = value;
			}
		}

		/// <summary>
		/// Gets the text that will be displayed for the specified item.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public string GetItemText(int index)
		{
			return mItemTextGetter(Items[index]);
		}
		
	}

	/// <summary>
	/// Delegate which converts an item to a string for display in a ListBox.
	/// </summary>
	/// <param name="item"></param>
	/// <returns></returns>
	public delegate string ListBoxItemText(object item);
}
