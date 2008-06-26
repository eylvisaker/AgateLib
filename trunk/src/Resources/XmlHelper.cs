using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ERY.AgateLib.Resources
{
    static class XmlHelper
    {
        internal static void AppendAttribute(XmlNode node, XmlDocument doc, 
            string name, string value)
        {
            XmlAttribute attrib = doc.CreateAttribute(name);
            attrib.Value = value;

            node.Attributes.Append(attrib);
        }
    }
}
