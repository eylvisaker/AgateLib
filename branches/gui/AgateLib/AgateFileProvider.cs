using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AgateLib.Utility;

namespace AgateLib
{
    /// <summary>
    /// Provides an interface for opening files.  Objects implementing IFileProvider can
    /// be added to separate lists for images, sounds, etc. which are used to find files
    /// when Surfaces, SoundBuffers, etc. are constructed.
    /// </summary>
    public static class AgateFileProvider
    {
        static FileProviderList mAssemblyProvider = new FileProviderList();
        static FileProviderList mImageProvider = new FileProviderList();
        static FileProviderList mSoundProvider = new FileProviderList();
        static FileProviderList mMusicProvider = new FileProviderList();
        static FileProviderList mResourceProvider = new FileProviderList();

        static AgateFileProvider()
		{
			Initialize();
		}

        static void Initialize()
        {
            string location = System.Reflection.Assembly.GetEntryAssembly().Location;

            mAssemblyProvider.Add(new FileSystemProvider(Path.GetDirectoryName(location)));
            mImageProvider.Add(new FileSystemProvider("."));
            mSoundProvider.Add(new FileSystemProvider("."));
            mMusicProvider.Add(new FileSystemProvider("."));
            mResourceProvider.Add(new FileSystemProvider("."));
        }

        /// <summary>
        /// The file provider for getting driver assemblies.  It is not recommended that you use this
        /// because it will cause assemblies to be loaded in the "LoadFrom" context.  Instead, place
        /// driver assemblies in the same directory as the application.
        /// </summary>
        public static FileProviderList Assemblies
        {
            get { return mAssemblyProvider; }
        }
        /// <summary>
        /// A list of the default file providers for opening images when a Surface is constructed.
        /// </summary>
        public static FileProviderList Images
        {
            get { return mImageProvider; }
        }
        /// <summary>
        /// A list of the default file providers for opening sounds when a SoundBuffer is constructed.
        /// </summary>
        public static FileProviderList Sounds
        {
            get { return mSoundProvider; }
        }
        /// <summary>
        /// A list of the default file providers for opening sounds when a Music object is constructed.
        /// </summary>
        public static FileProviderList Music
        {
            get { return mMusicProvider; }
        }
        /// <summary>
        /// A list of the default file providers for opening resources.
        /// </summary>
        public static FileProviderList Resources
        {
            get { return mResourceProvider; }
        }

        /// <summary>
        /// Saves a stream to a file in the temp path.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string SaveStreamToTempFile(Stream stream)
        {
            string tempfile = Path.GetTempFileName();

            byte[] buffer = new byte[stream.Length];

            using (FileStream tempstream = File.OpenWrite(tempfile))
            {
                stream.Read(buffer, 0, (int)stream.Length);

                tempstream.Write(buffer, 0, buffer.Length);
            }

            return tempfile;
        }
    }
}
