using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib.Resources
{
    /// <summary>
    /// The DisplayWindowResource represents a display window.
    /// XML Attributes: name, preferred_size, minimum_size, maximum_size,
    /// allow_resize, full_screen.  Title text is stored in the body of the XML element.
    /// <para>A zero or missing value for any width/height means it doesn't apply.
    /// </para>
    /// 
    /// 
    /// </summary>
    public class DisplayWindowResource : AgateResource 
    {
        Size mPreferredSize;
        Size mMinimumSize;
        Size mMaximumSize;
        string mTitle;
        bool mAllowResize;
        bool mFullScreen;

        public Size PreferredSize { get { return mPreferredSize; } set { mPreferredSize = value; } }
        public Size MinimumSize { get { return mMinimumSize; } set { mMinimumSize = value; } }
        public Size MaximumSize { get { return mMaximumSize; } set { mMaximumSize = value; } }
        public string Title { get { return mTitle; } set { mTitle = value; } }
        public bool AllowResize { get { return mAllowResize; } set { mAllowResize = value; } }
        public bool FullScreen { get { return mFullScreen; } set { mFullScreen = value; } }

        protected override AgateResource Clone()
        {
            DisplayWindowResource res = new DisplayWindowResource(Name);

            res.PreferredSize = PreferredSize;
            res.MinimumSize = MinimumSize;
            res.MaximumSize = MaximumSize;
            res.Title = Title;
            res.AllowResize = AllowResize;
            res.FullScreen = FullScreen;

            return res;
        }

        public DisplayWindowResource(string name) : base (name)
        {
        }
        internal DisplayWindowResource(XmlNode node, string version) : base(string.Empty)
        {
            switch (version)
            {
                case "0.3.0":

                    break;

            }
        }

        internal override void BuildNodes(System.Xml.XmlElement parent, System.Xml.XmlDocument doc)
        {
            XmlElement el = doc.CreateElement("DisplayWindow");

            XmlHelper.AppendAttribute(el, doc, "name", Name);
            XmlHelper.AppendAttribute(el, doc, "preferred_size", PreferredSize.ToString());
            XmlHelper.AppendAttribute(el, doc, "minimum_size", MinimumSize.ToString());
            XmlHelper.AppendAttribute(el, doc, "maximum_size", MaximumSize.ToString());
            XmlHelper.AppendAttribute(el, doc, "allow_resize", AllowResize);
            XmlHelper.AppendAttribute(el, doc, "full_screen", FullScreen);

            el.Value = Title;

            parent.AppendChild(el);
        }

    }
}
