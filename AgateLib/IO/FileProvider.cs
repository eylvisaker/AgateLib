using AgateLib.ApplicationModels;
using AgateLib.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.IO
{
	public static class FileProvider
	{
		internal static void Initialize(IPlatformFactory platformFactory)
		{
			Assets = platformFactory.CreateAssetFileProvider();
		}

		public static void Initialize(IReadFileProvider assetProvider, AssetLocations assetLocations)
		{
			Assets = assetProvider;

			SurfaceAssets = NewProviderFromSubdirectory(Assets, assetLocations.Surfaces);
			SoundAssets = NewProviderFromSubdirectory(Assets, assetLocations.Sound);
			MusicAssets = NewProviderFromSubdirectory(Assets, assetLocations.Music);
			ResourceAssets = NewProviderFromSubdirectory(Assets, assetLocations.Resources);
		}

		public static IReadFileProvider Assets { get; set; }

		public static IReadFileProvider SurfaceAssets { get; set; }
		public static IReadFileProvider ResourceAssets { get; set; }
		public static IReadFileProvider MusicAssets { get; set; }
		public static IReadFileProvider SoundAssets { get; set; }

		public static IReadWriteFileProvider UserFiles { get; set; }

		static IReadFileProvider NewProviderFromSubdirectory(IReadFileProvider parent, string subdir)
		{
			if (string.IsNullOrWhiteSpace(subdir) || subdir == ".")
				return parent;

			return new SubdirectoryProvider(parent, subdir);
		}
	}
}
