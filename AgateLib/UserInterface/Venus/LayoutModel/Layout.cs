using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Venus.LayoutModel
{
	public class Layout : IDictionary<string, WidgetLayoutModel>
	{
		private Dictionary<string, WidgetLayoutModel> layouts = new Dictionary<string, WidgetLayoutModel>();

		public WidgetLayoutModel this[string key]
		{
			get
			{
				return ((IDictionary<string, WidgetLayoutModel>)layouts)[key];
			}

			set
			{
				((IDictionary<string, WidgetLayoutModel>)layouts)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, WidgetLayoutModel>)layouts).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, WidgetLayoutModel>)layouts).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, WidgetLayoutModel>)layouts).Keys;
			}
		}

		public ICollection<WidgetLayoutModel> Values
		{
			get
			{
				return ((IDictionary<string, WidgetLayoutModel>)layouts).Values;
			}
		}

		public void Add(KeyValuePair<string, WidgetLayoutModel> item)
		{
			((IDictionary<string, WidgetLayoutModel>)layouts).Add(item);
		}

		public void Add(string key, WidgetLayoutModel value)
		{
			((IDictionary<string, WidgetLayoutModel>)layouts).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, WidgetLayoutModel>)layouts).Clear();
		}

		public bool Contains(KeyValuePair<string, WidgetLayoutModel> item)
		{
			return ((IDictionary<string, WidgetLayoutModel>)layouts).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, WidgetLayoutModel>)layouts).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, WidgetLayoutModel>[] array, int arrayIndex)
		{
			((IDictionary<string, WidgetLayoutModel>)layouts).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, WidgetLayoutModel>> GetEnumerator()
		{
			return ((IDictionary<string, WidgetLayoutModel>)layouts).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, WidgetLayoutModel> item)
		{
			return ((IDictionary<string, WidgetLayoutModel>)layouts).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, WidgetLayoutModel>)layouts).Remove(key);
		}

		public bool TryGetValue(string key, out WidgetLayoutModel value)
		{
			return ((IDictionary<string, WidgetLayoutModel>)layouts).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, WidgetLayoutModel>)layouts).GetEnumerator();
		}
	}
}
