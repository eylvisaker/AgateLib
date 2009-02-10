using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AgateLib.Utility
{
    public class FileProviderList : IList<IFileProvider>
    {
        List<IFileProvider> mProviders = new List<IFileProvider>();

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

        public IEnumerable<string> GetAllFiles(string filter)
        {
            for (int i = mProviders.Count - 1; i >= 0; i--)
            {
                foreach (string files in mProviders[i].GetAllFiles(filter))
                    yield return files;
            }
        }
        internal IEnumerable<string> GetAllFiles()
        {
            for (int i = mProviders.Count - 1; i >= 0; i--)
            {
                foreach (string files in mProviders[i].GetAllFiles())
                    yield return files;
            }
        }

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
                mProviders[index] = value;
            }
        }

        #endregion
        #region ICollection<IFileProvider> Members

        public void Add(IFileProvider item)
        {
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
