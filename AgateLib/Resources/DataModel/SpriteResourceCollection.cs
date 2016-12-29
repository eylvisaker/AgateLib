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
	public class SpriteResourceCollection : IDictionary<string, SpriteResource>
	{
		Dictionary<string, SpriteResource> spriteModels = new Dictionary<string, SpriteResource>();

		public SpriteResource this[string key]
		{
			get
			{
				return ((IDictionary<string, SpriteResource>)spriteModels)[key];
			}

			set
			{
				((IDictionary<string, SpriteResource>)spriteModels)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, SpriteResource>)spriteModels).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, SpriteResource>)spriteModels).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, SpriteResource>)spriteModels).Keys;
			}
		}

		public ICollection<SpriteResource> Values
		{
			get
			{
				return ((IDictionary<string, SpriteResource>)spriteModels).Values;
			}
		}

		public void Add(KeyValuePair<string, SpriteResource> item)
		{
			((IDictionary<string, SpriteResource>)spriteModels).Add(item);
		}

		public void Add(string key, SpriteResource value)
		{
			((IDictionary<string, SpriteResource>)spriteModels).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, SpriteResource>)spriteModels).Clear();
		}

		public bool Contains(KeyValuePair<string, SpriteResource> item)
		{
			return ((IDictionary<string, SpriteResource>)spriteModels).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, SpriteResource>)spriteModels).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, SpriteResource>[] array, int arrayIndex)
		{
			((IDictionary<string, SpriteResource>)spriteModels).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, SpriteResource>> GetEnumerator()
		{
			return ((IDictionary<string, SpriteResource>)spriteModels).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, SpriteResource> item)
		{
			return ((IDictionary<string, SpriteResource>)spriteModels).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, SpriteResource>)spriteModels).Remove(key);
		}

		public bool TryGetValue(string key, out SpriteResource value)
		{
			return ((IDictionary<string, SpriteResource>)spriteModels).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, SpriteResource>)spriteModels).GetEnumerator();
		}
	}
}