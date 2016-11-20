using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Venus.Fulfillment
{
	public class FacetWidgetPropertyMap : IDictionary<string, FacetWidgetPropertyMapValue>
	{
		Dictionary<string, FacetWidgetPropertyMapValue> map = new Dictionary<string, FacetWidgetPropertyMapValue>();

		public FacetWidgetPropertyMapValue this[string key]
		{
			get
			{
				return ((IDictionary<string, FacetWidgetPropertyMapValue>)map)[key];
			}

			set
			{
				((IDictionary<string, FacetWidgetPropertyMapValue>)map)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, FacetWidgetPropertyMapValue>)map).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, FacetWidgetPropertyMapValue>)map).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, FacetWidgetPropertyMapValue>)map).Keys;
			}
		}

		public ICollection<FacetWidgetPropertyMapValue> Values
		{
			get
			{
				return ((IDictionary<string, FacetWidgetPropertyMapValue>)map).Values;
			}
		}

		public void Add(KeyValuePair<string, FacetWidgetPropertyMapValue> item)
		{
			((IDictionary<string, FacetWidgetPropertyMapValue>)map).Add(item);
		}

		public void Add(string key, FacetWidgetPropertyMapValue value)
		{
			((IDictionary<string, FacetWidgetPropertyMapValue>)map).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, FacetWidgetPropertyMapValue>)map).Clear();
		}

		public bool Contains(KeyValuePair<string, FacetWidgetPropertyMapValue> item)
		{
			return ((IDictionary<string, FacetWidgetPropertyMapValue>)map).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, FacetWidgetPropertyMapValue>)map).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, FacetWidgetPropertyMapValue>[] array, int arrayIndex)
		{
			((IDictionary<string, FacetWidgetPropertyMapValue>)map).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, FacetWidgetPropertyMapValue>> GetEnumerator()
		{
			return ((IDictionary<string, FacetWidgetPropertyMapValue>)map).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, FacetWidgetPropertyMapValue> item)
		{
			return ((IDictionary<string, FacetWidgetPropertyMapValue>)map).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, FacetWidgetPropertyMapValue>)map).Remove(key);
		}

		public bool TryGetValue(string key, out FacetWidgetPropertyMapValue value)
		{
			return ((IDictionary<string, FacetWidgetPropertyMapValue>)map).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, FacetWidgetPropertyMapValue>)map).GetEnumerator();
		}
	}
}
