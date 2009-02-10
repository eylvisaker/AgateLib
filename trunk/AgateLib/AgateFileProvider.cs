using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AgateLib.Utility;

namespace AgateLib
{
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
        public static FileProviderList Images
        {
            get { return mImageProvider; }
        }
        public static FileProviderList Sounds
        {
            get { return mSoundProvider; }
        }
        public static FileProviderList Music
        {
            get { return mMusicProvider; }
        }
        public static FileProviderList Resources
        {
            get { return mResourceProvider; }
        }
    }
}
