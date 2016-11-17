using System.Collections;
using System.Collections.Generic;

namespace AgateLib.UserInterface.DataModel
{
	public class FacetModel : IDictionary<string, WidgetModel>
	{
		Dictionary<string, WidgetModel> widgets = new Dictionary<string, WidgetModel>();

		public WidgetModel this[string key]
		{
			get
			{
				return ((IDictionary<string, WidgetModel>)widgets)[key];
			}

			set
			{
				((IDictionary<string, WidgetModel>)widgets)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, WidgetModel>)widgets).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, WidgetModel>)widgets).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, WidgetModel>)widgets).Keys;
			}
		}

		public ICollection<WidgetModel> Values
		{
			get
			{
				return ((IDictionary<string, WidgetModel>)widgets).Values;
			}
		}

		public void Add(KeyValuePair<string, WidgetModel> item)
		{
			((IDictionary<string, WidgetModel>)widgets).Add(item);
		}

		public void Add(string key, WidgetModel value)
		{
			((IDictionary<string, WidgetModel>)widgets).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, WidgetModel>)widgets).Clear();
		}

		public bool Contains(KeyValuePair<string, WidgetModel> item)
		{
			return ((IDictionary<string, WidgetModel>)widgets).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, WidgetModel>)widgets).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, WidgetModel>[] array, int arrayIndex)
		{
			((IDictionary<string, WidgetModel>)widgets).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, WidgetModel>> GetEnumerator()
		{
			return ((IDictionary<string, WidgetModel>)widgets).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, WidgetModel> item)
		{
			return ((IDictionary<string, WidgetModel>)widgets).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, WidgetModel>)widgets).Remove(key);
		}

		public bool TryGetValue(string key, out WidgetModel value)
		{
			return ((IDictionary<string, WidgetModel>)widgets).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, WidgetModel>)widgets).GetEnumerator();
		}
	}
}