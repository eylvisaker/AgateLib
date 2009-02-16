using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AgateLib.Utility
{
    /// <summary>
    /// Public interface that should be implemented 
    /// </summary>
    public interface IFileProvider
    {
        /// <summary>
        /// Opens the specified file returning a stream.  Throws 
        /// FileNotFoundException if the file does not exist.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        Stream OpenRead(string filename);
        /// <summary>
        /// Checks to if the specified file exists in the file provider.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        bool FileExists(string filename);

        /// <summary>
        /// Enumerates through all filenames in the file provider.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetAllFiles();
        /// <summary>
        /// Enumerates through all filenames which match the specified search pattern.
        /// </summary>
        /// <remarks>The search pattern is not regex style pattern matching, rather it should 
        /// be bash pattern matching, so a searchPattern of "*" would match all files, and
        /// "*.*" would match all filenames with a period in them.</remarks>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        IEnumerable<string> GetAllFiles(string searchPattern);
    }
}
