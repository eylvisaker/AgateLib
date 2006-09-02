using System;
using System.Collections.Generic;
using System.IO;

namespace ERY.AgateLib
{
    /// <summary>
    /// Manages file locations and automatic finding of files.
    /// </summary>
    public static class FileManager
    {
        private static string mPathChars = "/";

        private static SearchPath mAssemblyPath;
        private static SearchPath mImagePath = new SearchPath(".");
        private static SearchPath mAudioPath = new SearchPath(".");

        private static bool mBufferStreams = true;
        private static int mBufferSize = 1000;

        private static bool mInitialized = false;
        internal static void Initialize()
        {
            if (mInitialized)
                return;

            string location = System.Reflection.Assembly.GetEntryAssembly().Location;

            mAssemblyPath = new SearchPath(Path.GetDirectoryName(location));

            mInitialized = true;
        }
        /// <summary>
        /// Indicates the characters that the application can use to specify
        /// directories, when using methods in this class to open files.
        /// 
        /// Defaults to "/".  
        /// </summary>
        public static string PathChars
        {
            get { return mPathChars; }
            set { mPathChars = value; }
        }

        /// <summary>
        /// Gets or sets the SearchPath object which contains the paths used for
        /// finding assemblies.  Defaults to containing only the directory where
        /// the application executable is located.
        /// </summary>
        public static SearchPath AssemblyPath
        {
            get { return mAssemblyPath; }
            set { mAssemblyPath = value; }
        }
        /// <summary>
        /// Gets or sets the SearchPath object which contains the paths used for
        /// finding images.  Defaults to containing only the current directory.
        /// </summary>
        public static SearchPath ImagePath
        {
            get { return mImagePath; }
            set { mImagePath = value; }
        }
        /// <summary>
        /// Gets or sets the SearchPath object which contains the paths used for
        /// finding sound files.  Defaults to containing only the current directory.
        /// </summary>
        public static SearchPath SoundPath
        {
            get { return mAudioPath; }
            set { mAudioPath = value; }
        }
        /// <summary>
        /// Gets or sets the SearchPath object which contains the paths used for
        /// finding music files.  Defaults to containing only the current directory.
        /// </summary>
        public static SearchPath MusicPath
        {
            get { return mAudioPath; }
            set { mAudioPath = value; }
        }
        /// <summary>
        /// Gets or sets a boolean value indicating whether or not streams created
        /// by the FileManager should be wrapped in a BufferedStream object.
        /// </summary>
        public static bool BufferStreams
        {
            get { return mBufferStreams; }
            set { mBufferStreams = value; }
        }
        /// <summary>
        /// Gets or sets how large of a buffer should be used if BufferStreams is true.
        /// </summary>
        public static int BufferSize
        {
            get { return mBufferSize; }
            set { mBufferSize = value; }
        }
        /// <summary>
        /// Opens an existing file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="mode"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        public static Stream OpenFile(SearchPath path, string filename, FileMode mode, FileAccess access)
        {
            string filepath = path.FindFileName(filename);

            if (filepath == null)
                throw new FileNotFoundException("The file was not found.", filename);

            FileStream f = new FileStream(filepath, mode, access);

            return SetBuffer(f);
        }

        private static Stream SetBuffer(FileStream f)
        {
            if (BufferStreams)
            {
                BufferedStream b = new BufferedStream(f, mBufferSize);

                return b;
            }
            else
                return f;
        }
    }
}
