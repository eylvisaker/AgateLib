using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AgateLib.Utility
{
    public class FileSystemProvider : IFileProvider
    {
        ISearchPathList mPathList;

        public FileSystemProvider()
        {
            mPathList = new SearchPathList(".");
        }
        public FileSystemProvider(params string[] paths)
        {
            mPathList = new SearchPathList(paths);
        }

        public Stream OpenRead(string filename)
        {
            string resolvedName = FindFileName(filename);
            if (resolvedName == null)
                throw new FileNotFoundException(string.Format("The file {0} was not found in any of the paths {1}.",
                    filename, mPathList), filename);

            return File.OpenRead(FindFileName(filename));
        }

        public bool FileExists(string filename)
        {
            if (FindFileName(filename) != null)
                return true;
            else
                return false;
        }


        /// <summary>
        /// Searches through all directories in the SearchPathList object for the specified
        /// filename.  The search is performed in the order directories have been added,
        /// and the first result is returned.  If no file is found, null is returned.
        /// </summary>
        /// <param name="filename">Filename to search for.</param>
        /// <returns>The full path of the file, if it exists.  Null if no file is found.</returns>
        string FindFileName(string filename)
        {
            if (filename == null)
                return null;

            DebugCrossPlatform(filename);

            foreach (string dir in PathList)
            {
                string path = Path.Combine(dir, filename);

                if (File.Exists(path))
                    return path;
            }

            return null;
        }


        private void DebugCrossPlatform(string filename)
        {
            if (filename == null)
                return;

            if (FileManager.CheckCrossPlatform(filename) == false)
            {
                System.Diagnostics.Debug.WriteLine("The path \"" + filename + "\" is not entered in a cross-platform manner.");
                System.Diagnostics.Debug.WriteLine("Avoid using the following characters:  " + FileManager.NonCrossPlatformChars);
            }
        }

        public ISearchPathList PathList
        {
            get
            {
                return mPathList;
            }
  
        }

        /// <summary>
        /// Gets all files in all paths.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllFiles()
        {
            List<string> files = new List<string>();

            foreach (string dir in PathList)
            {
                files.AddRange(Directory.GetFiles(dir));
            }

            return files;
        }
        /// <summary>
        /// Gets all files in all paths that match the specified search pattern.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public IEnumerable<string> GetAllFiles(string searchPattern)
        {
            List<string> files = new List<string>();

            foreach (string dir in PathList)
            {
                files.AddRange(Directory.GetFiles(dir, searchPattern));
            }

            return files;
        }


        public void AddPath(string path)
        {
            PathList.Add(path);
        }

    }
}
