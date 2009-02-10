using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AgateLib.Utility
{
    public interface IFileProvider
    {
        /// <summary>
        /// Opens the passed file 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        Stream OpenRead(string filename);
        bool FileExists(string filename);

        IEnumerable<string> GetAllFiles();
        IEnumerable<string> GetAllFiles(string searchPattern);
    }
}
