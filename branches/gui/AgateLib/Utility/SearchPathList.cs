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
using System.IO;

namespace AgateLib.Utility
{
    /// <summary>
    /// A class which is used to simplify searching for files in several different
    /// directories.
    /// All paths entered are stored as absolute paths, so do not expect to add an
    /// item and retrieve it and have it be the same thing.
    /// <code></code>
    /// </summary>
    [Obsolete]
    public sealed class SearchPathList : ISearchPathList
    {
        List<string> mSearchPaths = new List<string>();

        /// <summary>
        /// Constructs a SearchPathList object.  No default paths are added
        /// (You might want to consider using SearchPathList(".") instead to include
        /// the current directory.)
        /// </summary>
        public SearchPathList()
        {
        }
        /// <summary>
        /// Constructs a SearchPathList object, and adds all the paths specified
        /// to the list of paths to search.
        /// </summary>
        /// <param name="array">A comma delimited list of strings.</param>
        public SearchPathList(params string[] array)
        {
            foreach (string s in array)
                Add(s);
        }

        /// <summary>
        /// Recursively adds the specified directory, and all subdirectories up
        /// to the specified number of levels deep.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="levels"></param>
        void Add(string item, int levels)
        {
            if (Directory.Exists(item) == false)
                throw new DirectoryNotFoundException();

            item = Path.GetFullPath(item);

            mSearchPaths.Add(item);

            if (levels > 0)
            {
                string[] dirs = Directory.GetDirectories(item);

                foreach (string dir in dirs)
                    Add(dir, levels - 1);
            }
        }
        
        /// <summary>
        /// Provides debugging information.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string retval = "";
            int count = 0;

            foreach (string s in mSearchPaths)
            {
                if (count > 0)
                    retval += "; ";

                retval += "\"" + s + "\"";

                count++;
            }

            return retval;
        }

        #region --- ICollection<string> Members ---
        /// <summary>
        /// Adds a search path to the list.
        /// </summary>
        /// <param name="item"></param>
        public void Add(string item)
        {
            Add(item, 0);
        }
        /// <summary>
        /// Clears all search paths from the list.
        /// </summary>
        public void Clear()
        {
            mSearchPaths.Clear();
        }
        /// <summary>
        /// Checks to see if the given path is already in the list.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(string item)
        {
            return mSearchPaths.Contains(Path.GetFullPath(item));
        }
        /// <summary>
        /// Copies the list of paths to an array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(string[] array, int arrayIndex)
        {
            mSearchPaths.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Gets how many search paths are listed here.
        /// </summary>
        public int Count
        {
            get { return mSearchPaths.Count; }
        }

        bool ICollection<string>.IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// Removes an item from the search path list.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(string item)
        {
            return mSearchPaths.Remove(Path.GetFullPath(item));
        }

        #endregion
        #region --- IList<string> Members ---

        int IList<string>.IndexOf(string item)
        {
            return mSearchPaths.IndexOf(Path.GetFullPath(item));
        }

        void IList<string>.Insert(int index, string item)
        {
            mSearchPaths.Insert(index, Path.GetFullPath(item));
        }

        void IList<string>.RemoveAt(int index)
        {
            mSearchPaths.RemoveAt(index);
        }
        /// <summary>
        /// Returns a search path at a given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index]
        {
            get
            {
                return mSearchPaths[index];
            }
            set
            {
                mSearchPaths[index] = Path.GetFullPath(value);
            }
        }

        #endregion

        #region --- IEnumerable<string> Members ---
        /// <summary>
        /// Gets an IEnumerator&lt;string&gt; object for iterating through
        /// the search paths.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<string> GetEnumerator()
        {
            return mSearchPaths.GetEnumerator();
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
