using AgateLib.ApplicationModels;
using AgateLib.Quality;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.IO
{
	public static class Assets
	{
		static IReadFileProvider mAssetProvider;
		static IReadFileProvider mSurfaces;
		static IReadFileProvider mResources;
		static IReadFileProvider mMusic;
		static IReadFileProvider mSounds;
		static IReadFileProvider mUserInterfaceAssets;
		
		public static void AddAssetLocations(IReadFileProvider ap, AssetLocations assetLocations)
		{
			Condition.Requires<ArgumentNullException>(ap != null, "ap");

			// check for circular reference
			VerifyNoncircular(assetLocations);

			var assetProvider = NewProviderFromSubdirectory(ap, assetLocations.Path);
			var surfaces = NewProviderFromSubdirectory(assetProvider, assetLocations.Surfaces);
			var sounds = NewProviderFromSubdirectory(assetProvider, assetLocations.Sound);
			var music = NewProviderFromSubdirectory(assetProvider, assetLocations.Music);
			var resources = NewProviderFromSubdirectory(assetProvider, assetLocations.Resources);
			var userInterfaceAssets = NewProviderFromSubdirectory(assetProvider, assetLocations.UserInterface);

			AddOrCombine(ref mAssetProvider, assetProvider);
			AddOrCombine(ref mSurfaces, surfaces);
			AddOrCombine(ref mResources, resources);
			AddOrCombine(ref mMusic, music);
			AddOrCombine(ref mSounds, sounds);
			AddOrCombine(ref mUserInterfaceAssets, userInterfaceAssets);

			if (assetLocations.ExtraAssets != null)
			{
				AddAssetLocations(ap, assetLocations.ExtraAssets);
			}
		}

		private static void VerifyNoncircular(AssetLocations al)
		{
			List<AssetLocations> examined = new List<AssetLocations>();

			while (al.ExtraAssets != null)
			{
				if (examined.Contains(al))
					throw new AgateException("AssetLocations passed contains circular references.");

				examined.Add(al);
				al = al.ExtraAssets;
			}
		}

		private static void AddOrCombine(ref IReadFileProvider storage, IReadFileProvider provider)
		{
			if (storage == null)
				storage = provider;
			else if (storage is ReadFileProviderList)
			{
				var rfpl = (ReadFileProviderList)storage;

				rfpl.Add(provider);
			}
			else
			{
				var rfpl = new ReadFileProviderList();

				rfpl.Add(storage);
				rfpl.Add(provider);

				storage = rfpl;
			}
		}

		static IReadFileProvider NewProviderFromSubdirectory(IReadFileProvider parent, string subdir)
		{
			if (string.IsNullOrWhiteSpace(subdir) || subdir == ".")
				return parent;

			return new SubdirectoryProvider(parent, subdir);
		}

		internal static IReadFileProvider AssetProvider { get { return mAssetProvider; } set { mAssetProvider = value; } }
		public static IReadFileProvider Surfaces { get { return mSurfaces; } set { mSurfaces = value; } }
		public static IReadFileProvider Resources { get { return mResources; } set { mResources = value; } }
		public static IReadFileProvider Music { get { return mMusic; } set { mMusic = value; } }
		public static IReadFileProvider Sounds { get { return mSounds; } set { mSounds = value; } }
		public static IReadFileProvider UserInterfaceAssets { get { return mUserInterfaceAssets; } set { mUserInterfaceAssets = value; } }

		/// <summary>
		/// Opens the specified file returning a stream.  Throws 
		/// FileNotFoundException if the file does not exist.
		/// </summary>
		/// <param name="filename">The path and filename of the file to read from.</param>
		/// <returns></returns>
		public static Task<Stream> OpenReadAsync(string filename)
		{
			return mAssetProvider.OpenReadAsync(filename);
		}

		/// <summary>
		/// Opens the specified file returning a stream.  Throws 
		/// FileNotFoundException if the file does not exist.
		/// </summary>
		/// <param name="filename">The path and filename of the file to read from.</param>
		/// <returns></returns>
		public static Stream OpenRead(string filename)
		{
			return mAssetProvider.OpenRead(filename);
		}
		/// <summary>
		/// Checks to if the specified file exists in the file provider.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static bool FileExists(string filename)
		{
			return mAssetProvider.FileExists(filename);
		}

		/// <summary>
		/// Enumerates through all existing filenames in the file provider.
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<string> GetAllFiles()
		{
			return mAssetProvider.GetAllFiles();
		}

		/// <summary>
		/// Enumerates through all filenames which match the specified search pattern.
		/// </summary>
		/// <remarks>The search pattern is not regex style pattern matching, rather it should 
		/// be bash pattern matching, so a searchPattern of "*" would match all files, and
		/// "*.*" would match all filenames with a period in them.</remarks>
		/// <param name="searchPattern"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetAllFiles(string searchPattern)
		{
			return mAssetProvider.GetAllFiles(searchPattern);
		}
		/// <summary>
		/// Returns a string containing the entire contents of the specified file.
		/// </summary>
		/// <param name="filename">The path and filename of the file to read from.</param>
		/// <returns></returns>
		public static string ReadAllText(string filename)
		{
			return mAssetProvider.ReadAllText(filename);
		}

		/// <summary>
		/// Returns true if the specified filename points to an actual file on disk.
		/// If this method returns false, then ResolveFile will throw an exception
		/// for that file.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static bool IsRealFile(string filename)
		{
			return mAssetProvider.IsRealFile(filename);
		}

		/// <summary>
		/// Returns the full path of the given filename.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static string ResolveFile(string filename)
		{
			return mAssetProvider.ResolveFile(filename);
		}

		/// <summary>
		/// Returns true if the file system is not a physical file system.
		/// </summary>
		public static bool IsLogicalFilesystem { get { return mAssetProvider.IsLogicalFilesystem; } }
	}
}
