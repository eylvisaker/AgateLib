using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AgateLib.Utility
{
    /// <summary>
    /// Contains a list of IFileProvider objects that are used to search for
    /// and open files.
    /// </summary>
    public class FileProviderList : IList<IFileProvider>, IFileProvider 
    {
        List<IFileProvider> mProviders = new List<IFileProvider>();

        /// <summary>
        /// Opens a specified file by searching backwards through the list of 
        /// providers until a matching filename is found.  A FileNotFoundException
        /// is thrown if the file does not exist.
        /// </summary>
        /// <param name="filename">The filename to search for.</param>
        /// <returns></returns>
        public Stream OpenRead(string filename)
        {
            for (int i = mProviders.Count - 1; i >= 0; i--)
            {
                if (mProviders[i].FileExists(filename))
                {
                    return mProviders[i].OpenRead(filename);
                }
            }

            throw new FileNotFoundException("Could not find the file.", filename);
        }

        /// <summary>
        /// Returns all filenames matching the specified filter in 
        /// all file providers.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<string> GetAllFiles(string filter)
        {
            for (int i = mProviders.Count - 1; i >= 0; i--)
            {
                foreach (string files in mProviders[i].GetAllFiles(filter))
                    yield return files;
            }
        }
        /// <summary>
        /// Returns all filenames in all file providers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllFiles()
        {
            for (int i = mProviders.Count - 1; i >= 0; i--)
            {
                foreach (string files in mProviders[i].GetAllFiles())
                    yield return files;
            }
        }

        /// <summary>
        /// Returns true if the specified file exists in a file provider.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool FileExists(string filename)
        {
            for (int i = mProviders.Count - 1; i >= 0; i--)
            {
                if (mProviders[i].FileExists(filename))
                    return true;
            }

            return false;
        }
        /// <summary>
        /// Adds a path in the filesystem to the list of locations to search when openning a file.
        /// </summary>
        /// <param name="path"></param>
        public void AddPath(string path)
        {
            Add(new FileSystemProvider(path));
        }

        #region IList<IFileProvider> Members

        public int IndexOf(IFileProvider item)
        {
            return mProviders.IndexOf(item);
        }

        public void Insert(int index, IFileProvider item)
        {
            if (item is FileProviderList)
            {
                if (item == this) throw new ArgumentException("Cannot add a FileProviderList to itself!");
            }
            
            mProviders.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            mProviders.RemoveAt(index);
        }

        public IFileProvider this[int index]
        {
            get
            {
                return mProviders[index];
            }
            set
            {
                if (value is FileProviderList)
                {
                    if (value == this) throw new ArgumentException("Cannot add a FileProviderList to itself!");
                }
            
                mProviders[index] = value;
            }
        }

        #endregion
        #region ICollection<IFileProvider> Members

        public void Add(IFileProvider item)
        {
            if (item is FileProviderList)
            {
                if (item == this) throw new ArgumentException("Cannot add a FileProviderList to itself!");
            }
            mProviders.Add(item);
        }

        public void Clear()
        {
            mProviders.Clear();
        }

        public bool Contains(IFileProvider item)
        {
            return mProviders.Contains(item);
        }

        public void CopyTo(IFileProvider[] array, int arrayIndex)
        {
            mProviders.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return mProviders.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(IFileProvider item)
        {
            return mProviders.Remove(item);
        }

        #endregion
        #region IEnumerable<IFileProvider> Members

        public IEnumerator<IFileProvider> GetEnumerator()
        {
            return mProviders.GetEnumerator();
        }

        #endregion
        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion


    }
}
