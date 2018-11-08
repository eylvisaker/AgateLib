using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;

namespace AgateLib.Storage
{
    /// <summary>
    /// Interface that provides storage on a per-user basis.
    /// </summary>
    public interface IUserStorage
    {
        bool FileExists(string path);
        Stream OpenFile(string path, FileMode fileMode);
        Stream OpenFile(string path, FileMode fileMode, FileAccess fileAccess);
        Stream OpenFile(string path, FileMode fileMode, FileAccess fileAccess, FileShare fileShare);
    }

    /// <summary>
    /// Class that provides storage on a per-user basis.
    /// </summary>
    [Singleton]
    public class UserStorage : IUserStorage
    {
        private IsolatedStorageFile iso;

        public UserStorage()
        {
            //iso = IsolatedStorageFile.GetUserStoreForApplication();
            iso = IsolatedStorageFile.GetUserStoreForDomain();
        }

        public bool FileExists(string path)
        {
            return iso.FileExists(path);
        }

        public Stream OpenFile(string path, FileMode fileMode)
        {
            return iso.OpenFile(path, fileMode);
        }

        public Stream OpenFile(string path, FileMode fileMode, FileAccess fileAccess)
        {
            return iso.OpenFile(path, fileMode, fileAccess);
        }

        public Stream OpenFile(string path, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            return iso.OpenFile(path, fileMode, fileAccess, fileShare);
        }
    }
}
