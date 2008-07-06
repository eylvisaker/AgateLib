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

namespace ERY.AgateLib.Resources
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
        public ResourceGroup(string language) : this()
        {
           
            mLanguage = language;
        }

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

        public string LanguageName
        {
            get { return mLanguage; }
            set { mLanguage = value; }
        }

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

        public void Add(AgateResource item)
        {
            if (item is StringTable)
            {
                if (Strings != null)
                    throw new ArgumentException("A string table already exists in this ResourceGroup.");
            }

            mStore.Add(item);
        }

        public void Clear()
        {
            mStore.Clear();
        }

        public bool Contains(AgateResource item)
        {
            return mStore.Contains(item);
        }

        public void CopyTo(AgateResource[] array, int arrayIndex)
        {
            mStore.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return mStore.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(AgateResource item)
        {
            return mStore.Remove(item);
        }

        #endregion
        #region --- IEnumerable<AgateResource> Members ---

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
