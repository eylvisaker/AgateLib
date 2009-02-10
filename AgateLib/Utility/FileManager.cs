//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.IO;

namespace AgateLib.Utility
{
    /// <summary>
    /// Manages file locations and automatic finding of files.
    /// </summary>e
    [Obsolete("Use AgateFileProvider class instead.")]
    public static class FileManager
    {
        private static string mPathChars = "/";

		private static SearchPath mAssemblyPath = new SearchPath(AgateFileProvider.Assemblies);
        private static SearchPath mImagePath = new SearchPath(AgateFileProvider.Images);
        private static SearchPath mAudioPath = new SearchPath(AgateFileProvider.Sounds);
        private static SearchPath mResourcePath = new SearchPath(AgateFileProvider.Resources);

        private static bool mBufferStreams = true;
        private static int mBufferSize = 1000;

        /// <summary>
        /// Indicates the characters that the application can use to specify
        /// directories, when using methods in this class to open files.
        /// 
        /// Defaults to "/".  
        /// </summary>
        [Obsolete("Always use forward slash \"/\" to separate paths.")]
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
        /// Gets or sets the SearchPath object which contains the paths used for
        /// finding resource definition files.  
        /// Defaults to containing only the current directory.
        /// </summary>
        public static SearchPath ResourcePath
        {
            get { return mResourcePath; }
            set { mResourcePath = value; }
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
