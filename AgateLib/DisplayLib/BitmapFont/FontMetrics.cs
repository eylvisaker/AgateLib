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
using System.Xml;
using AgateLib.Geometry;
using System.Runtime.Serialization;
using AgateLib.Quality;

namespace AgateLib.DisplayLib.BitmapFont
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
		public ICollection<int> Keys
		{
			get { return mGlyphs.Keys; }
		}
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
		public ICollection<GlyphMetrics> Values
		{
			get { return mGlyphs.Values; }
		}

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

				Condition.Requires(result != null);

				return result;
			}
			set
			{
				mGlyphs[key] = value;
			}
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
		public int Count
		{
			get { return mGlyphs.Count; }
		}

		bool ICollection<KeyValuePair<int, GlyphMetrics>>.IsReadOnly
		{
			get { return false; }
		}

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