using NLog;
using System;
using System.IO;

namespace AgateLib.Storage
{
    /// <summary>
    /// Class that provides storage on a per-user basis.
    /// </summary>
    [Singleton]
    public class UserStorage : IUserStorage
    {
#if DESKTOP
        private bool appDataFolderSet, noRootWarning;

        private string appDataFolder;
        private string rootFolder;
        private readonly Logger log;

        public UserStorage()
        {
            appDataFolder = FirstRooted(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".saved_games"));

            rootFolder = Path.Combine(appDataFolder, "UnknownApplication");
            log = LogManager.GetCurrentClassLogger();
        }

        private string FirstRooted(params string[] paths)
        {
            foreach (string path in paths)
            {
                if (Path.IsPathRooted(path))
                {
                    return path;
                }
            }

            return null;
        }

        public void SetApplicationDataFolder(string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
            {
                throw new ArgumentException(nameof(folderName));
            }

            rootFolder = Path.Combine(appDataFolder, folderName);

            appDataFolderSet = true;
            noRootWarning = false;

            log.Info($"User storage set to {rootFolder}.");
            Directory.CreateDirectory(rootFolder);
        }

        public bool FileExists(string localPath)
        {
            WarnIfNoRoot();
            string path = MapPath(localPath);

            return File.Exists(path);
        }

        public void DeleteFile(string localPath)
        {
            WarnIfNoRoot();
            string path = MapPath(localPath);

            File.Delete(localPath);
        }

        public Stream OpenFile(string localPath, FileMode fileMode)
        {
            WarnIfNoRoot();
            string path = MapPath(localPath);

            CreateDirectoryIfApplicable(path, fileMode);

            return File.Open(path, fileMode);
        }

        public Stream OpenFile(string localPath, FileMode fileMode, FileAccess fileAccess)
        {
            WarnIfNoRoot();
            string path = MapPath(localPath);

            CreateDirectoryIfApplicable(path, fileMode);

            return File.Open(path, fileMode, fileAccess);
        }

        public Stream OpenFile(string localPath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            WarnIfNoRoot();
            string path = MapPath(localPath);

            CreateDirectoryIfApplicable(path, fileMode);

            return File.Open(path, fileMode, fileAccess, fileShare);
        }

        private void CreateDirectoryIfApplicable(string fullPath, FileMode fileMode)
        {
            if (fileMode == FileMode.Create ||
                fileMode == FileMode.CreateNew ||
                fileMode == FileMode.OpenOrCreate)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            }
        }

        private void WarnIfNoRoot()
        {
            if (appDataFolderSet)
            {
                return;
            }

            if (noRootWarning)
            {
                return;
            }

            log.Warn($"No root folder set. User data is stored in {rootFolder}");
            noRootWarning = true;
        }

        private string MapPath(string path) => Path.Combine(rootFolder, path);

#endif

        #region --- UWP ---
#if WINDOWS_UWP || ANDROID
        private IsolatedStorageFile iso;

        public UserStorage()
        {
            iso = IsolatedStorageFile.GetUserStoreForApplication();
            
            // If the above line doesn't work, this one might suffice.
            // iso = IsolatedStorageFile.GetUserStoreForDomain();
        }

        public void SetApplicationDataFolder(string folderName)
        {
            // no-op, since we're using isolated storage.
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
#endif

        #endregion
    }
}
