using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Resources
{
    /// <summary>
    /// A class which contains a list of resources specific to a particular language.
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
        public ResourceGroup this[int index]
        {
            get { return mLanguages[index]; }
        }
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
        public event EventHandler<LanguageListChangedEventArgs> LanguageAdded;
        public event EventHandler<LanguageListChangedEventArgs> LanguageRemoved;

        #region --- ICollection<ResourceGroup> Members ---

        public void Clear()
        {
            mLanguages.Clear();
        }

        public bool Contains(ResourceGroup item)
        {
            return mLanguages.Contains(item);
        }
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

        public void CopyTo(ResourceGroup[] array, int arrayIndex)
        {
            mLanguages.CopyTo(array, arrayIndex);
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(ResourceGroup item)
        {
            bool retval = mLanguages.Remove(item);

            if (retval)
                OnLanguageRemoved(item.LanguageName);

            return retval;
        }

        #endregion
        #region --- IEnumerable<ResourceGroup> Members ---

        public IEnumerator<ResourceGroup> GetEnumerator()
        {
            return mLanguages.GetEnumerator();
        }

        #endregion
        #region --- IEnumerable Members ---

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mLanguages.GetEnumerator();
        }

        #endregion
    }

    
}
