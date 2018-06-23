//
//    Copyright (c) 2006-2018 Erik Ylvisaker
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
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using AgateLib.Quality;

namespace AgateLib.Display.BitmapFont
{
	/// <summary>
	/// FontMetrics is a class which describes everything needed to render a font
	/// from a bitmap image.
	/// </summary>
	public sealed class FontMetrics : IDictionary<int, GlyphMetrics>
	{
		Dictionary<int, GlyphMetrics> mGlyphs = new Dictionary<int, GlyphMetrics>();

		/// <summary>
		/// Constructs an empty font metrics object.
		/// </summary>
		public FontMetrics()
		{ }


		/// <summary>
		/// Performs a deep copy.
		/// </summary>
		/// <returns></returns>
		public FontMetrics Clone()
		{
			FontMetrics result = new FontMetrics();

			foreach (KeyValuePair<int, GlyphMetrics> v in this.mGlyphs)
			{
				result.Add(v.Key, v.Value.Clone());
			}

			return result;
		}

		#region IDictionary<int,GlyphMetrics> Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Add(int key, GlyphMetrics value)
		{
			mGlyphs.Add(key, value);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainsKey(int key)
		{
			return mGlyphs.ContainsKey(key);
		}
		/// <summary>
		/// 
		/// </summary>
		public ICollection<int> Keys => mGlyphs.Keys;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Remove(int key)
		{
			return mGlyphs.Remove(key);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool TryGetValue(int key, out GlyphMetrics value)
		{
			return mGlyphs.TryGetValue(key, out value);
		}

		/// <summary>
		/// Returns a collection of the value.
		/// </summary>
		public ICollection<GlyphMetrics> Values => mGlyphs.Values;

		/// <summary>
		/// Returns a specified glyph metrics.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public GlyphMetrics this[int key]
		{
			get
			{
				var result = mGlyphs[key];

				return result;
			}
			set => mGlyphs[key] = value;
		}

		#endregion
		#region ICollection<KeyValuePair<int,GlyphMetrics>> Members

		void ICollection<KeyValuePair<int, GlyphMetrics>>.Add(KeyValuePair<int, GlyphMetrics> item)
		{
			((ICollection<KeyValuePair<int, GlyphMetrics>>)mGlyphs).Add(item);
		}
		/// <summary>
		/// Clears the list.
		/// </summary>
		public void Clear()
		{
			mGlyphs.Clear();
		}

		bool ICollection<KeyValuePair<int, GlyphMetrics>>.Contains(KeyValuePair<int, GlyphMetrics> item)
		{
			return ((ICollection<KeyValuePair<int, GlyphMetrics>>)mGlyphs).Contains(item);
		}

		void ICollection<KeyValuePair<int, GlyphMetrics>>.CopyTo(KeyValuePair<int, GlyphMetrics>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<int, GlyphMetrics>>)mGlyphs).CopyTo(array, arrayIndex);
		}
		/// <summary>
		/// Returns the number of glyphs.
		/// </summary>
		public int Count => mGlyphs.Count;

		bool ICollection<KeyValuePair<int, GlyphMetrics>>.IsReadOnly => false;

		bool ICollection<KeyValuePair<int, GlyphMetrics>>.Remove(KeyValuePair<int, GlyphMetrics> item)
		{
			return ((ICollection<KeyValuePair<int, GlyphMetrics>>)mGlyphs).Remove(item);
		}

		#endregion
		#region IEnumerable<KeyValuePair<int,GlyphMetrics>> Members

		/// <summary>
		/// Creates an enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<KeyValuePair<int, GlyphMetrics>> GetEnumerator()
		{
			return mGlyphs.GetEnumerator();
		}

		#endregion
		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}