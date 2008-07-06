using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.IO;

namespace ERY.AgateLib.Resources
{
    /// <summary>
    /// Class which wraps an XML based resource file.  This class provides methods for adding
    /// and extracting resources.
    /// </summary>
    public class AgateResourceManager
    {
        #region --- Static Members ---

        bool mThrowOnReadError = true;

        /// <summary>
        /// Gets or sets a bool value indicating whether or not an exception
        /// is thrown if an unknown resource type is encountered when reading
        /// an XML file.
        /// </summary>
        public bool ThrowOnReadError
        {
            get { return mThrowOnReadError; }
            set { mThrowOnReadError = value; }
        }



        #endregion

        ResourceGroup mCurrentLanguage;
        LanguageList mLanguages = new LanguageList();
        private string mFilename;

        /// <summary>
        /// Constructs a new AgateResources object.
        /// </summary>
        public AgateResourceManager()
        {
            mLanguages.Add("Default");
        }

        public string Filename
        {
            get { return mFilename; }
            set { mFilename = value; }
        }

        public ResourceGroup CurrentLanguage
        {
            get
            {
                if (mCurrentLanguage == null)
                    mCurrentLanguage = DefaultLanguage;
                return mCurrentLanguage;
            }
        }
        public ResourceGroup DefaultLanguage
        {
            get { return Languages["Default"]; }
        }

        public void SetCurrentLanguage(string language)
        {
            for (int i = 0; i < mLanguages.Count; i++)
            {
                if (mLanguages[i].LanguageName.Equals(language, StringComparison.InvariantCultureIgnoreCase))
                {
                    mCurrentLanguage = mLanguages[i];
                    return;
                }
            }

            throw new ArgumentException("Could not find the specified language.");
        }
        public LanguageList Languages
        {
            get { return mLanguages; }
        }

        public void Save()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.CreateElement("AgateResources");
            XmlHelper.AppendAttribute(root, doc, "Version", "0.3.0");

            doc.AppendChild(root);

            for (int i = 0; i < mLanguages.Count; i++)
            {
                mLanguages[i].BuildNodes(root, doc);
            }

            doc.Save(Filename);

        }

        public static AgateResourceManager LoadFromFile(string filename)
        {
            AgateResourceManager retval = new AgateResourceManager();

            retval.Load(filename);
            retval.Filename = filename;

            return retval;
        }
        public void Load(string filename)
        {
            Load(FileManager.OpenFile(FileManager.ResourcePath, filename, FileMode.Open, FileAccess.Read));
        }
        /// <summary>
        /// Loads the resource information from a stream containing XML data.
        /// This erases all information in the current AgateResourceManager.
        /// </summary>
        /// <param name="stream"></param>
        public void Load(Stream stream)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            XmlNode root = doc.ChildNodes[0];

            if (root.Attributes["Version"] == null)
                throw new InvalidDataException("XML Resource file does not contain version attibute.");

            string version = root.Attributes["Version"].Value;

            mLanguages.Clear();

            switch (version)
            {
                case "0.3.0":
                    for (int i = 0; i < root.ChildNodes.Count; i++)
                    {
                        XmlNode node = root.ChildNodes[i];

                        if (node.Name == "Language")
                        {
                            string lang = node.Attributes["name"].Value;
                            mLanguages.Add(lang);

                            ResourceGroup group = mLanguages[lang];

                            // load the group
                            ReadNodes(group, node, version);
                        }
                    }
                    break;

                default:
                    throw new InvalidDataException("XML Resource file version " + version + " not supported.");
            }
        }

        private void ReadNodes(ResourceGroup group, XmlNode parentNode, string version)
        {
            for (int i = 0; i < parentNode.ChildNodes.Count; i++)
            {
                XmlNode node = parentNode.ChildNodes[i];

                ReadNode(group, node, version);
            }
        }

        private void ReadNode(ResourceGroup group, XmlNode node, string version)
        {
            AgateResource resource = null;

            switch (node.Name)
            {
                case "StringTable":
                    StringTable strings = new StringTable(node, version);
                    group.Strings.Combine(strings);
                    return;

                case "DisplayWindow":
                    DisplayWindowResource res = new DisplayWindowResource(node, version);
                    group.Add(res);
                    return;

                default:
                    ReadError(node.Name + " unrecognized.");
                    return;
            }

            group.Add(resource);
        }

        private void ReadError(string p)
        {
            if (ThrowOnReadError)
            {
                throw new InvalidDataException(p);
            }
            else
                Debug.Print(p);
        }

        public AgateResource this[string name]
        {
            get
            {
                if (CurrentLanguage.ContainsResource(name))
                    return CurrentLanguage[name];
                else
                    return DefaultLanguage[name];
            }
        }
    }
}
