using System.Collections;
using System.Collections.Generic;

namespace AgateLib.UserInterface.DataModel
{
	public class FontModelCollection : IDictionary<string, FontModel>
	{
		Dictionary<string, FontModel> fontModels = new Dictionary<string, FontModel>();

		public FontModel this[string key]
		{
			get
			{
				return ((IDictionary<string, FontModel>)fontModels)[key];
			}

			set
			{
				((IDictionary<string, FontModel>)fontModels)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, FontModel>)fontModels).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, FontModel>)fontModels).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, FontModel>)fontModels).Keys;
			}
		}

		public ICollection<FontModel> Values
		{
			get
			{
				return ((IDictionary<string, FontModel>)fontModels).Values;
			}
		}

		public void Add(KeyValuePair<string, FontModel> item)
		{
			((IDictionary<string, FontModel>)fontModels).Add(item);
		}

		public void Add(string key, FontModel value)
		{
			((IDictionary<string, FontModel>)fontModels).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, FontModel>)fontModels).Clear();
		}

		public bool Contains(KeyValuePair<string, FontModel> item)
		{
			return ((IDictionary<string, FontModel>)fontModels).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, FontModel>)fontModels).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, FontModel>[] array, int arrayIndex)
		{
			((IDictionary<string, FontModel>)fontModels).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, FontModel>> GetEnumerator()
		{
			return ((IDictionary<string, FontModel>)fontModels).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, FontModel> item)
		{
			return ((IDictionary<string, FontModel>)fontModels).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, FontModel>)fontModels).Remove(key);
		}

		public bool TryGetValue(string key, out FontModel value)
		{
			return ((IDictionary<string, FontModel>)fontModels).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, FontModel>)fontModels).GetEnumerator();
		}
	}
}