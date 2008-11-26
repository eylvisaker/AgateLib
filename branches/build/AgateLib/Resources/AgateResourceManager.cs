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
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.IO;

using AgateLib.DisplayLib;
using AgateLib.Utility;

namespace AgateLib.Resources
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

        /// <summary>
        /// Gets or sets the filename for the XML resource file.
        /// </summary>
        public string Filename
        {
            get { return mFilename; }
            set { mFilename = value; }
        }

        /// <summary>
        /// Gets the current language resources are read from.
        /// </summary>
        public ResourceGroup CurrentLanguage
        {
            get
            {
                if (mCurrentLanguage == null)
                    mCurrentLanguage = DefaultLanguage;
                return mCurrentLanguage;
            }
        }
        /// <summary>
        /// Returns the collection of resources which belong to the default language.
        /// </summary>
        public ResourceGroup DefaultLanguage
        {
            get { return Languages["Default"]; }
        }

        /// <summary>
        /// Sets which language should be used to read resources.
        /// </summary>
        /// <param name="language"></param>
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
        /// <summary>
        /// Gets a list of the languages in this resource file.
        /// </summary>
        public LanguageList Languages
        {
            get { return mLanguages; }
        }

        /// <summary>
        /// Saves the resources to a file located in the Filename property.
        /// </summary>
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

        /// <summary>
        /// Static method which creates a resource manager based on the contents of a file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static AgateResourceManager LoadFromFile(string filename)
        {
            AgateResourceManager retval = new AgateResourceManager();

            retval.Load(filename);
            retval.Filename = filename;

            return retval;
        }
        /// <summary>
        /// Loads the resource information from a file.
        /// This erases all information in the current AgateResourceManager.
        /// </summary>
        /// <param name="filename"></param>
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
            try
            {
                for (int i = 0; i < parentNode.ChildNodes.Count; i++)
                {
                    XmlNode node = parentNode.ChildNodes[i];

                    ReadNode(group, node, version);
                }
            }
            catch (Exception e)
            {
                throw new AgateResourceException("An error occurred while reading XML resource file: \r\n"
                    + e.Message, e);
            }
        }
        private void ReadNode(ResourceGroup group, XmlNode node, string version)
        {
            switch (node.Name)
            {
                case "StringTable":
                    StringTable strings = new StringTable(node, version);
                    group.Strings.Combine(strings);
                    return;

                case "DisplayWindow":
                    DisplayWindowResource dispWind = new DisplayWindowResource(node, version);
                    group.Add(dispWind);
                    return;

                case "Surface":
                    SurfaceResource surf = new SurfaceResource(node, version);
                    group.Add(surf);
                    return;

                case "Sprite":
                    SpriteResource sprite = new SpriteResource(node, version);
                    group.Add(sprite);
                    return;

                default:
                    ReadError(node.Name + " unrecognized.");
                    return;
            }
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

        /// <summary>
        /// Returns a resource in the current language based on its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns true if the specified resource is present in either the
        /// current language or the default language.  No other languages are checked.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsResource(string name)
        {
            if (CurrentLanguage.ContainsResource(name))
                return true;

            return DefaultLanguage.ContainsResource(name);
        }

        /// <summary>
        /// Creates a sprite based on a resource.
        /// </summary>
        /// <param name="name">Name of the sprite resource to construct.</param>
        /// <returns></returns>
        public ISprite CreateSprite(string name)
        {
            if (ContainsResource(name) == false)
                throw new AgateResourceException("Resource " + name + " not found.");

            if (this[name].GetType() != typeof(SpriteResource))
                throw new AgateResourceException("Resource " + name + " is not of type SpriteResource.");

            SpriteResource res = (SpriteResource)this[name];

            return res.CreateSprite();

        }
    }
}
