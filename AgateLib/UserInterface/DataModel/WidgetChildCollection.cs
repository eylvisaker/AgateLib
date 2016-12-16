using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.UserInterface.DataModel
{
	public class WidgetChildCollection : IDictionary<string, WidgetProperties>
	{
		List<KeyValuePair<string, WidgetProperties>> widgets = new List<KeyValuePair<string, WidgetProperties>>();

		public WidgetProperties this[string key]
		{
			get { return widgets.First(x => x.Key == key).Value; }
			set
			{
				Remove(key);
				Add(new KeyValuePair<string, WidgetProperties>(key, value));
			}
		}

		public int Count => widgets.Count;

		public ICollection<string> Keys => widgets.Select(x => x.Key).ToList();

		public ICollection<WidgetProperties> Values => widgets.Select(x => x.Value).ToList();

		bool ICollection<KeyValuePair<string, WidgetProperties>>.IsReadOnly => false;

		public void Add(KeyValuePair<string, WidgetProperties> item)
		{
			item.Value.Name = item.Key;
			widgets.Add(item);
		}

		public void Add(string name, WidgetProperties widget)
		{
			Add(new KeyValuePair<string, WidgetProperties>(name, widget));
		}

		public void Add(WidgetProperties widget)
		{
			Add(widget.Name, widget);
		}

		public void Clear()
		{
			widgets.Clear();
		}

		public bool Contains(KeyValuePair<string, WidgetProperties> item)
		{
			return widgets.Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return widgets.Any(x => x.Key == key);
		}

		public void CopyTo(KeyValuePair<string, WidgetProperties>[] array, int arrayIndex)
		{
			widgets.CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, WidgetProperties>> GetEnumerator()
		{
			return widgets.GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, WidgetProperties> item)
		{
			return widgets.Remove(item);
		}

		public bool Remove(string key)
		{
			return widgets.RemoveAll(x => x.Key == key) != 0;
		}

		public bool TryGetValue(string key, out WidgetProperties value)
		{
			var result = widgets.Any(x => x.Key == key);

			if (result)
			{
				value = widgets.First(x => x.Key == key).Value;
			}
			else
				value = null;

			return result;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override string ToString()
		{
			return $"Count = {Count}";
		}
	}
}