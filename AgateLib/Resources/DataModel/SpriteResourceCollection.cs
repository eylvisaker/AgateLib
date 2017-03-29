//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
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