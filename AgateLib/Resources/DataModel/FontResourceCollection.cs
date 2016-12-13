using System.Collections;
using System.Collections.Generic;
using AgateLib.DisplayLib.BitmapFont;

namespace AgateLib.Resources.DataModel
{
	public class FontResourceCollection : IDictionary<string, FontResource>
	{
		Dictionary<string, FontResource> fontModels = new Dictionary<string, FontResource>();

		public FontResource this[string key]
		{
			get
			{
				return ((IDictionary<string, FontResource>)fontModels)[key];
			}

			set
			{
				((IDictionary<string, FontResource>)fontModels)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, FontResource>)fontModels).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, FontResource>)fontModels).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, FontResource>)fontModels).Keys;
			}
		}

		public ICollection<FontResource> Values
		{
			get
			{
				return ((IDictionary<string, FontResource>)fontModels).Values;
			}
		}

		public void Add(KeyValuePair<string, FontResource> item)
		{
			((IDictionary<string, FontResource>)fontModels).Add(item);
		}

		public void Add(string key, FontResource value)
		{
			((IDictionary<string, FontResource>)fontModels).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, FontResource>)fontModels).Clear();
		}

		public bool Contains(KeyValuePair<string, FontResource> item)
		{
			return ((IDictionary<string, FontResource>)fontModels).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, FontResource>)fontModels).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, FontResource>[] array, int arrayIndex)
		{
			((IDictionary<string, FontResource>)fontModels).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, FontResource>> GetEnumerator()
		{
			return ((IDictionary<string, FontResource>)fontModels).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, FontResource> item)
		{
			return ((IDictionary<string, FontResource>)fontModels).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, FontResource>)fontModels).Remove(key);
		}

		public bool TryGetValue(string key, out FontResource value)
		{
			return ((IDictionary<string, FontResource>)fontModels).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, FontResource>)fontModels).GetEnumerator();
		}
	}
}