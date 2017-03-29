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