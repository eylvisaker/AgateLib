using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib.Resources
{
    static internal class XmlHelper
    {
        internal static void AppendAttribute(XmlNode node, XmlDocument doc, 
            string name, string value)
        {
            XmlAttribute attrib = doc.CreateAttribute(name);
            attrib.Value = value;

            node.Attributes.Append(attrib);
        }

        internal static void AppendAttribute(XmlNode node, XmlDocument doc, string name, int value)
        {
            AppendAttribute(node, doc, name, value.ToString());
        }
        internal static void AppendAttribute(XmlNode node, XmlDocument doc, string name, bool value)
        {
            AppendAttribute(node, doc, name, value.ToString());
        }

        internal static Size ReadAttributeSize(XmlNode node, string attributeName)
        {
            string text = node.Attributes[attributeName].Value;

            return SizeConverter.ConvertFromString(null, System.Globalization.CultureInfo.CurrentCulture, text);
        }
        internal static Size ReadAttributeSize(XmlNode node, string attributeName, Size defaultValue)
        {
            if (node.Attributes[attributeName] == null)
                return defaultValue;
            else
                return ReadAttributeSize(node, attributeName);
        }


        internal static int ReadAttributeInt(XmlNode node, string attributeName)
        {
            string text = node.Attributes[attributeName].Value;

            return int.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
        }
        internal static int ReadAttributeInt(XmlNode node, string attributeName, int defaultValue)
        {
            if (node.Attributes[attributeName] == null)
                return defaultValue;
            else
                return ReadAttributeInt(node, attributeName);
        }

        internal static bool ReadAttributeBool(XmlNode node, string attributeName)
        {
            string text = node.Attributes[attributeName].Value;

            return bool.Parse(text);
        }
        internal static bool ReadAttributeBool(XmlNode node, string attributeName, bool defaultValue)
        {
            if (node.Attributes[attributeName] == null)
                return defaultValue;
            else
                return ReadAttributeBool(node, attributeName);
        }
    }
}
