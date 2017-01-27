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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using AgateLib.Quality;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Configuration.State;

namespace AgateLib.IO
{
	[Obsolete("This is going away. Use AgateApp.Assets instead.")]
	public static class Assets
	{
		private static IOState State => AgateApp.State.IO;

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

			var coreAssets = AgateApp.State.App.Assets;
			AddOrCombine(ref coreAssets, assetProvider);
			AgateApp.State.App.Assets = coreAssets;

			AddOrCombine(ref AgateApp.State.IO.mImages, surfaces);
			AddOrCombine(ref AgateApp.State.IO.mResources, resources);
			AddOrCombine(ref AgateApp.State.IO.mMusic, music);
			AddOrCombine(ref AgateApp.State.IO.mSounds, sounds);
			AddOrCombine(ref AgateApp.State.IO.mUserInterfaceAssets, userInterfaceAssets);

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

		internal static IReadFileProvider AssetProvider
		{
			get { return AgateApp.State.App.Assets; }
			set { AgateApp.State.App.Assets = value; }
		}

		[Obsolete("Use your own read file provider to get subdirectories of the assets folder.")]
		public static IReadFileProvider Images
		{
			get { return State.mImages; }
			set { State.mImages = value; }
		}

		[Obsolete("Use your own read file provider to get subdirectories of the assets folder.")]
		public static IReadFileProvider Resources
		{
			get { return State.mResources; }
			set { State.mResources = value; }
		}

		[Obsolete("Use your own read file provider to get subdirectories of the assets folder.")]
		public static IReadFileProvider Music
		{
			get { return State.mMusic; }
			set { State.mMusic = value; }
		}

		[Obsolete("Use your own read file provider to get subdirectories of the assets folder.")]
		public static IReadFileProvider Sounds
		{
			get { return State.mSounds; }
			set { State.mSounds = value; }
		}

		[Obsolete("Use your own read file provider to get subdirectories of the assets folder.")]
		public static IReadFileProvider UserInterfaceAssets
		{
			get { return State.mUserInterfaceAssets; }
			set { State.mUserInterfaceAssets = value; }
		}

		/// <summary>
		/// Opens the specified file returning a stream.  Throws 
		/// FileNotFoundException if the file does not exist.
		/// </summary>
		/// <param name="filename">The path and filename of the file to read from.</param>
		/// <returns></returns>
		public static Task<Stream> OpenReadAsync(string filename)
		{
			return AgateApp.State.App.Assets.OpenReadAsync(filename);
		}

		/// <summary>
		/// Opens the specified file returning a stream.  Throws 
		/// FileNotFoundException if the file does not exist.
		/// </summary>
		/// <param name="filename">The path and filename of the file to read from.</param>
		/// <returns></returns>
		public static Stream OpenRead(string filename)
		{
			return AgateApp.State.App.Assets.OpenRead(filename);
		}
		/// <summary>
		/// Checks to if the specified file exists in the file provider.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static bool FileExists(string filename)
		{
			return AgateApp.State.App.Assets.FileExists(filename);
		}

		/// <summary>
		/// Enumerates through all existing filenames in the file provider.
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<string> GetAllFiles()
		{
			return AssetProvider.GetAllFiles();
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
			return AssetProvider.GetAllFiles(searchPattern);
		}
		/// <summary>
		/// Returns a string containing the entire contents of the specified file.
		/// </summary>
		/// <param name="filename">The path and filename of the file to read from.</param>
		/// <returns></returns>
		public static string ReadAllText(string filename)
		{
			return AssetProvider.ReadAllText(filename);
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
			return AssetProvider.IsRealFile(filename);
		}

		/// <summary>
		/// Returns the full path of the given filename.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static string ResolveFile(string filename)
		{
			return AssetProvider.ResolveFile(filename);
		}

		/// <summary>
		/// Returns true if the file system is not a physical file system.
		/// </summary>
		public static bool IsLogicalFilesystem => AssetProvider.IsLogicalFilesystem;
	}
}
