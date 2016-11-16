using System.Collections;
using System.Collections.Generic;

namespace AgateLib.UserInterface.DataModel
{
	public class FontModelCollection : IDictionary<string, List<FontModel>>
	{
		Dictionary<string, List<FontModel>> fontModels = new Dictionary<string, List<FontModel>>();

		public List<FontModel> this[string key]
		{
			get
			{
				return ((IDictionary<string, List<FontModel>>)fontModels)[key];
			}

			set
			{
				((IDictionary<string, List<FontModel>>)fontModels)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, List<FontModel>>)fontModels).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, List<FontModel>>)fontModels).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, List<FontModel>>)fontModels).Keys;
			}
		}

		public ICollection<List<FontModel>> Values
		{
			get
			{
				return ((IDictionary<string, List<FontModel>>)fontModels).Values;
			}
		}

		public void Add(KeyValuePair<string, List<FontModel>> item)
		{
			((IDictionary<string, List<FontModel>>)fontModels).Add(item);
		}

		public void Add(string key, List<FontModel> value)
		{
			((IDictionary<string, List<FontModel>>)fontModels).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, List<FontModel>>)fontModels).Clear();
		}

		public bool Contains(KeyValuePair<string, List<FontModel>> item)
		{
			return ((IDictionary<string, List<FontModel>>)fontModels).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, List<FontModel>>)fontModels).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, List<FontModel>>[] array, int arrayIndex)
		{
			((IDictionary<string, List<FontModel>>)fontModels).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, List<FontModel>>> GetEnumerator()
		{
			return ((IDictionary<string, List<FontModel>>)fontModels).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, List<FontModel>> item)
		{
			return ((IDictionary<string, List<FontModel>>)fontModels).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, List<FontModel>>)fontModels).Remove(key);
		}

		public bool TryGetValue(string key, out List<FontModel> value)
		{
			return ((IDictionary<string, List<FontModel>>)fontModels).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, List<FontModel>>)fontModels).GetEnumerator();
		}
	}
}