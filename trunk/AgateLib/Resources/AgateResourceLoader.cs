using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using AgateLib.Utility;
using System.Diagnostics;

namespace AgateLib.Resources
{
    public static class AgateResourceLoader
    {

        /// <summary>
        /// Saves the resources to a file located in the Filename property.
        /// </summary>
        public static void SaveResources(AgateResourceCollection resources, string filename)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("AgateResources");
            XmlHelper.AppendAttribute(root, doc, "Version", "0.3.0");

            doc.AppendChild(root);

            foreach(AgateResource res in resources)
            {
                res.BuildNodes(root, doc);
            }

            doc.Save(filename);

        }

        /// <summary>
        /// Static method which creates a resource manager based on the contents of a file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static AgateResourceCollection LoadResources(string filename)
        {
            using (System.IO.Stream s = AgateFileProvider.ResourceProvider.OpenRead(filename))
            {
                return LoadResources(s);
            }
            
        }
        /// <summary>
        /// Loads the resource information from a stream containing XML data.
        /// This erases all information in the current AgateResourceCollection.
        /// </summary>
        /// <param name="stream"></param>
        public static AgateResourceCollection LoadResources(Stream stream)
        {
            AgateResourceCollection retval = new AgateResourceCollection();

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(stream);
            }
            catch (XmlException e)
            {
                throw new AgateResourceException("The XML resource file is malformed.", e);
            }

            XmlNode root = doc.ChildNodes[0];

            if (root.Attributes["Version"] == null)
                throw new AgateResourceException("XML resource file does not contain the required version attibute.");

            string version = root.Attributes["Version"].Value;

            switch (version)
            {
                case "0.3.0":
                    ReadVersion030(retval, root, version);
                    break;

                default:
                    throw new AgateResourceException("XML Resource file version " + version + " not supported.");
            }

            return retval;
        }

        private static void ReadVersion030(AgateResourceCollection resources, XmlNode root, string version)
        {
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                XmlNode node = root.ChildNodes[i];

                if (node is XmlComment) continue;

                try
                {
                    resources.Add(ReadNode(node, version));
                }
                catch (Exception e)
                {
                    throw new AgateResourceException("An error occurred while reading XML resource file: \r\n"
                        + e.Message, e);
                }
            }
        }

        private static AgateResource ReadNode(XmlNode node, string version)
        {
            switch (node.Name)
            {
                case "StringTable":
                    return new StringTable(node, version);

                case "DisplayWindow":
                    return new DisplayWindowResource(node, version);

                case "Surface":
                    return new SurfaceResource(node, version);

                case "Sprite":
                    return new SpriteResource(node, version);

                default:
                    ReadError("Unrecognized node " + node.Name + ".");
                    return null;
            }
        }
        private static void ReadError(string p)
        {
            if (ThrowOnReadError)
            {
                throw new InvalidDataException(p);
            }
            else
                Debug.Print(p);
        }

        static bool mThrowOnReadError = true;

        /// <summary>
        /// Gets or sets a bool value indicating whether or not an exception
        /// is thrown if an unknown resource type is encountered when reading
        /// an XML file.
        /// </summary>
        public static bool ThrowOnReadError
        {
            get { return mThrowOnReadError; }
            set { mThrowOnReadError = value; }
        }

    }
}
