using System.Collections;
using System.Collections.Generic;
using AgateLib.UserInterface.DataModel;

namespace AgateLib.Resources.DataModel
{
	public class ThemeModelCollection : IDictionary<string, ThemeModel>
	{
		Dictionary<string, ThemeModel> themes = new Dictionary<string, ThemeModel>();

		public ThemeModel this[string key]
		{
			get
			{
				return ((IDictionary<string, ThemeModel>)themes)[key];
			}

			set
			{
				((IDictionary<string, ThemeModel>)themes)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, ThemeModel>)themes).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, ThemeModel>)themes).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, ThemeModel>)themes).Keys;
			}
		}

		public ICollection<ThemeModel> Values
		{
			get
			{
				return ((IDictionary<string, ThemeModel>)themes).Values;
			}
		}

		public void Add(KeyValuePair<string, ThemeModel> item)
		{
			((IDictionary<string, ThemeModel>)themes).Add(item);
		}

		public void Add(string key, ThemeModel value)
		{
			((IDictionary<string, ThemeModel>)themes).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, ThemeModel>)themes).Clear();
		}

		public bool Contains(KeyValuePair<string, ThemeModel> item)
		{
			return ((IDictionary<string, ThemeModel>)themes).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, ThemeModel>)themes).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, ThemeModel>[] array, int arrayIndex)
		{
			((IDictionary<string, ThemeModel>)themes).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, ThemeModel>> GetEnumerator()
		{
			return ((IDictionary<string, ThemeModel>)themes).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, ThemeModel> item)
		{
			return ((IDictionary<string, ThemeModel>)themes).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, ThemeModel>)themes).Remove(key);
		}

		public bool TryGetValue(string key, out ThemeModel value)
		{
			return ((IDictionary<string, ThemeModel>)themes).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, ThemeModel>)themes).GetEnumerator();
		}
	}
}