using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib.BitmapFont
{
    /// <summary>
    /// FontMetrics is a class which describes everything needed to render a font
    /// from a bitmap image.
    /// </summary>
    public sealed class FontMetrics : IDictionary<char, GlyphMetrics>, ICloneable 
    {
        Dictionary<char, GlyphMetrics> mGlyphs = new Dictionary<char, GlyphMetrics>();

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

        void ICollection<KeyValuePair<char,GlyphMetrics>>.Add(KeyValuePair<char, GlyphMetrics> item)
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

        bool ICollection<KeyValuePair<char,GlyphMetrics>>.Remove(KeyValuePair<char, GlyphMetrics> item)
        {
            return ((ICollection<KeyValuePair<char,GlyphMetrics>>)mGlyphs).Remove(item);
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



        #region ICloneable Members



        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        /// <summary>
        /// Loads the font metrics object from XML.
        /// </summary>
        /// <param name="xmlFile"></param>
        public void Load(string xmlFile)
        {
            mGlyphs.Clear();

            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFile);

            XmlNode rootNode = doc.ChildNodes[0];

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                GlyphMetrics glyph = new GlyphMetrics();

                char key = (char)int.Parse(node.Attributes["Char"].Value);
                glyph.SourceRect = Rectangle.Parse(node.Attributes["Source"].Value);

                glyph.LeftOverhang = GetAttributeInt32(node, "LeftOverhang");
                glyph.RightOverhang = GetAttributeInt32(node, "RightOverhang");

                mGlyphs.Add(key, glyph);
            }
        }

        private int GetAttributeInt32(XmlNode node, string p)
        {
            if (node[p] == null)
                return 0;

            return int.Parse(node[p].Value);
        }


        internal void Save(XmlNode node, XmlDocument doc)
        {
            XmlNode root = doc.CreateElement("Font");

            foreach (char glyph in mGlyphs.Keys)
            {
                XmlNode current = doc.CreateElement("Glyph");
                GlyphMetrics glyphMetrics = this[glyph];

                AddAttribute(doc, current, "Char", glyph);
                AddAttribute(doc, current, "Source", glyphMetrics.SourceRect);

                if (glyphMetrics.LeftOverhang != 0) AddAttribute(doc, current, "LeftOverhang", glyphMetrics.LeftOverhang);
                if (glyphMetrics.RightOverhang != 0) AddAttribute(doc, current, "RightOverhang", glyphMetrics.RightOverhang);

                root.AppendChild(current);
            }

            node.AppendChild(root); 
        }

        private static void AddAttribute(XmlDocument doc, XmlNode current, string name, int value)
        {
            XmlAttribute att = doc.CreateAttribute(name);
            att.Value = value.ToString();
            current.Attributes.Append(att);
        }
        private static void AddAttribute(XmlDocument doc, XmlNode current, string name, Rectangle value)
        {
            XmlAttribute att = doc.CreateAttribute(name);
            att.Value = value.ToString();
            current.Attributes.Append(att);
        }
        private static void AddAttribute(XmlDocument doc, XmlNode current, string name, char value)
        {
            XmlAttribute att = doc.CreateAttribute(name);
            att.Value = ((int)value).ToString();
            current.Attributes.Append(att);
        }
    }
}
