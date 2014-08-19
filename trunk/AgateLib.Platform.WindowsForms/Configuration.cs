﻿using AgateLib.Drivers;
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
	[Obsolete]
	public static class Configuration
	{
		static FileProviderList mAssetProvider = new FileProviderList();
		static FileProviderList mResourceProvider = new FileProviderList();
		static FileProviderList mSurfaceProvider = new FileProviderList();
		static FileProviderList mSoundProvider = new FileProviderList();
		static FileProviderList mMusicProvider = new FileProviderList();

		[Obsolete]
		public static void Initialize()
		{

		}

		[Obsolete("Use AgateLib.IO.FileProvider instead.", true)]
		public static FileProviderList Resources { get { return mResourceProvider; } }
		[Obsolete("Use AgateLib.IO.FileProvider instead.", true)]
		public static FileProviderList Images { get { return mSurfaceProvider; } }
		[Obsolete("Use AgateLib.IO.FileProvider instead.", true)]
		public static FileProviderList Sounds { get { return mSoundProvider; } }
		[Obsolete("Use AgateLib.IO.FileProvider instead.", true)]
		public static FileProviderList MusicProvider { get { return mMusicProvider; } }
	}
}