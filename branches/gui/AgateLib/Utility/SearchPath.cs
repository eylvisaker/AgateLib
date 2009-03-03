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
    [Obsolete("Use AgateFileProvider instead.")]
    public class SearchPath : ICollection<string>, IList<string>
    {
        FileProviderList mProvider;

        internal SearchPath(FileProviderList provider)
        {
            mProvider = provider;
        }

        /// <summary>
        /// Constructs a SearchPath object.  No default paths are added
        /// (You might want to consider using SearchPath(".") instead to include
        /// the current directory.)
        /// </summary>
        [Obsolete("Use AgateFileProvider methods instead.", true)]
        public SearchPath()
        {
        }
        /// <summary>
        /// Constructs a SearchPath object, and adds all the paths specified
        /// to the list of paths to search.
        /// </summary>
        /// <param name="array">A comma delimited list of strings.</param>
        [Obsolete("Use AgateFileProvider methods instead.", true)]
        public SearchPath(params string[] array)
        {
            foreach (string s in array)
                Add(s);
        }
        /// <summary>
        /// Constructs a SearchPath object, and adds all the paths specified
        /// to the list of paths to search, as well as subdirectories up to the
        /// level specified.
        /// </summary>
        /// <param name="levels">How many subdirectories each to add.</param>
        /// <param name="array">A comma delimited list of strings.</param>
        [Obsolete]
        public SearchPath(int levels, params string[] array)
        {
            foreach (string s in array)
                Add(s, levels);
        }
        /// <summary>
        /// Recursively adds the specified directory, and all subdirectories up
        /// to the specified number of levels deep.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="levels"></param>
        public void Add(string item, int levels)
        {
            if (levels != 0)
                throw new NotImplementedException("Adding directory levels not supported.");

            mProvider.AddPath(item);
        }
        /// <summary>
        /// Searches through all directories in the SearchPath object for the specified
        /// filename.  The search is performed in the order directories have been added,
        /// and the first result is returned.  If no file is found, null is returned.
        /// </summary>
        /// <param name="filename">Filename to search for.</param>
        /// <returns>The full path of the file, if it exists.  Null if no file is found.</returns>
        public string FindFileName(string filename)
        {
            if (mProvider.FileExists(filename))
                return filename;
            else
                return null;
        }

        /// <summary>
        /// Gets all files in all paths.
        /// </summary>
        /// <returns></returns>
        public ICollection<string> GetAllFiles()
        {
            List<string> files = new List<string>();
            files.AddRange(mProvider.GetAllFiles());

            return files;
        }
        /// <summary>
        /// Gets all files in all paths that match the specified search pattern.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public ICollection<string> GetAllFiles(string searchPattern)
        {
            List<string> files = new List<string>();
            files.AddRange(mProvider.GetAllFiles(searchPattern));

            return files;
        }

        /// <summary>
        /// Provides debugging information.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return mProvider.ToString();
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
            mProvider.Clear();
        }
        /// <summary>
        /// Checks to see if the given path is already in the list.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(string item)
        {
            foreach (IFileProvider provider in mProvider)
            {
                FileSystemProvider f = provider as FileSystemProvider;

                if (f == null)
                    continue;

                if (f.SearchPath == item)
                    return true;
            }

            return false;
        }
        /// <summary>
        /// Copies the list of paths to an array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(string[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets how many search paths are listed here.
        /// </summary>
        public int Count
        {
            get { return mProvider.Count; }
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
            throw new NotImplementedException();
        }

        #endregion
        #region --- IList<string> Members ---

        int IList<string>.IndexOf(string item)
        {
            throw new NotImplementedException();
        }

        void IList<string>.Insert(int index, string item)
        {
            mProvider.Insert(index, new FileSystemProvider(item));
        }

        void IList<string>.RemoveAt(int index)
        {
            mProvider.RemoveAt(index);
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
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
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
            throw new NotImplementedException();
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
