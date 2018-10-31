using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;

namespace AgateLib.Storage
{
    [Singleton]
    public class UserStorage
    {
        private IsolatedStorageFile iso;

        public UserStorage()
        {
            //iso = IsolatedStorageFile.GetUserStoreForApplication();
            iso = IsolatedStorageFile.GetUserStoreForDomain();
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
