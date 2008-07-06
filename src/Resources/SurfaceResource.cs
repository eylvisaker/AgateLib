using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib.Resources
{
    /// <summary>
    /// Resource which loads a surface. <br/>
    /// XML Attributes:<br/> 
    ///   string name, string filename
    /// <para>A zero or missing value for any width/height means it doesn't apply.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Not all attributes are used at the moment.  minimum_size and maximum_size can be specified and
    /// will be preserved if the resource file is loaded and saved, but they are not used in the construction
    /// of the DisplayWindow.
    /// </remarks>
    public class SurfaceResource : AgateResource 
    {
        string mFilename;

        public string Filename { get { return mFilename; } set { mFilename = value; } }

        public SurfaceResource(string name)
            : base(name)
        {
        }

        internal SurfaceResource(XmlNode node, string version)
            : base(string.Empty)
        {
            switch (version)
            {
                case "0.3.0":
                    Name = node.Attributes["name"].Value;
                    Filename = node.Attributes["filename"].Value;

                    break;
            }
        }
        internal override void BuildNodes(System.Xml.XmlElement parent, System.Xml.XmlDocument doc)
        {
            XmlElement el = doc.CreateElement("Surface");

            XmlHelper.AppendAttribute(el, doc, "name", Name);
            XmlHelper.AppendAttribute(el, doc, "filename", Filename);

            parent.AppendChild(el);
        }

        protected override AgateResource Clone()
        {
            throw new NotImplementedException();
        }
    }
}
