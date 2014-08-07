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

namespace AgateLib.DisplayLib.BitmapFont
{
	/// <summary>
	/// FontMetrics is a class which describes everything needed to render a font
	/// from a bitmap image.
	/// </summary>
	public sealed class FontMetrics : IDictionary<char, GlyphMetrics>
	{
		Dictionary<char, GlyphMetrics> mGlyphs = new Dictionary<char, GlyphMetrics>();

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
			FontMetrics retval = new FontMetrics();

			foreach (KeyValuePair<char, GlyphMetrics> v in this.mGlyphs)
			{
				retval.Add(v.Key, v.Value.Clone());
			}

			return retval;
		}

		#region IDictionary<char,GlyphMetrics> Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Add(char key, GlyphMetrics value)
		{
			mGlyphs.Add(key, value);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainsKey(char key)
		{
			return mGlyphs.ContainsKey(key);
		}
		/// <summary>
		/// 
		/// </summary>
		public ICollection<char> Keys
		{
			get { return mGlyphs.Keys; }
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Remove(char key)
		{
			return mGlyphs.Remove(key);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool TryGetValue(char key, out GlyphMetrics value)
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
		public GlyphMetrics this[char key]
		{
			get
			{
				return mGlyphs[key];
			}
			set
			{
				mGlyphs[key] = value;
			}
		}

		#endregion
		#region ICollection<KeyValuePair<char,GlyphMetrics>> Members

		void ICollection<KeyValuePair<char, GlyphMetrics>>.Add(KeyValuePair<char, GlyphMetrics> item)
		{
			((ICollection<KeyValuePair<char, GlyphMetrics>>)mGlyphs).Add(item);
		}
		/// <summary>
		/// Clears the list.
		/// </summary>
		public void Clear()
		{
			mGlyphs.Clear();
		}

		bool ICollection<KeyValuePair<char, GlyphMetrics>>.Contains(KeyValuePair<char, GlyphMetrics> item)
		{
			return ((ICollection<KeyValuePair<char, GlyphMetrics>>)mGlyphs).Contains(item);
		}

		void ICollection<KeyValuePair<char, GlyphMetrics>>.CopyTo(KeyValuePair<char, GlyphMetrics>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<char, GlyphMetrics>>)mGlyphs).CopyTo(array, arrayIndex);
		}
		/// <summary>
		/// Returns the number of glyphs.
		/// </summary>
		public int Count
		{
			get { return mGlyphs.Count; }
		}

		bool ICollection<KeyValuePair<char, GlyphMetrics>>.IsReadOnly
		{
			get { return false; }
		}

		bool ICollection<KeyValuePair<char, GlyphMetrics>>.Remove(KeyValuePair<char, GlyphMetrics> item)
		{
			return ((ICollection<KeyValuePair<char, GlyphMetrics>>)mGlyphs).Remove(item);
		}

		#endregion
		#region IEnumerable<KeyValuePair<char,GlyphMetrics>> Members

		/// <summary>
		/// Creates an enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<KeyValuePair<char, GlyphMetrics>> GetEnumerator()
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