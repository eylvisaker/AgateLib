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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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