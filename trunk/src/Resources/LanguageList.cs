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

namespace ERY.AgateLib.Resources
{
    /// <summary>
    /// A class which contains a list of ResourceGroups each with resources in a specific language.
    /// </summary>
    public class LanguageList : ICollection<ResourceGroup>
    {
        List<ResourceGroup> mLanguages = new List<ResourceGroup>();

        void ICollection<ResourceGroup>.Add(ResourceGroup grp)
        {
            foreach (ResourceGroup lang in mLanguages)
            {
                if (lang.LanguageName.Equals(grp.LanguageName, StringComparison.InvariantCultureIgnoreCase))
                    throw new ArgumentException("Language already exists!");
            }
            
            mLanguages.Add(grp);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void Add(string name)
        {
            foreach (ResourceGroup lang in mLanguages)
            {
                if (lang.LanguageName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    throw new ArgumentException("Language already exists!");
            }

            mLanguages.Add(new ResourceGroup(name));

            OnLanguageAdded(name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResourceGroup this[string name]
        {
            get
            {
                for (int i = 0; i < mLanguages.Count; i++)
                {
                    if (mLanguages[i].LanguageName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                        return mLanguages[i];
                }

                throw new IndexOutOfRangeException(
                    "Language " + name + " not found.");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ResourceGroup this[int index]
        {
            get { return mLanguages[index]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return mLanguages.Count; }
        }

        private void OnLanguageAdded(string languageName)
        {
            if (LanguageAdded != null)
                LanguageAdded(this, new LanguageListChangedEventArgs(languageName));
        }
        private void OnLanguageRemoved(string languageName)
        {
            if (LanguageRemoved != null)
                LanguageRemoved(this, new LanguageListChangedEventArgs(languageName));
        }
        /// <summary>
        /// Event which is raised when a language is added to the list.
        /// </summary>
        public event EventHandler<LanguageListChangedEventArgs> LanguageAdded;
        /// <summary>
        /// Event which is raised when a language is removed from the list.
        /// </summary>
        public event EventHandler<LanguageListChangedEventArgs> LanguageRemoved;

        #region --- ICollection<ResourceGroup> Members ---

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            mLanguages.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(ResourceGroup item)
        {
            return mLanguages.Contains(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageName"></param>
        /// <returns></returns>
        public bool Contains(string languageName)
        {
            for (int i = 0; i < mLanguages.Count; i++)
            {
                if (mLanguages[i].LanguageName.Equals(languageName,
                                   StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(ResourceGroup[] array, int arrayIndex)
        {
            mLanguages.CopyTo(array, arrayIndex);
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
        public bool Remove(ResourceGroup item)
        {
            bool retval = mLanguages.Remove(item);

            if (retval)
                OnLanguageRemoved(item.LanguageName);

            return retval;
        }

        #endregion
        #region --- IEnumerable<ResourceGroup> Members ---

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ResourceGroup> GetEnumerator()
        {
            return mLanguages.GetEnumerator();
        }

        #endregion
        #region --- IEnumerable Members ---

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mLanguages.GetEnumerator();
        }

        #endregion
    }

    
}
