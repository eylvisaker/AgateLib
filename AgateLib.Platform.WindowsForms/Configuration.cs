using AgateLib.Drivers;
using AgateLib.Platform.WindowsForms.DisplayImplementation;
using AgateLib.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Platform.WindowsForms.Factories;
using AgateLib.Utility;

namespace AgateLib.Platform.WindowsForms
{
	public static class Configuration
	{
		static FileProviderList mResourceProvider = new FileProviderList();
		static FileProviderList mImageProvider = new FileProviderList();
		static FileProviderList mSoundProvider = new FileProviderList();
		static FileProviderList mMusicProvider = new FileProviderList();

		public static void Initialize()
		{
			Core.Initialize(new FormsFactory());
		}

		public static FileProviderList Resources { get { return mResourceProvider; } }
		public static FileProviderList Images { get { return mImageProvider; } }
		public static FileProviderList Sounds { get { return mSoundProvider; } }
		public static FileProviderList MusicProvider { get { return mMusicProvider; } }
	}
}
