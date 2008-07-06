using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib.Resources
{
    /// <summary>
    /// The DisplayWindowResource represents a display window.
    /// XML Attributes:<br/> 
    ///   string name, Size size, Size minimum_size, Size maximum_size,
    ///   bool allow_resize, bool full_screen, int bpp.  Title text is stored in the body of the XML element.
    /// <para>A zero or missing value for any width/height means it doesn't apply.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Not all attributes are used at the moment.  minimum_size and maximum_size can be specified and
    /// will be preserved if the resource file is loaded and saved, but they are not used in the construction
    /// of the DisplayWindow.
    /// </remarks>
    public class DisplayWindowResource : AgateResource 
    {
        Size mSize;
        Size mMinimumSize;
        Size mMaximumSize;
        string mTitle;
        bool mAllowResize;
        bool mFullScreen;
        int mBpp;

        /// <summary>
        /// Gets or sets the preferred size of the DisplayWindow when it is created.
        /// For windowed systems, this will specify the size, but for full-screen systems this
        /// will specify the starting point for searching for a full-screen resolution.
        /// </summary>
        public Size Size { get { return mSize; } set { mSize = value; } }
        public Size MinimumSize { get { return mMinimumSize; } set { mMinimumSize = value; } }
        public Size MaximumSize { get { return mMaximumSize; } set { mMaximumSize = value; } }
        public string Title { get { return mTitle; } set { mTitle = value; } }
        public bool AllowResize { get { return mAllowResize; } set { mAllowResize = value; } }
        public bool FullScreen { get { return mFullScreen; } set { mFullScreen = value; } }
        public int Bpp { get { return mBpp; } set { mBpp = value; } }

        protected override AgateResource Clone()
        {
            DisplayWindowResource res = new DisplayWindowResource(Name);

            res.Size = Size;
            res.MinimumSize = MinimumSize;
            res.MaximumSize = MaximumSize;
            res.Title = Title;
            res.AllowResize = AllowResize;
            res.FullScreen = FullScreen;
            res.Bpp = Bpp;

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
                    Name = node.Attributes["name"].Value;

                    Size = XmlHelper.ReadAttributeSize(node, "preferred_size");
                    MinimumSize = XmlHelper.ReadAttributeSize(node, "minimum_size", Size.Empty);
                    MaximumSize = XmlHelper.ReadAttributeSize(node, "maximum_size", Size.Empty);
                    Bpp = XmlHelper.ReadAttributeInt(node, "bpp", 32);
                    AllowResize = XmlHelper.ReadAttributeBool(node, "allow_resize", false);
                    FullScreen = XmlHelper.ReadAttributeBool(node, "full_screen", false);

                    Title = node.InnerText.Trim();

                    break;

            }
        }

        internal override void BuildNodes(System.Xml.XmlElement parent, System.Xml.XmlDocument doc)
        {
            XmlElement el = doc.CreateElement("DisplayWindow");

            XmlHelper.AppendAttribute(el, doc, "name", Name);
            XmlHelper.AppendAttribute(el, doc, "preferred_size", Size.ToString());
            XmlHelper.AppendAttribute(el, doc, "minimum_size", MinimumSize.ToString());
            XmlHelper.AppendAttribute(el, doc, "maximum_size", MaximumSize.ToString());
            XmlHelper.AppendAttribute(el, doc, "allow_resize", AllowResize);
            XmlHelper.AppendAttribute(el, doc, "full_screen", FullScreen);
            XmlHelper.AppendAttribute(el, doc, "bpp", Bpp);

            el.InnerText = Title;

            parent.AppendChild(el);
        }

    }
}
