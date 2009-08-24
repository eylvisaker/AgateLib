using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Gui
{
	class ListBox : Widget 
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
