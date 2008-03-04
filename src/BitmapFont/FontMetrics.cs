using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.BitmapFont
{
    /// <summary>
    /// FontMetrics is a class which describes everything needed to render a font
    /// from a bitmap image.
    /// </summary>
    public sealed class FontMetrics : IDictionary<char, GlyphMetrics>
    {
        Dictionary<char, GlyphMetrics> v = new Dictionary<char, GlyphMetrics>();

        #region IDictionary<char,GlyphMetrics> Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(char key, GlyphMetrics value)
        {
            v.Add(key, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(char key)
        {
            return v.ContainsKey(key);
        }
        /// <summary>
        /// 
        /// </summary>
        public ICollection<char> Keys
        {
            get { return v.Keys; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(char key)
        {
            return v.Remove(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(char key, out GlyphMetrics value)
        {
            return v.TryGetValue(key, out value);
        }

        /// <summary>
        /// Returns a collection of the value.
        /// </summary>
        public ICollection<GlyphMetrics> Values
        {
            get { return v.Values; }
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
                return v[key];
            }
            set
            {
                v[key] = value;
            }
        }

        #endregion

        #region ICollection<KeyValuePair<char,GlyphMetrics>> Members

        void ICollection<KeyValuePair<char,GlyphMetrics>>.Add(KeyValuePair<char, GlyphMetrics> item)
        {
            ((ICollection<KeyValuePair<char, GlyphMetrics>>)v).Add(item);
        }
        /// <summary>
        /// Clears the list.
        /// </summary>
        public void Clear()
        {
            v.Clear();
        }

        bool ICollection<KeyValuePair<char, GlyphMetrics>>.Contains(KeyValuePair<char, GlyphMetrics> item)
        {
            return ((ICollection<KeyValuePair<char, GlyphMetrics>>)v).Contains(item);
        }

        void ICollection<KeyValuePair<char, GlyphMetrics>>.CopyTo(KeyValuePair<char, GlyphMetrics>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<char, GlyphMetrics>>)v).CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Returns the number of glyphs.
        /// </summary>
        public int Count
        {
            get { return v.Count; }
        }

        bool ICollection<KeyValuePair<char, GlyphMetrics>>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<KeyValuePair<char,GlyphMetrics>>.Remove(KeyValuePair<char, GlyphMetrics> item)
        {
            return ((ICollection<KeyValuePair<char,GlyphMetrics>>)v).Remove(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<char,GlyphMetrics>> Members

        public IEnumerator<KeyValuePair<char, GlyphMetrics>> GetEnumerator()
        {
            return v.GetEnumerator();
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
