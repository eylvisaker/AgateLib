//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; internal set; }
		
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="AgateLib.Settings.SettingsGroup"/> is in
		/// debugging mode. If true, every access to a member will be echoed to System.Diagnostics.Trace.
		/// </summary>
		/// <value>
		/// <c>true</c> if debug; otherwise, <c>false</c>.
		/// </value>
		public bool Debug { get; set; }
		
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
			
			if (Debug)
			{
				Trace.WriteLine(string.Format("Settings[\"{0}\"][\"{1}\"] written.", Name, key));
			}
		}

		/// <summary>
		/// Returns whether or not the specified key is present.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainsKey(string key)
		{
			if (Debug)
			{
				Trace.WriteLine(string.Format("Settings[\"{0}\"][\"{1}\"] checked.", Name, key));
			}

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
			if (Debug)
			{
				Trace.WriteLine(string.Format("Settings[\"{0}\"][\"{1}\"] checked.", Name, key));
			}

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
			get 
			{ 
				if (Debug)
				{
					Trace.WriteLine(string.Format("Settings[\"{0}\"][\"{1}\"] checked.", Name, key));
				}
				return mStore[key]; 
			}
			set
			{
				if (Debug)
				{
					Trace.WriteLine(string.Format("Settings[\"{0}\"][\"{1}\"] written.", Name, key));
				} 
				mStore[key] = value; 
			}
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
