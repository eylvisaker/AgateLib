using System.IO;

namespace AgateLib.Storage
{
    /// <summary>
    /// Interface that provides storage on a per-user basis.
    /// </summary>
    public interface IUserStorage
    {
        /// <summary>
        /// For desktop platforms, sets the folder inside the root app data folder for the storage
        /// for this user.
        /// </summary>
        /// <param name="folderName">Name of the folder. Recommended is to use
        /// company_name/product_name, eg. "VermilionTower/ThornbridgeSaga"</param>
        /// <returns></returns>
        void SetApplicationDataFolder(string folderName);

        bool FileExists(string path);
        Stream OpenFile(string path, FileMode fileMode);
        Stream OpenFile(string path, FileMode fileMode, FileAccess fileAccess);
        Stream OpenFile(string path, FileMode fileMode, FileAccess fileAccess, FileShare fileShare);
        void DeleteFile(string path);
    }
}
