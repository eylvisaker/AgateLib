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

namespace ERY.AgateLib.Resources
{

    /// <summary>
    /// XML ResourceManager.
    /// 
    /// Implementation is currently very basic.
    /// </summary>
    public class ResourceManager
    {
        XmlDocument mDoc;
        XmlNode mRootNode;

        /// <summary>
        /// Constructs a ResourceManager from an XML File.
        /// </summary>
        /// <param name="xmlFile"></param>
        public ResourceManager(string xmlFile)
        {
            mDoc = new XmlDocument();
            mDoc.Load(xmlFile);

            OnLoad();
        }
        /// <summary>
        /// Constructs a ResourceManager from a stream containing XML data.
        /// </summary>
        /// <param name="stream"></param>
        public ResourceManager(System.IO.Stream stream)
        {
            mDoc = new XmlDocument();
            mDoc.Load(stream);

            OnLoad();
        }



        private void OnLoad()
        {
            foreach (XmlNode node in mDoc.ChildNodes)
            {
                if (node.Name == "resources")
                {
                    mRootNode = node;
                    break;
                }
            }
        }

        internal Resource GetResource(string type, string name)
        {
            foreach (XmlNode node in mRootNode.ChildNodes)
            {
                if (node.Name == type && node.Attributes["name"].Value  == name)
                {
                    return new Resource(node);
                }
            }

            throw new Exception("Resource type " + type + " named \"" + name + "\" not found.");
        }
    }

}
