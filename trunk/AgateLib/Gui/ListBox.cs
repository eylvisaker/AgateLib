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
	public class ListBox : Widget 
	{
		List<object> mItems = new List<object>();
		ListBoxItemText mItemTextGetter;

		public ListBox()
		{
			mItemTextGetter = ItemToString;
		}

		string ItemToString(object item)
		{
			return item.ToString();
		}

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

		public string GetItemText(int index)
		{
			return mItemTextGetter(Items[index]);
		}
		
	}

	public delegate string ListBoxItemText(object item);
}
