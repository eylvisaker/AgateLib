using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ERY.AgateLib
{

    /// <summary>
    /// XML ResourceManager.
    /// 
    /// Implementation is currently very basic.
    /// </summary>
    public class ResourceManager
    {
        XmlDataDocument mDoc;
        XmlNode mRootNode;

        /// <summary>
        /// Constructs a ResourceManager from an XML File.
        /// </summary>
        /// <param name="xmlFile"></param>
        public ResourceManager(string xmlFile)
        {
            mDoc = new XmlDataDocument();
            mDoc.Load(xmlFile);

            OnLoad();
        }
        /// <summary>
        /// Constructs a ResourceManager from a stream containing XML data.
        /// </summary>
        /// <param name="stream"></param>
        public ResourceManager(System.IO.Stream stream)
        {
            mDoc = new XmlDataDocument();
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
