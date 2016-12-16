using System;
using System.Collections;
using System.Collections.Generic;

namespace AgateLib.UserInterface.DataModel
{
	public class FacetModel : IDictionary<string, WidgetProperties>
	{
		Dictionary<string, WidgetProperties> widgets = new Dictionary<string, WidgetProperties>();

		public WidgetProperties this[string key]
		{
			get
			{
				return ((IDictionary<string, WidgetProperties>)widgets)[key];
			}

			set
			{
				((IDictionary<string, WidgetProperties>)widgets)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, WidgetProperties>)widgets).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, WidgetProperties>)widgets).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, WidgetProperties>)widgets).Keys;
			}
		}

		public ICollection<WidgetProperties> Values
		{
			get
			{
				return ((IDictionary<string, WidgetProperties>)widgets).Values;
			}
		}

		public void Add(KeyValuePair<string, WidgetProperties> item)
		{
			((IDictionary<string, WidgetProperties>)widgets).Add(item);
		}

		public void Add(string key, WidgetProperties value)
		{
			((IDictionary<string, WidgetProperties>)widgets).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, WidgetProperties>)widgets).Clear();
		}

		public bool Contains(KeyValuePair<string, WidgetProperties> item)
		{
			return ((IDictionary<string, WidgetProperties>)widgets).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, WidgetProperties>)widgets).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, WidgetProperties>[] array, int arrayIndex)
		{
			((IDictionary<string, WidgetProperties>)widgets).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, WidgetProperties>> GetEnumerator()
		{
			return ((IDictionary<string, WidgetProperties>)widgets).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, WidgetProperties> item)
		{
			return ((IDictionary<string, WidgetProperties>)widgets).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, WidgetProperties>)widgets).Remove(key);
		}

		public bool TryGetValue(string key, out WidgetProperties value)
		{
			return ((IDictionary<string, WidgetProperties>)widgets).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, WidgetProperties>)widgets).GetEnumerator();
		}

		internal void Validate()
		{
			foreach(var wp in this.Values)
			{
				wp.Validate();
			}
		}
	}
}