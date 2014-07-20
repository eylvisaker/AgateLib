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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AgateLib.Resources
{
	/// <summary>
	/// Class which contains a table of string-string mappings.
	/// Similar to a Dictionary, but includes methods for reading to / writing from
	/// an AgateLib resource file.
	/// </summary>
	public sealed class StringTable : AgateResource, IDictionary<string, string>
	{
		Dictionary<string, string> mTable = new Dictionary<string, string>();

		internal StringTable()
			: base("StringTable")
		{ }
		internal StringTable(XElement node, string version)
			: base("StringTable")
		{
			switch (version)
			{
				case "0.3.2":
				case "0.3.1":
				case "0.3.0":
					foreach(var stringNode in node.Elements())
					{
						if (stringNode.Name != "string")
							throw new AgateResourceException(
								"Invalid node appeared in string table.");
						if (stringNode.Attribute("name") == null)
							throw new AgateResourceException(
								"Unnamed string node found.");

						string key = stringNode.Attribute("name").Value;
						string value = stringNode.Value;

						mTable.Add(key, value);
					}
					break;
			}
		}

		internal void Combine(StringTable strings)
		{
			foreach (string key in strings.mTable.Keys)
			{
				mTable.Remove(key);
				mTable.Add(key, strings.mTable[key]);
			}
		}

		internal override void BuildNodes(XElement parent)
		{
			XElement element = new XElement("StringTable");

			foreach (string keyName in mTable.Keys)
			{
				if (string.IsNullOrEmpty(mTable[keyName]))
					continue;

				XElement key = new XElement("string");
				key.Add(new XAttribute("name", keyName));

				key.Value = mTable[keyName];

				element.Add(key);
			}

			parent.Add(element);
		}

		/// <summary>
		/// Clones the string table.
		/// </summary>
		/// <returns></returns>
		protected override AgateResource Clone()
		{
			return new StringTable();
		}

		#region --- IDictionary<string,string> Members ---

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Add(string key, string value)
		{
			mTable.Add(key, value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainsKey(string key)
		{
			return mTable.ContainsKey(key);
		}

		/// <summary>
		/// 
		/// </summary>
		public ICollection<string> Keys
		{
			get { return mTable.Keys; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Remove(string key)
		{
			return mTable.Remove(key);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool TryGetValue(string key, out string value)
		{
			return mTable.TryGetValue(key, out value);
		}

		/// <summary>
		/// 
		/// </summary>
		public ICollection<string> Values
		{
			get { return mTable.Values; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string this[string key]
		{
			get
			{
				return mTable[key];
			}
			set
			{
				mTable[key] = value;
			}
		}

		#endregion
		#region --- ICollection<KeyValuePair<string,string>> Members ---

		void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> item)
		{
			((ICollection<KeyValuePair<string, string>>)mTable).Add(item);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Clear()
		{
			mTable.Clear();
		}

		bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> item)
		{
			return ((ICollection<KeyValuePair<string, string>>)mTable).Contains(item);
		}

		void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, string>>)mTable).CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get { return mTable.Count; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}

		bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item)
		{
			return ((ICollection<KeyValuePair<string, string>>)mTable).Remove(item);
		}

		#endregion
		#region --- IEnumerable<KeyValuePair<string,string>> Members ---

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return mTable.GetEnumerator();
		}

		#endregion
		#region --- IEnumerable Members ---

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return mTable.GetEnumerator();
		}

		#endregion

	}
}
