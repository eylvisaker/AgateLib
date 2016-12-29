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

using System.Collections;
using System.Collections.Generic;

namespace AgateLib.Resources.DataModel
{
	public class SurfaceModelCollection : IDictionary<string, SurfaceModel>
	{
		Dictionary<string, SurfaceModel> surfaces = new Dictionary<string, SurfaceModel>();

		public SurfaceModel this[string key]
		{
			get
			{
				return ((IDictionary<string, SurfaceModel>)surfaces)[key];
			}

			set
			{
				((IDictionary<string, SurfaceModel>)surfaces)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, SurfaceModel>)surfaces).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, SurfaceModel>)surfaces).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, SurfaceModel>)surfaces).Keys;
			}
		}

		public ICollection<SurfaceModel> Values
		{
			get
			{
				return ((IDictionary<string, SurfaceModel>)surfaces).Values;
			}
		}

		public void Add(KeyValuePair<string, SurfaceModel> item)
		{
			((IDictionary<string, SurfaceModel>)surfaces).Add(item);
		}

		public void Add(string key, SurfaceModel value)
		{
			((IDictionary<string, SurfaceModel>)surfaces).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, SurfaceModel>)surfaces).Clear();
		}

		public bool Contains(KeyValuePair<string, SurfaceModel> item)
		{
			return ((IDictionary<string, SurfaceModel>)surfaces).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, SurfaceModel>)surfaces).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, SurfaceModel>[] array, int arrayIndex)
		{
			((IDictionary<string, SurfaceModel>)surfaces).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, SurfaceModel>> GetEnumerator()
		{
			return ((IDictionary<string, SurfaceModel>)surfaces).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, SurfaceModel> item)
		{
			return ((IDictionary<string, SurfaceModel>)surfaces).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, SurfaceModel>)surfaces).Remove(key);
		}

		public bool TryGetValue(string key, out SurfaceModel value)
		{
			return ((IDictionary<string, SurfaceModel>)surfaces).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, SurfaceModel>)surfaces).GetEnumerator();
		}
	}
}