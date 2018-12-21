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
#if XBOX
            iso = IsolatedStorageFile.GetUserStoreForApplication();
#else
            iso = IsolatedStorageFile.GetUserStoreForDomain();
#endif
        }

        public bool FileExists(string path)
        {
            return iso.FileExists(path);
        }

        public Stream OpenFile(string path, FileMode fileMode)
        {
            CreateDirectoryIfApplicable(path, fileMode);

            return iso.OpenFile(path, fileMode);
        }
        
        public Stream OpenFile(string path, FileMode fileMode, FileAccess fileAccess)
        {
            CreateDirectoryIfApplicable(path, fileMode);

            return iso.OpenFile(path, fileMode, fileAccess);
        }

        public Stream OpenFile(string path, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            CreateDirectoryIfApplicable(path, fileMode);

            return iso.OpenFile(path, fileMode, fileAccess, fileShare);
        }

        private void CreateDirectoryIfApplicable(string path, FileMode fileMode)
        {
            if (fileMode == FileMode.Create ||
                            fileMode == FileMode.CreateNew ||
                            fileMode == FileMode.OpenOrCreate)
            {
                iso.CreateDirectory(Path.GetDirectoryName(path));
            }
        }
    }
}
