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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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
			UserInterfaceAssets = NewProviderFromSubdirectory(Assets, assetLocations.UserInterface);
		}

		public static IReadFileProvider Assets { get; set; }

		public static IReadFileProvider SurfaceAssets { get; set; }
		public static IReadFileProvider ResourceAssets { get; set; }
		public static IReadFileProvider MusicAssets { get; set; }
		public static IReadFileProvider SoundAssets { get; set; }
		public static IReadFileProvider UserInterfaceAssets { get; set; }

		public static IReadWriteFileProvider UserFiles { get; set; }

		static IReadFileProvider NewProviderFromSubdirectory(IReadFileProvider parent, string subdir)
		{
			if (string.IsNullOrWhiteSpace(subdir) || subdir == ".")
				return parent;

			return new SubdirectoryProvider(parent, subdir);
		}

	}
}
