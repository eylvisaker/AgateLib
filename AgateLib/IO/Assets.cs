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

			AddOrCombine(ref Core.State.IO.mAssetProvider, assetProvider);
			AddOrCombine(ref Core.State.IO.mImages, surfaces);
			AddOrCombine(ref Core.State.IO.mResources, resources);
			AddOrCombine(ref Core.State.IO.mMusic, music);
			AddOrCombine(ref Core.State.IO.mSounds, sounds);
			AddOrCombine(ref Core.State.IO.mUserInterfaceAssets, userInterfaceAssets);

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
			get { return Core.State.IO.mAssetProvider; }
			set { Core.State.IO.mAssetProvider = value; }
		}

		public static IReadFileProvider Images
		{
			get { return Core.State.IO.mImages; }
			set { Core.State.IO.mImages = value; }
		}

		public static IReadFileProvider Resources
		{
			get { return Core.State.IO.mResources; }
			set { Core.State.IO.mResources = value; }
		}

		public static IReadFileProvider Music
		{
			get { return Core.State.IO.mMusic; }
			set { Core.State.IO.mMusic = value; }
		}

		public static IReadFileProvider Sounds
		{
			get { return Core.State.IO.mSounds; }
			set { Core.State.IO.mSounds = value; }
		}

		public static IReadFileProvider UserInterfaceAssets
		{
			get { return Core.State.IO.mUserInterfaceAssets; }
			set { Core.State.IO.mUserInterfaceAssets = value; }
		}

		/// <summary>
		/// Opens the specified file returning a stream.  Throws 
		/// FileNotFoundException if the file does not exist.
		/// </summary>
		/// <param name="filename">The path and filename of the file to read from.</param>
		/// <returns></returns>
		public static Task<Stream> OpenReadAsync(string filename)
		{
			return Core.State.IO.mAssetProvider.OpenReadAsync(filename);
		}

		/// <summary>
		/// Opens the specified file returning a stream.  Throws 
		/// FileNotFoundException if the file does not exist.
		/// </summary>
		/// <param name="filename">The path and filename of the file to read from.</param>
		/// <returns></returns>
		public static Stream OpenRead(string filename)
		{
			return Core.State.IO.mAssetProvider.OpenRead(filename);
		}
		/// <summary>
		/// Checks to if the specified file exists in the file provider.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static bool FileExists(string filename)
		{
			return Core.State.IO.mAssetProvider.FileExists(filename);
		}

		/// <summary>
		/// Enumerates through all existing filenames in the file provider.
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<string> GetAllFiles()
		{
			return Core.State.IO.mAssetProvider.GetAllFiles();
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
			return Core.State.IO.mAssetProvider.GetAllFiles(searchPattern);
		}
		/// <summary>
		/// Returns a string containing the entire contents of the specified file.
		/// </summary>
		/// <param name="filename">The path and filename of the file to read from.</param>
		/// <returns></returns>
		public static string ReadAllText(string filename)
		{
			return Core.State.IO.mAssetProvider.ReadAllText(filename);
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
			return Core.State.IO.mAssetProvider.IsRealFile(filename);
		}

		/// <summary>
		/// Returns the full path of the given filename.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static string ResolveFile(string filename)
		{
			return Core.State.IO.mAssetProvider.ResolveFile(filename);
		}

		/// <summary>
		/// Returns true if the file system is not a physical file system.
		/// </summary>
		public static bool IsLogicalFilesystem
		{
			get { return Core.State.IO.mAssetProvider.IsLogicalFilesystem; }
		}
	}
}
