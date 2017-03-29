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

namespace AgateLib.Resources.Managers
{
	public class PropertyMap<T> : IDictionary<string, PropertyMapValue<T>>
	{
		Dictionary<string, PropertyMapValue<T>> map = new Dictionary<string, PropertyMapValue<T>>();

		public PropertyMapValue<T> this[string key]
		{
			get
			{
				return ((IDictionary<string, PropertyMapValue<T>>)map)[key];
			}

			set
			{
				((IDictionary<string, PropertyMapValue<T>>)map)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, PropertyMapValue<T>>)map).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, PropertyMapValue<T>>)map).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, PropertyMapValue<T>>)map).Keys;
			}
		}

		public ICollection<PropertyMapValue<T>> Values
		{
			get
			{
				return ((IDictionary<string, PropertyMapValue<T>>)map).Values;
			}
		}

		public void Add(KeyValuePair<string, PropertyMapValue<T>> item)
		{
			((IDictionary<string, PropertyMapValue<T>>)map).Add(item);
		}

		public void Add(string key, PropertyMapValue<T> value)
		{
			((IDictionary<string, PropertyMapValue<T>>)map).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, PropertyMapValue<T>>)map).Clear();
		}

		public bool Contains(KeyValuePair<string, PropertyMapValue<T>> item)
		{
			return ((IDictionary<string, PropertyMapValue<T>>)map).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, PropertyMapValue<T>>)map).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, PropertyMapValue<T>>[] array, int arrayIndex)
		{
			((IDictionary<string, PropertyMapValue<T>>)map).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, PropertyMapValue<T>>> GetEnumerator()
		{
			return ((IDictionary<string, PropertyMapValue<T>>)map).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, PropertyMapValue<T>> item)
		{
			return ((IDictionary<string, PropertyMapValue<T>>)map).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, PropertyMapValue<T>>)map).Remove(key);
		}

		public bool TryGetValue(string key, out PropertyMapValue<T> value)
		{
			return ((IDictionary<string, PropertyMapValue<T>>)map).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, PropertyMapValue<T>>)map).GetEnumerator();
		}
	}
}
