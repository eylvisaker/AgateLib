using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Settings
{
	public class SettingsGroup : IDictionary<string, string>
	{
		Dictionary<string, string> mStore = new Dictionary<string, string>();

		/// <summary>
		/// Returns true if this settings group has no members.
		/// </summary>
		public bool IsEmpty
		{
			get { return Count == 0; }
		}

		#region IDictionary<string,string> Members

		public void Add(string key, string value)
		{
			mStore.Add(key, value);
		}

		public bool ContainsKey(string key)
		{
			return mStore.ContainsKey(key);
		}

		public ICollection<string> Keys
		{
			get { return mStore.Keys; }
		}

		public bool Remove(string key)
		{
			return mStore.Remove(key);
		}

		public bool TryGetValue(string key, out string value)
		{
			return mStore.TryGetValue(key, out value);
		}

		public ICollection<string> Values
		{
			get { return mStore.Values; }
		}

		public string this[string key]
		{
			get
			{
				return mStore[key];
			}
			set
			{
				mStore[key] = value;
			}
		}

		#endregion

		#region ICollection<KeyValuePair<string,string>> Members

		void ICollection<KeyValuePair<string,string>>.Add(KeyValuePair<string, string> item)
		{
			Add(item.Key, item.Value);
		}

		public void Clear()
		{
			mStore.Clear();
		}

		bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> item)
		{
			return ((ICollection<KeyValuePair<string, string>>)mStore).Contains(item);
		}

		void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, string>>)mStore).CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return mStore.Count; }
		}

		bool ICollection<KeyValuePair<string, string>>.IsReadOnly
		{
			get { return false; }
		}

		bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item)
		{
			return Remove(item.Key);
		}

		#endregion

		#region IEnumerable<KeyValuePair<string,string>> Members

		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return mStore.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
