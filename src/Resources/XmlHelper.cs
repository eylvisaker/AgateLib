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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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
