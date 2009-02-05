using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AgateLib.Utility
{
    public static class AgateFileProvider
    {
        static IFileProvider mAssemblyProvider;
        static IFileProvider mImageProvider = new FileSystemProvider(".");
        static IFileProvider mSoundProvider = new FileSystemProvider(".");
        static IFileProvider mMusicProvider = new FileSystemProvider(".");
        static IFileProvider mResourceProvider = new FileSystemProvider(".");

        static AgateFileProvider()
		{
			Initialize();
		}

        static void Initialize()
        {
            string location = System.Reflection.Assembly.GetEntryAssembly().Location;

            mAssemblyProvider = new FileSystemProvider(Path.GetDirectoryName(location));
        }

        /// <summary>
        /// The file provider for getting driver assemblies.  It is not recommended that you use this
        /// because it will cause assemblies to be loaded in the "LoadFrom" context.  Instead, place
        /// driver assemblies in the same directory as the application.
        /// </summary>
        public static IFileProvider AssemblyProvider
        {
            get { return mAssemblyProvider; }
            set { mAssemblyProvider = value; }
        }
        public static IFileProvider ImageProvider
        {
            get { return mImageProvider; }
            set { mImageProvider = value; }
        }
        public static IFileProvider SoundProvider
        {
            get { return mSoundProvider; }
            set { mSoundProvider = value; }
        }
        public static IFileProvider MusicProvider
        {
            get { return mMusicProvider; }
            set { mMusicProvider = value; }
        }
        public static IFileProvider ResourceProvider
        {
            get { return mResourceProvider; }
            set { mResourceProvider = value; }
        }
    }
}
