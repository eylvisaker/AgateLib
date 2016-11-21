using System;
using System.Collections;
using System.Collections.Generic;

namespace AgateLib.UserInterface.DataModel
{
	public class ThemeModel : IDictionary<string, WidgetThemeModel>
	{
		Dictionary<string, WidgetThemeModel> widgets = 
			new Dictionary<string, WidgetThemeModel>(StringComparer.OrdinalIgnoreCase);

		public WidgetThemeModel this[string key]
		{
			get
			{
				return ((IDictionary<string, WidgetThemeModel>)widgets)[key];
			}

			set
			{
				((IDictionary<string, WidgetThemeModel>)widgets)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, WidgetThemeModel>)widgets).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, WidgetThemeModel>)widgets).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, WidgetThemeModel>)widgets).Keys;
			}
		}

		public ICollection<WidgetThemeModel> Values
		{
			get
			{
				return ((IDictionary<string, WidgetThemeModel>)widgets).Values;
			}
		}

		public void Add(KeyValuePair<string, WidgetThemeModel> item)
		{
			((IDictionary<string, WidgetThemeModel>)widgets).Add(item);
		}

		public void Add(string key, WidgetThemeModel value)
		{
			((IDictionary<string, WidgetThemeModel>)widgets).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, WidgetThemeModel>)widgets).Clear();
		}

		public bool Contains(KeyValuePair<string, WidgetThemeModel> item)
		{
			return ((IDictionary<string, WidgetThemeModel>)widgets).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, WidgetThemeModel>)widgets).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, WidgetThemeModel>[] array, int arrayIndex)
		{
			((IDictionary<string, WidgetThemeModel>)widgets).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, WidgetThemeModel>> GetEnumerator()
		{
			return ((IDictionary<string, WidgetThemeModel>)widgets).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, WidgetThemeModel> item)
		{
			return ((IDictionary<string, WidgetThemeModel>)widgets).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, WidgetThemeModel>)widgets).Remove(key);
		}

		public bool TryGetValue(string key, out WidgetThemeModel value)
		{
			return ((IDictionary<string, WidgetThemeModel>)widgets).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, WidgetThemeModel>)widgets).GetEnumerator();
		}
	}
}