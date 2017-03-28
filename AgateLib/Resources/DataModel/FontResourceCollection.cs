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
using AgateLib.DisplayLib.BitmapFont;

namespace AgateLib.Resources.DataModel
{
	public class FontResourceCollection : IDictionary<string, FontResource>
	{
		Dictionary<string, FontResource> fontModels = new Dictionary<string, FontResource>();

		public FontResource this[string key]
		{
			get
			{
				return ((IDictionary<string, FontResource>)fontModels)[key];
			}

			set
			{
				((IDictionary<string, FontResource>)fontModels)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, FontResource>)fontModels).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, FontResource>)fontModels).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, FontResource>)fontModels).Keys;
			}
		}

		public ICollection<FontResource> Values
		{
			get
			{
				return ((IDictionary<string, FontResource>)fontModels).Values;
			}
		}

		public void Add(KeyValuePair<string, FontResource> item)
		{
			((IDictionary<string, FontResource>)fontModels).Add(item);
		}

		public void Add(string key, FontResource value)
		{
			((IDictionary<string, FontResource>)fontModels).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, FontResource>)fontModels).Clear();
		}

		public bool Contains(KeyValuePair<string, FontResource> item)
		{
			return ((IDictionary<string, FontResource>)fontModels).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, FontResource>)fontModels).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, FontResource>[] array, int arrayIndex)
		{
			((IDictionary<string, FontResource>)fontModels).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, FontResource>> GetEnumerator()
		{
			return ((IDictionary<string, FontResource>)fontModels).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, FontResource> item)
		{
			return ((IDictionary<string, FontResource>)fontModels).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, FontResource>)fontModels).Remove(key);
		}

		public bool TryGetValue(string key, out FontResource value)
		{
			return ((IDictionary<string, FontResource>)fontModels).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, FontResource>)fontModels).GetEnumerator();
		}

		internal void ApplyPath(string path)
		{
			foreach (var font in this)
				font.Value.ApplyPath(path);
		}
	}
}