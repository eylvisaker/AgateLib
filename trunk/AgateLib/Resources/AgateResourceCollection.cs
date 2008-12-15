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
    public class AgateResourceCollection : IDictionary<string, AgateResource>, ICollection<AgateResource>
    {
        Dictionary<string, AgateResource> mStore = new Dictionary<string, AgateResource>();
        StringTable strings;

        public AgateResourceCollection()
        {
        }

		/// <summary>
		/// Enumerates through the SpriteResources contained in this group of resources.
		/// </summary>
		public IEnumerable<SpriteResource> Sprites
		{
			get
			{
                foreach (KeyValuePair<string, AgateResource> kvp in mStore)
                {
                    if (kvp.Value is SpriteResource)
                        yield return kvp.Value as SpriteResource;
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
                foreach (KeyValuePair<string, AgateResource> kvp in mStore)
                {
                    if (kvp.Value is StringTable)
                        return (StringTable)kvp.Value;
                }

                return null;
            }
        }

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

            mStore.Add(item.Name, item);
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
        public bool Contains(string resourceName)
        {
            return mStore.ContainsKey(resourceName);
        }
 
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return mStore.Count; }
        }
               
        #region --- IDictionary<string,AgateResource> Members ---

        void IDictionary<string,AgateResource>.Add(string key, AgateResource value)
        {
            value.Name = key;

        }

        
        public ICollection<string> Keys
        {
            get { return mStore.Keys; }
        }


        bool IDictionary<string,AgateResource>.ContainsKey(string key)
        {
            return mStore.ContainsKey(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(string resourceName)
        {
            return mStore.Remove(resourceName);
        }
        public bool TryGetValue(string key, out AgateResource value)
        {
            return mStore.TryGetValue(key, out value);
        }

        public ICollection<AgateResource> Values
        {
            get { return mStore.Values; }
        }
        public AgateResource this[string key]
        {
            get
            {
                return mStore[key];
            }
            set
            {
                mStore[key] = value;
            }
        }

        #endregion
        #region --- ICollection<KeyValuePair<string,AgateResource>> Members ---

        void ICollection<KeyValuePair<string,AgateResource>>.Add(KeyValuePair<string, AgateResource> item)
        {
            mStore.Add(item.Key, item.Value);
        }
        bool ICollection<KeyValuePair<string,AgateResource>>.Contains(KeyValuePair<string, AgateResource> item)
        {
            return (mStore as ICollection<KeyValuePair<string, AgateResource>>).Contains(item);   
        }
        void ICollection<KeyValuePair<string, AgateResource>>.CopyTo(KeyValuePair<string, AgateResource>[] array, int arrayIndex)
        {
            (mStore as ICollection<KeyValuePair<string, AgateResource>>).CopyTo(array, arrayIndex);
        }
        bool ICollection<KeyValuePair<string, AgateResource>>.Remove(KeyValuePair<string, AgateResource> item)
        {
            return mStore.Remove(item.Key);
        }
        bool ICollection<KeyValuePair<string, AgateResource>>.IsReadOnly
        {
            get { return false; }
        }

        #endregion
        #region --- IEnumerable<KeyValuePair<string,AgateResource>> Members ---

        IEnumerator<KeyValuePair<string, AgateResource>> IEnumerable<KeyValuePair<string, AgateResource>>.GetEnumerator()
        {
            return mStore.GetEnumerator();
        }

        #endregion

        #region --- IEnumerable<AgateResource> Members ---

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<AgateResource> GetEnumerator()
        {
            return mStore.Values.GetEnumerator();
        }

        #endregion
        #region --- IEnumerable Members ---

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region --- ICollection<AgateResource> Members ---

        public bool Contains(AgateResource item)
        {
            return mStore.ContainsValue(item);
        }

        void ICollection<AgateResource>.CopyTo(AgateResource[] array, int arrayIndex)
        {
            foreach (AgateResource res in mStore.Values)
            {
                array[arrayIndex] = res;
                arrayIndex++;
            }
        }
        bool ICollection<AgateResource>.IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(AgateResource item)
        {
            return mStore.Remove(item.Name);
        }

        #endregion
    }
}
