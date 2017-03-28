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

using System;
using System.Collections;
using System.Collections.Generic;
using AgateLib.UserInterface.DataModel;

namespace AgateLib.Resources.DataModel
{
	public class FacetModelCollection : IDictionary<string, FacetModel>
	{
		Dictionary<string, FacetModel> facets = new Dictionary<string, FacetModel>();

		public FacetModel this[string key]
		{
			get
			{
				return ((IDictionary<string, FacetModel>)facets)[key];
			}

			set
			{
				((IDictionary<string, FacetModel>)facets)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, FacetModel>)facets).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, FacetModel>)facets).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, FacetModel>)facets).Keys;
			}
		}

		public ICollection<FacetModel> Values
		{
			get
			{
				return ((IDictionary<string, FacetModel>)facets).Values;
			}
		}
		
		public void Add(KeyValuePair<string, FacetModel> item)
		{
			((IDictionary<string, FacetModel>)facets).Add(item);
		}

		public void Add(string key, FacetModel value)
		{
			((IDictionary<string, FacetModel>)facets).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, FacetModel>)facets).Clear();
		}

		public bool Contains(KeyValuePair<string, FacetModel> item)
		{
			return ((IDictionary<string, FacetModel>)facets).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, FacetModel>)facets).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, FacetModel>[] array, int arrayIndex)
		{
			((IDictionary<string, FacetModel>)facets).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, FacetModel>> GetEnumerator()
		{
			return ((IDictionary<string, FacetModel>)facets).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, FacetModel> item)
		{
			return ((IDictionary<string, FacetModel>)facets).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, FacetModel>)facets).Remove(key);
		}

		public bool TryGetValue(string key, out FacetModel value)
		{
			return ((IDictionary<string, FacetModel>)facets).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, FacetModel>)facets).GetEnumerator();
		}

		internal void Validate()
		{
			foreach(var facet in this.Values)
			{
				facet.Validate();
			}
		}

		public void ApplyPath(string path)
		{
		}
	}
}