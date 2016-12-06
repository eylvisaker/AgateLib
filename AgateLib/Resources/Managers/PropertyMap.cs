using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Resources.Managers.UserInterface
{
	public class PropertyMap<T> : IDictionary<string, PropertyMapValue<T>>
	{
		Dictionary<string, PropertyMapValue<T>> map = new Dictionary<string, PropertyMapValue<T>>();

		public PropertyMapValue<T> this[string key]
		{
			get
			{
				return ((IDictionary<string, PropertyMapValue<T>>)map)[key];
			}

			set
			{
				((IDictionary<string, PropertyMapValue<T>>)map)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, PropertyMapValue<T>>)map).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, PropertyMapValue<T>>)map).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, PropertyMapValue<T>>)map).Keys;
			}
		}

		public ICollection<PropertyMapValue<T>> Values
		{
			get
			{
				return ((IDictionary<string, PropertyMapValue<T>>)map).Values;
			}
		}

		public void Add(KeyValuePair<string, PropertyMapValue<T>> item)
		{
			((IDictionary<string, PropertyMapValue<T>>)map).Add(item);
		}

		public void Add(string key, PropertyMapValue<T> value)
		{
			((IDictionary<string, PropertyMapValue<T>>)map).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, PropertyMapValue<T>>)map).Clear();
		}

		public bool Contains(KeyValuePair<string, PropertyMapValue<T>> item)
		{
			return ((IDictionary<string, PropertyMapValue<T>>)map).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, PropertyMapValue<T>>)map).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, PropertyMapValue<T>>[] array, int arrayIndex)
		{
			((IDictionary<string, PropertyMapValue<T>>)map).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, PropertyMapValue<T>>> GetEnumerator()
		{
			return ((IDictionary<string, PropertyMapValue<T>>)map).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, PropertyMapValue<T>> item)
		{
			return ((IDictionary<string, PropertyMapValue<T>>)map).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, PropertyMapValue<T>>)map).Remove(key);
		}

		public bool TryGetValue(string key, out PropertyMapValue<T> value)
		{
			return ((IDictionary<string, PropertyMapValue<T>>)map).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, PropertyMapValue<T>>)map).GetEnumerator();
		}
	}
}
