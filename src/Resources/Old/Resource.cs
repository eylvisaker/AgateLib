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

using AgateLib.Geometry;

namespace AgateLib.Resources.Old
{
    /// <summary>
    /// Class similar to ClanLib's CL_Resource class.
    /// Needs lots of work.
    /// </summary>
    [Obsolete]
    public class Resource
    {
        private string mType;
        private string mName;
        private XmlNode mNode;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        internal Resource(string type, string name)
        {
            mType = type;
            mName = name;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        internal Resource(XmlNode node)
        {
            mNode = node;

            mType = mNode.Name;

            if (mNode["name"] != null)
                mName = mNode.Attributes["name"].Value;
            else
                mName = "";

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public bool HasAttribute(string attributeName)
        {
            for (int i = 0; i < mNode.Attributes.Count; i++)
                if (mNode.Attributes[i].Name == attributeName)
                    return true;

            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public string GetStringAttribute(string attributeName)
        {
            return mNode.Attributes[attributeName].Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public int GetIntAttribute(string attributeName)
        {
            string val = GetStringAttribute(attributeName);

            return int.Parse(val);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public double GetDoubleAttribute(string attributeName)
        {
            string val = GetStringAttribute(attributeName);

            return double.Parse(val);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public Point GetPointAttribute(string attributeName)
        {
            string val = GetStringAttribute(attributeName);
            string[] pts = val.Split(',');

            if (pts.Length != 2)
                throw new Exception("Error parsing Point attribute " + attributeName + " on type " + mType + ".");

            Point pt = new Point(
                int.Parse(pts[0]),
                int.Parse(pts[1]));

            return pt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public Size GetSizeAttribute(string attributeName)
        {
            string val = GetStringAttribute(attributeName);
            string[] pts = val.Split(',');

            if (pts.Length != 2)
                throw new Exception("Error parsing Size attribute " + attributeName + " on type " + mType + ".");

            Size sz = new Size(
                int.Parse(pts[0]),
                int.Parse(pts[1]));

            return sz;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<Resource> GetSubElements(string type)
        {
            List<Resource> retval = new List<Resource>();

            foreach (XmlNode node in mNode.ChildNodes)
            {
                if (node.Name == type)
                    retval.Add(new Resource(node));
            }

            return retval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Resource GetFirstSubElement(string type)
        {
            IList<Resource> temp = GetSubElements(type);

            if (temp.Count > 0)
                return GetSubElements(type)[0];
            else
                return null;
        }

    }
}
