using AgateLib.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.IO
{
	public static class FileProvider
	{
		static IReadFileProvider mSurfaceAssets;
		static IReadFileProvider mResourceAssets;
		static IReadFileProvider mMusicAssets;
		static IReadFileProvider mSoundAssets;

		internal static void Initialize(IPlatformFactory platformFactory)
		{
			Assets = platformFactory.CreateAssetFileProvider();
		}

		public static IReadFileProvider Assets { get; private set; }
		public static IReadFileProvider SurfaceAssets
		{
			get { return mSurfaceAssets ?? Assets; }
			set { mSurfaceAssets = Assets; }
		}
		public static IReadFileProvider ResourceAssets
		{
			get { return mResourceAssets ?? Assets; }
			set { mResourceAssets = Assets; }
		}
		public static IReadFileProvider MusicAssets
		{
			get { return mMusicAssets ?? Assets; }
			set { mMusicAssets = Assets; }
		}
		public static IReadFileProvider SoundAssets
		{
			get { return mSoundAssets ?? Assets; }
			set { mSoundAssets = Assets; }
		}
		
		public static IReadWriteFileProvider UserFiles { get; private set; }

	}
}
