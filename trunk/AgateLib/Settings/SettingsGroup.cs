using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Settings
{
	/// <summary>
	/// A group of settings.  This is essentially just a Dictionary object 
	/// where both key and value types are strings.
	/// </summary>
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

		#region --- IDictionary<string,string> Members ---

		/// <summary>
		/// Adds a setting to the group.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Add(string key, string value)
		{
			mStore.Add(key, value);
		}

		/// <summary>
		/// Returns whether or not the specified key is present.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainsKey(string key)
		{
			return mStore.ContainsKey(key);
		}

		/// <summary>
		/// Gets the collection of keys.
		/// </summary>
		public ICollection<string> Keys
		{
			get { return mStore.Keys; }
		}

		/// <summary>
		/// Removes a key/value pair.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Remove(string key)
		{
			return mStore.Remove(key);
		}
		/// <summary>
		/// Trys to get a value from the SettingsGroup.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool TryGetValue(string key, out string value)
		{
			return mStore.TryGetValue(key, out value);
		}

		/// <summary>
		/// Gets a collection of the values.
		/// </summary>
		public ICollection<string> Values
		{
			get { return mStore.Values; }
		}

		/// <summary>
		/// Gets or sets a value in the SettingsGroup.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string this[string key]
		{
			get { return mStore[key]; }
			set { mStore[key] = value; }
		}

		#endregion
		#region --- ICollection<KeyValuePair<string,string>> Members ---

		/// <summary>
		/// Returns the number of settings in the group.
		/// </summary>
		public int Count
		{
			get { return mStore.Count; }
		}
		/// <summary>
		/// Clears the settings from the group.
		/// </summary>
		public void Clear()
		{
			mStore.Clear();
		}

		void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> item)
		{
			Add(item.Key, item.Value);
		}
		void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, string>>)mStore).CopyTo(array, arrayIndex);
		}
		bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> item)
		{
			return ((ICollection<KeyValuePair<string, string>>)mStore).Contains(item);
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
		#region --- IEnumerable<KeyValuePair<string,string>> Members ---

		/// <summary>
		/// Enumerates the KeyValuePair objects.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return mStore.GetEnumerator();
		}

		#endregion
		#region --- IEnumerable Members ---

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
