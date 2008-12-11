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
using System.Xml.Serialization;

namespace AgateLib.Resources
{
    /// <summary>
    /// Container for a list of resources that all share the same language.
    /// </summary>
    public class ResourceGroup : ICollection<AgateResource>
    {
        private string mLanguage = "Default";
        List<AgateResource> mStore = new List<AgateResource>();

        internal ResourceGroup()
        {
            Add(new StringTable());
        }
        /// <summary>
        /// Constructs a ResourceGroup.
        /// </summary>
        /// <param name="language">The language of this resource group.</param>
        public ResourceGroup(string language) : this()
        {
            mLanguage = language;
        }

		/// <summary>
		/// Enumerates through the SpriteResources contained in this group of resources.
		/// </summary>
		public IEnumerable<SpriteResource> Sprites
		{
			get
			{
				for (int i = 0; i < mStore.Count; i++)
				{
					if (mStore[i] is SpriteResource)
						yield return (SpriteResource)mStore[i];
				}
			}
		}

        /// <summary>
        /// Gets the StringTable for this langauge.
        /// </summary>
        public StringTable Strings
        {
            get
            {
                for (int i = 0; i < Count; i++)
                {
                    if (mStore[i] is StringTable)
                        return (StringTable)mStore[i];
                }

                return null;
            }
        }
        /// <summary>
        /// Gets or sets the name of this language.
        /// </summary>
        public string LanguageName
        {
            get { return mLanguage; }
            set { mLanguage = value; }
        }
        /// <summary>
        /// Gets the resource of the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AgateResource this[string name]
        {
            get
            {
                for (int i = 0; i < mStore.Count; i++)
                    if (mStore[i].Name.Equals(name, StringComparison.InvariantCulture))
                        return mStore[i];

                throw new KeyNotFoundException("Resource not found.");
            }
        }
        /// <summary>
        /// Returns whether or not the passed resource exists in this group.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsResource(string name)
        {
            for (int i = 0; i < mStore.Count; i++)
                if (mStore[i].Name.Equals(name, StringComparison.InvariantCulture))
                    return true;

            return false;
        }

        internal void BuildNodes(XmlNode parent, XmlDocument doc)
        {
            XmlElement languageNode = doc.CreateElement("Language");
            XmlHelper.AppendAttribute(languageNode, doc, "name", LanguageName);

            parent.AppendChild(languageNode);

            mStore.Sort(sorter);

            for (int i = 0; i < Count; i++)
            {
                mStore[i].BuildNodes(languageNode, doc);
            }
        }
        
        int sorter(AgateResource a, AgateResource b)
        {
            return a.Name.CompareTo(b.Name);
        }

        #region --- ICollection<AgateResource> Members ---

        /// <summary>
        /// Adds a resource to this group.  An exception is thrown if an item with the same name 
        /// already exists in the group.
        /// </summary>
        /// <param name="item"></param>
        public void Add(AgateResource item)
        {
            if (item is StringTable)
            {
                if (Strings != null)
                    throw new ArgumentException("A string table already exists in this ResourceGroup.");
            }

            mStore.Add(item);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            mStore.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(AgateResource item)
        {
            return mStore.Contains(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(AgateResource[] array, int arrayIndex)
        {
            mStore.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return mStore.Count; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(AgateResource item)
        {
            return mStore.Remove(item);
        }

        #endregion
        #region --- IEnumerable<AgateResource> Members ---

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<AgateResource> GetEnumerator()
        {
            return mStore.GetEnumerator();
        }

        #endregion
        #region --- IEnumerable Members ---

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion


    }
    
}
